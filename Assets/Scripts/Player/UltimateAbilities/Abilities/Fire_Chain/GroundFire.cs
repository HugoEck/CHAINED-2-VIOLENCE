using UnityEngine;

/// <summary>
/// This script handles how long the ground fire will be active and spreads the fire from the ground to the enemy that collides with it
/// </summary>
public class GroundFire : MonoBehaviour
{
    [SerializeField] private GameObject _fireParticles;

    [SerializeField] private float _groundFireActiveDuration;  

    private float _groundFireActiveTimer;

    [HideInInspector]
    public bool bIsGroundFireActive = true;

    private FireChainSegment _fireChainSegment;

    private void Start()
    {
        _fireChainSegment = FindObjectOfType<FireChainSegment>();

        _groundFireActiveTimer = _groundFireActiveDuration;
    }

    /// <summary>
    /// This method is called from the SpawnGroundFire class
    /// </summary>
    public void UpdateGroundFire()
    {
        _groundFireActiveTimer -= Time.deltaTime;

        if (_groundFireActiveTimer <= 0)
        {
            bIsGroundFireActive = false;          
            return;
        }
    }
    
    private void OnEnable()
    {
        bIsGroundFireActive = true;
        _groundFireActiveTimer = _groundFireActiveDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerNameToCompare = "Enemy";

        int layerToCompare = LayerMask.NameToLayer(layerNameToCompare);

        if (other.gameObject.layer == layerToCompare)
        {
            Transform fire = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Fire");

            if (fire != null)
            {
                fire.gameObject.SetActive(true);
            }
            else
            {
                // Create a new child GameObject
                GameObject fireObject = new GameObject("Fire");
                fireObject.tag = "Fire";
                // Set the new object as a child of the collided object
                fireObject.transform.parent = other.transform;
                fireObject.transform.position = other.transform.position;

                // Add the ElectricitySpread component to the new child GameObject
                FireSpread newFire = fireObject.AddComponent<FireSpread>();
                newFire._fireDamage =  _fireChainSegment.GetFireDamage;
                newFire._fireActiveDuration = _fireChainSegment.GetFireActiveDuration;

                // Instantiate the electricity particles and set it as a child of electricityObject
                GameObject particlesInstance = Instantiate(_fireParticles, fireObject.transform);
                newFire._fireParticles = _fireParticles;

                // Get the ParticleSystem component
                ParticleSystem particleSystem = particlesInstance.GetComponent<ParticleSystem>();

                // Access the Shape module
                ParticleSystem.ShapeModule shapeModule = particleSystem.shape;

                // Check for MeshRenderer or SkinnedMeshRenderer
                MeshRenderer meshRenderer = other.GetComponentInChildren<MeshRenderer>();
                SkinnedMeshRenderer skinnedMeshRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>();

                if (meshRenderer != null)
                {
                    // Normal MeshRenderer found
                    MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
                    if (meshFilter != null && meshFilter.sharedMesh != null)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.MeshRenderer; // Set to Mesh
                        shapeModule.meshRenderer = meshRenderer;

                        // Calculate the size of the mesh
                        Bounds meshBounds = meshFilter.sharedMesh.bounds;
                        Vector3 meshSize = meshBounds.size; // Get the size of the mesh

                        // Set particle system size based on mesh size
                        var mainModule = particleSystem.main;
                        mainModule.startSizeX = meshSize.x; // Scale X
                        mainModule.startSizeY = meshSize.y; // Scale Y
                        mainModule.startSizeZ = meshSize.z; // Scale Z
                    }
                    else
                    {
                        Debug.LogWarning("No valid MeshFilter found for the MeshRenderer.");
                    }
                }
                else if (skinnedMeshRenderer != null)
                {
                    // SkinnedMeshRenderer found
                    if (skinnedMeshRenderer.sharedMesh != null)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer; // Set to Skinned Mesh
                        shapeModule.skinnedMeshRenderer = skinnedMeshRenderer;

                        // Calculate the size of the skinned mesh
                        Bounds meshBounds = skinnedMeshRenderer.sharedMesh.bounds;
                        Vector3 meshSize = meshBounds.size; // Get the size of the mesh

                        // Set particle system size based on mesh size
                        var mainModule = particleSystem.main;
                        mainModule.startSizeX = meshSize.x; // Scale X
                        mainModule.startSizeY = meshSize.y; // Scale Y
                        mainModule.startSizeZ = meshSize.z; // Scale Z
                    }
                    else
                    {
                        Debug.LogWarning("No valid shared mesh found for the SkinnedMeshRenderer.");
                    }
                }
                else
                {
                    Debug.LogWarning("No MeshRenderer or SkinnedMeshRenderer found in the hierarchy.");
                }

            }
        }
    }
}
