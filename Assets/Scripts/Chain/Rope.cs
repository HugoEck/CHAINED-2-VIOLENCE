using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is for simulating the chain physics (PLACEHOLDER NAME ROPE, WILL BE CHAIN)
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [Header("Chain attributes")]
    [SerializeField] private int _iterations = 5;
    [SerializeField] private float _gravity = 10.0f;
    [SerializeField] private float _damping = 0.7f;
    [SerializeField] private float _chainForceMultiplier = 10.0f;
    [SerializeField] private MeshFilter _chainMeshFilter;

    [Header("Attach endpoints")]
    [SerializeField] private Transform _player1Attachment;
    [SerializeField] private Transform _player2Attachment;
    [SerializeField] private PlayerMovement _player1Movement;
    [SerializeField] private PlayerMovement _player2Movement;
    [SerializeField] private float _maxPlayerVelocity = 10.0f;

    [Header("Temporary collision")]
    [SerializeField] private float _meshThickness = 1.0f;

    [Header("Path")]
    [SerializeField] private PathAutoEndPoints _vertexPathInfo;

    [Header("Chain")]
    [SerializeField] private GameObject _chainLinkPrefab;      // Reference to the Torus prefab

    private List<GameObject> _chainLinks = new List<GameObject>();  // Store references to the instantiated torus objects
    private List<ChainSegment> _chainSegments = new List<ChainSegment>();

    private Rigidbody _player1RigidBody;
    private Rigidbody _player2RigidBody;

    private int _numberOfPointsAlongPath = 20;
    
    private float _maxRopeLength = 5.0f;  // Maximum stretch length for the rope
    private float _pathLength = 0;
    private float _pointSpacing = 0;
    private Vector3[] _points;
    private Vector3[] _pointsOld;

    private bool _pinStartPoint = true;
    private bool _pinEndPoint = true;

   
    void Start()
    {
        // Get the rigid body from the player object (parent), not the chain holder
        _player1RigidBody = _player1Attachment.GetComponentInParent<Rigidbody>();
        _player2RigidBody = _player2Attachment.GetComponentInParent<Rigidbody>();

        // Calculate the total path length using the Path Creator tool
        CalculatePathLength();

        // Define a scaling factor based on the relationship between path length and number of points (number is based on how many points felt good for a path length that was x long)
        const float pointsPerUnitLength = 7.42f;

        // Set the number of points based on path length
        _numberOfPointsAlongPath = Mathf.Max(3, Mathf.RoundToInt(_pathLength * pointsPerUnitLength)); // Minimum 3 points
        Debug.Log($"Calculated numPoints: {_numberOfPointsAlongPath}");

        
        _points = new Vector3[_numberOfPointsAlongPath];
        _pointsOld = new Vector3[_numberOfPointsAlongPath];

        // Initialize the points along the path
        for (int i = 0; i < _numberOfPointsAlongPath; i++)
        {
            float t = i / (_numberOfPointsAlongPath - 1f);
            _points[i] = _vertexPathInfo.pathCreator.path.GetPointAtTime(t, PathCreation.EndOfPathInstruction.Stop);
            _pointsOld[i] = _points[i];
        }

        // Calculate the spacing between points
        _pointSpacing = _pathLength / _numberOfPointsAlongPath;

        // Calculate max rope length using a non-linear scaling based on path length
        // Here we use a scaling exponent to account for the observed behavior at longer chain lengths
        const float baseScaleFactor = 0.75f; // Adjust this for fine-tuning
        const float exponentFactor = 0.8f;  // Introduce a square root like scaling for longer ropes

        _maxRopeLength = _pathLength * (baseScaleFactor + Mathf.Pow(_pathLength, exponentFactor) / 10f);
        Debug.Log($"Calculated Max Rope Length: {_maxRopeLength}");

        // instantiate chain segments
        GenerateChain();
    }

    /// <summary>
    /// Calculate the length of the path based on the vertex curve (check component on gameobject)
    /// </summary>
    void CalculatePathLength()
    {
        // Reset path length for dynamic adjustment 
        _pathLength = 0;

        // Recalculate the total length of the path based on the points (adding to the pathLength by adding the distance between each point in the path)
        Vector3 previousPoint = _vertexPathInfo.pathCreator.path.GetPointAtTime(0, PathCreation.EndOfPathInstruction.Stop);
        for (int i = 1; i <= 100; i++) 
        {
            float t = i / 100f;
            Vector3 currentPoint = _vertexPathInfo.pathCreator.path.GetPointAtTime(t, PathCreation.EndOfPathInstruction.Stop);
            _pathLength += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }

    void LateUpdate()
    {       
        UpdateChain();
    }

    /// <summary>
    ///  Method to instantiate chain segments 
    /// </summary>
    void GenerateChain()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Vector3 position = _points[i];

            // Instantiate the torus prefab at each point along the rope
            GameObject torusInstance = Instantiate(_chainLinkPrefab, position, Quaternion.identity);

            _chainSegments.Add(torusInstance.GetComponent<ChainSegment>());
         
            // Add the instantiated torus to the list so we can update it later
            _chainLinks.Add(torusInstance);
        }
    }


    /// <summary>
    /// Method to update the position and rotation of the chain segments in LateUpdate
    /// </summary>
    void UpdateChain()
    {
        for (int i = 0; i < _points.Length && i < _chainLinks.Count; i++)
        {
            Vector3 position = _points[i];
            _chainLinks[i].transform.position = position;

            // Update rotation for the first segment based on the second segment
            if (i == 0 && _points.Length > 1)
            {
                Vector3 directionToSecond = (_points[1] - _points[0]).normalized; // Direction to the second point
                if (directionToSecond != Vector3.zero) // Ensure it's not a zero vector
                {
                    Quaternion rotation = Quaternion.LookRotation(directionToSecond); // Rotate to face the second point
                    _chainLinks[i].transform.rotation = rotation;
                }
            }
            else if (i > 0) // For all segments after the first
            {
                Vector3 direction = _points[i - 1] - position; // Direction from current point to the previous point
                Quaternion rotation = Quaternion.LookRotation(direction); // Rotate to face the previous point
                _chainLinks[i].transform.rotation = rotation;
            }

            _chainLinks[i].transform.rotation *= Quaternion.Euler(0, 90, 0);

            // Rotation on every other chain to get the "chain effect"
            if (i % 2 == 1)
            {
                _chainLinks[i].transform.rotation *= Quaternion.Euler(90, 0, 0); // Add an additional 90-degree twist
            }
        }
    }


    void FixedUpdate()
    {
        // Update the first and last point positions to always fix their positions at the targets
        _points[0] = _vertexPathInfo.origin.position;
        _points[_numberOfPointsAlongPath - 1] = _vertexPathInfo.target.position;

        for (int i = 1; i < _points.Length - 1; i++)  
        {       
            // Don't update the physics of the first and last point if they are pinned
            bool pinned = (i == 0 && _pinStartPoint) || (i == _points.Length - 1 && _pinEndPoint);

            if (!pinned)
            {
                Vector3 curr = _points[i];
                _points[i] = _points[i] + (_points[i] - _pointsOld[i]) * _damping + Vector3.down * _gravity * Time.deltaTime * Time.deltaTime;
                _pointsOld[i] = curr;
            }
        }

        for (int i = 0; i < _iterations; i++)
        {
            ConstrainCollisions();
            ConstrainConnections();
        }

        DragOtherPlayer();
    }

    /// <summary>
    /// Apply a pulling force through the rope and into both players when the rope is "stretched"
    /// </summary>
    private void DragOtherPlayer()
    {
        // Calculate the distance between the two players
        float currentRopeLength = Vector3.Distance(_points[0], _points[_points.Length - 1]);

        // Get the rigidbody states
        bool isPlayer1Kinematic = _player1RigidBody.isKinematic;
        bool isPlayer2Kinematic = _player2RigidBody.isKinematic;

        // If both players are not kinematic, apply forces based on speed and mass
        float massPlayer1 = _player1RigidBody.mass;
        float massPlayer2 = _player2RigidBody.mass;

        // Get the speeds of the players
        float player1Speed = _player1RigidBody.velocity.magnitude; // Speed of Player1
        float player2Speed = _player2RigidBody.velocity.magnitude; // Speed of Player2

        // Calculate base pulling force based on mass and speed
        float baseForcePlayer1 = massPlayer1 * player1Speed * _chainForceMultiplier; // Adjust forceMultiplier for scaling
        float baseForcePlayer2 = massPlayer2 * player2Speed * _chainForceMultiplier; // Adjust forceMultiplier for scaling

        // Calculate total force for each player
        float totalForce = baseForcePlayer1 + baseForcePlayer2;

        // Calculate individual forces as a percentage of total force
        float player1Force = baseForcePlayer1 / totalForce;
        float player2Force = baseForcePlayer2 / totalForce;

        // This variable is used so that the players never "not" pull eachother ensuring that no infinite stretching happens
        const float minimumForce = 10f; // Adjust as necessary

        // If the rope is stretched, apply forces and adjust player speeds
        if (currentRopeLength > _maxRopeLength)
        {
            player1Force = Mathf.Max(player1Force * totalForce, minimumForce);
            player2Force = Mathf.Max(player2Force * totalForce, minimumForce);

            
            float ropeStretch = currentRopeLength - _maxRopeLength;
            float stretchFactor = Mathf.Clamp01(ropeStretch / 2.0f); // Max stretch impact

            // Player 1 - Calculate movement direction relative to the rope
            Vector3 directionToRopeStart = (_player2Attachment.position - _player1Attachment.position).normalized;
            float dotProductPlayer1 = Vector3.Dot(_player1RigidBody.velocity.normalized, directionToRopeStart);

            // Adjust Player1's speed based on direction of movement relative to the rope
            if (dotProductPlayer1 < 0) // Player 1 is moving away from the rope and stretching it 
            {
                float newWalkingSpeed1 = _player1Movement.originalWalkingSpeed * Mathf.Lerp(1f, 0.0f, stretchFactor);
                _player1Movement.SetPlayerWalkingSpeed(newWalkingSpeed1);
            }
            else // Player 1 is moving towards slack, allow normal speed
            {
                _player1Movement.SetPlayerWalkingSpeed(_player1Movement.originalWalkingSpeed);
            }

            // Player 2 - Calculate movement direction relative to the rope
            Vector3 directionToRopeEnd = (_player1Attachment.position - _player2Attachment.position).normalized;
            float dotProductPlayer2 = Vector3.Dot(_player2RigidBody.velocity.normalized, directionToRopeEnd);

            // Adjust Player2's speed based on direction of movement relative to the rope
            if (dotProductPlayer2 < 0) // Player 2 is moving away from the rope and stretching it 
            {
                float newWalkingSpeed2 = _player2Movement.originalWalkingSpeed * Mathf.Lerp(1f, 0.0f, stretchFactor);
                _player2Movement.SetPlayerWalkingSpeed(newWalkingSpeed2);
            }
            else // Player 2 is moving towards slack, allow normal speed
            {
                _player2Movement.SetPlayerWalkingSpeed(_player2Movement.originalWalkingSpeed);
            }

            // Directions towards the players
            Vector3 directionToPlayer2 = (_points[0] - _player2RigidBody.position).normalized; 
            Vector3 directionToPlayer1 = (_points[_numberOfPointsAlongPath - 1] - _player1RigidBody.position).normalized; 

            // Apply forces proportionally to the calculated strengths in the correct directions
            ApplyForceToPlayers(directionToPlayer1 * player2Force,
                                directionToPlayer2 * player1Force);

            // Clamp player velocities so that they can't be flung too fast and break the physics simulation

            ClampVelocity(_player1RigidBody, _maxPlayerVelocity);
            
        }
        else
        {
            // If the rope is not stretched, set normal walking speed
            _player1Movement.SetPlayerWalkingSpeed(_player1Movement.originalWalkingSpeed);
            _player2Movement.SetPlayerWalkingSpeed(_player2Movement.originalWalkingSpeed);
        }
    }

    private void ApplyForceToPlayers(Vector3 forceOnPlayer1, Vector3 forceOnPlayer2)
    {
        _player1RigidBody.AddForce(forceOnPlayer1);
        _player2RigidBody.AddForce(forceOnPlayer2);

        
    }


    /// <summary>
    /// Clamp player velocity so that they don't get flung too fast
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="maxVelocity"></param>
    private void ClampVelocity(Rigidbody rb, float maxVelocity)
    {
        // Get the current velocity
        Vector3 currentVelocity = rb.velocity;

        // Calculate the horizontal velocity magnitude (ignore the y component)
        float horizontalMagnitude = new Vector3(currentVelocity.x, 0, currentVelocity.z).magnitude;

        // If the horizontal magnitude exceeds maxVelocity, clamp it
        if (horizontalMagnitude > maxVelocity)
        {
            // Calculate the new horizontal velocity
            Vector3 clampedHorizontalVelocity = (new Vector3(currentVelocity.x, 0, currentVelocity.z)).normalized * maxVelocity;

            // Set the Rigidbody's velocity with the clamped horizontal component and preserve the vertical component
            rb.velocity = new Vector3(clampedHorizontalVelocity.x, currentVelocity.y, clampedHorizontalVelocity.z);
        }
    }

    /// <summary>
    /// Always make sure that the points along the path are at a constrained distance from eachother, thus simulating the "chain" effect
    /// </summary>
    void ConstrainConnections()
    {
        for (int iteration = 0; iteration < _iterations; iteration++)
        {
            for (int i = 0; i < _points.Length - 1; i++)
            {
                Vector3 centre = (_points[i] + _points[i + 1]) / 2;
                Vector3 offset = _points[i] - _points[i + 1];
                float length = offset.magnitude;
                Vector3 dir = length > 0 ? offset / length : Vector3.zero; // Prevent division by zero

                // Calculate desired positions based on fixed distance
                Vector3 desiredPositionA = centre + dir * (_pointSpacing / 2);
                Vector3 desiredPositionB = centre - dir * (_pointSpacing / 2);

                // Move points towards their desired positions with a smoothing factor
                float smoothingFactor = 1000f; // Adjust for how strongly you want to enforce the constraint
                if (i != 0 || !_pinStartPoint)
                {
                    _points[i] = Vector3.Lerp(_points[i], desiredPositionA, smoothingFactor);
                }
                if (i + 1 != _points.Length - 1 || !_pinEndPoint)
                {
                    _points[i + 1] = Vector3.Lerp(_points[i + 1], desiredPositionB, smoothingFactor);
                }
            }
        }
    }

    /// <summary>
    /// PLACEHOLDER COLLISION DETECTION, CURRENTLY ONLY FOR CONSTRAINING THE Y POSITION OF THE POINTS FOR THE GROUND
    /// </summary>
    void ConstrainCollisions()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            bool pinned = i == 0 || i == _points.Length - 1;

            if (!pinned)
            {
                if (_points[i].y < _meshThickness / 2)
                {
                    _points[i].y = _meshThickness / 2;
                }
            }
        }
    }

    /// <summary>
    /// Draw all the points for debugging purposes
    /// </summary>
    void OnDrawGizmos()
    {
        if (_points != null)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                Gizmos.DrawSphere(_points[i], 0.05f);
            }
        }
    }
}


