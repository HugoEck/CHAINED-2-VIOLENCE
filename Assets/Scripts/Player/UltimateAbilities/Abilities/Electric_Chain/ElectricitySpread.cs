using UnityEngine;

/// <summary>
/// This script handles the spreading from enemy to enemy of electricity
/// </summary>
public class ElectricitySpread : MonoBehaviour
{
    public GameObject _electricityParticles;
    public float _electricityDamage { get; set; }
    public float _electricityActiveDuration { get; set; }
    public float _additionalElectricitySpreadRadius { get; set; }

    private float _electricityActiveTimer;

    private float _damageTimer;
    private float _damageInterval = 1f;

    private BaseManager enemyManager;

    private CapsuleCollider _enemyCollider;
    private CapsuleCollider _electricitySpreadRadius;

    private void Start()
    {     
        _damageTimer = 0f;

        if (enemyManager == null)
        {
            _enemyCollider = GetComponentInParent<CapsuleCollider>();

            enemyManager = GetComponentInParent<BaseManager>();
            _electricityActiveTimer = _electricityActiveDuration;

            if (_electricitySpreadRadius == null)
            {
                _electricitySpreadRadius = gameObject.AddComponent<CapsuleCollider>();
                _electricitySpreadRadius.isTrigger = true;

                CopyColliderValues(_enemyCollider, _electricitySpreadRadius);
                _electricitySpreadRadius.radius += 1.5f;
            }
            else
            {
                _electricitySpreadRadius.enabled = true;
            }

        }       
    }

    private void Update()
    {
        _electricityActiveTimer -= Time.deltaTime;

        if (_electricityActiveTimer <= 0)
        {
            _electricityActiveTimer = _electricityActiveDuration;
            gameObject.SetActive(false);      
            return;
        }

        _damageTimer += Time.deltaTime;
        if (_damageTimer >= _damageInterval)
        {
            if(enemyManager != null)
            {
                // Apply damage to the enemy
                enemyManager.DealDamageToEnemy(_electricityDamage);
                _damageTimer = 0f; // Reset the damage timer
            }         
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerNameToCompare = "Enemy";

        int layerToCompare = LayerMask.NameToLayer(layerNameToCompare);

        if (other.gameObject.layer == layerToCompare)
        {
            Transform electricity = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Electricity");

            if (electricity != null)
            {
                electricity.gameObject.SetActive(true);
            }
            else
            {
                // Create a new child GameObject
                GameObject electricityObject = new GameObject("Electricity");
                electricityObject.tag = "Electricity";
                // Set the new object as a child of the collided object
                electricityObject.transform.parent = other.transform;
                electricityObject.transform.position = other.transform.position;

                // Add the ElectricitySpread component to the new child GameObject
                ElectricitySpread newElectricity = electricityObject.AddComponent<ElectricitySpread>();
                newElectricity._electricityDamage = _electricityDamage;
                newElectricity._electricityActiveDuration = _electricityActiveDuration;
                newElectricity._additionalElectricitySpreadRadius = _additionalElectricitySpreadRadius;

                // Instantiate the electricity particles and set it as a child of electricityObject
                GameObject particlesInstance = Instantiate(_electricityParticles, electricityObject.transform);
                newElectricity._electricityParticles = _electricityParticles;

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
                        Vector3 meshSize = meshBounds.size * 2; // Get the size of the mesh

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
                        Vector3 meshSize = meshBounds.size * 2; // Get the size of the mesh

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
    
    /// <summary>
    /// The electricity collider needs to copy the original collider of the enemy to properly adjust for different enemy types 
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
