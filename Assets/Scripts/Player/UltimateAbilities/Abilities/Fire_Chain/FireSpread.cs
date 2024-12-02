using UnityEngine;

/// <summary>
/// This script handles the spreading of the fire to enemies if they collide with another enemy that is inflicted with fire
/// </summary>
public class FireSpread : MonoBehaviour
{
    public GameObject _fireParticles;
    public float _fireDamage { get; set; }
    public float _fireActiveDuration { get; set; }
    private float _additionalSpreadRadius = 0.5f;

    private float _fireActiveTimer;

    private float _damageTimer;
    private float _damageInterval = 1f;
    float fireMultiplier = 5;
    private BaseManager enemyManager;

    private CapsuleCollider _enemyCollider;
    private CapsuleCollider _fireSpreadRadius;

    private void Start()
    {
        _damageTimer = 0f;

        if (enemyManager == null)
        {
            _enemyCollider = GetComponentInParent<CapsuleCollider>();

            enemyManager = GetComponentInParent<BaseManager>();
            _fireActiveTimer = _fireActiveDuration;

            if (_fireSpreadRadius == null)
            {
                _fireSpreadRadius = gameObject.AddComponent<CapsuleCollider>();
                _fireSpreadRadius.isTrigger = true;

                CopyColliderValues(_enemyCollider, _fireSpreadRadius);
                _fireSpreadRadius.radius += _additionalSpreadRadius;
            }
            else
            {
                _fireSpreadRadius.enabled = true;
            }

        }
    }

    private void Update()
    {
        _fireActiveTimer -= Time.deltaTime;

        if (_fireActiveTimer <= 0)
        {
            _fireActiveTimer = _fireActiveDuration;
            gameObject.SetActive(false);
            return;
        }

        _damageTimer += Time.deltaTime;
        if (_damageTimer >= _damageInterval)
        {
            // Apply damage to the enemy
            enemyManager.DealDamageToEnemy(_fireDamage, BaseManager.DamageType.UltimateAbilityDamage);
            _damageTimer = 0f; // Reset the damage timer
        }
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
                newFire._fireDamage = _fireDamage;
                newFire._fireActiveDuration = _fireActiveDuration;

                // Instantiate the electricity particles and set it as a child of electricityObject
                GameObject particlesInstance = Instantiate(_fireParticles, fireObject.transform);
                newFire._fireParticles = _fireParticles;

                // Get the ParticleSystem component
                ParticleSystem particleSystem = particlesInstance.GetComponent<ParticleSystem>();

                // Access the Shape module
                ParticleSystem.ShapeModule shapeModule = particleSystem.shape;

                // Check for MeshRenderer or SkinnedMeshRenderer
                MeshRenderer meshRenderer = other.GetComponentInChildren<MeshRenderer>(false);
                SkinnedMeshRenderer skinnedMeshRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>(false);

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
                        mainModule.startSizeX = _enemyCollider.radius * fireMultiplier; // Scale X
                        mainModule.startSizeY = _enemyCollider.height * fireMultiplier; // Scale Y
                        mainModule.startSizeZ = _enemyCollider.radius * fireMultiplier; // Scale Z
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
                        Bounds meshBounds = skinnedMeshRenderer.bounds;
                        Vector3 meshSize = meshBounds.size; // Get the size of the mesh

                        // Set particle system size based on mesh size
                        var mainModule = particleSystem.main;
                        mainModule.startSizeX = _enemyCollider.radius * fireMultiplier; // Scale X
                        mainModule.startSizeY = _enemyCollider.height * fireMultiplier; // Scale Y
                        mainModule.startSizeZ = _enemyCollider.radius * fireMultiplier; // Scale Z
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

    /// <summary>
    /// The fire spread collider needs to inherit the normal collider to make proporions correct
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    private void CopyColliderValues(CapsuleCollider source, CapsuleCollider target)
    {
        if (source != null && target != null)
        {
            target.center = source.center; // Copy center
            target.height = source.height; // Copy height
            target.radius = source.radius; // Copy radius
            target.direction = source.direction; // Copy direction
        }
        else
        {
            Debug.LogWarning("One of the colliders is null.");
        }
    }
}
