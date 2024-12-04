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
    public BaseManager enemyManager { get; set; }
    public CapsuleCollider _enemyCollider { get; set; }

    private float _electricityActiveTimer;

    private float _damageTimer;
    private float _damageInterval = 1f;
    
    private float electricityMultiplier = 10;
    
    private CapsuleCollider _electricitySpreadRadius;

    private float InitialActiveDuration;

    private void Start()
    {
        InitialActiveDuration = _electricityActiveDuration;
        _electricityActiveTimer = InitialActiveDuration;
        _damageTimer = 1f;
        
        _electricitySpreadRadius = gameObject.GetComponent<CapsuleCollider>();
        _electricitySpreadRadius.radius = 0.1f;
        _electricitySpreadRadius.height = 0.1f;

    }

    private void Update()
    {
        _electricityActiveTimer -= Time.deltaTime;

        
        if (_electricityActiveTimer <= 0)
        {
            _electricityActiveTimer = InitialActiveDuration;
            gameObject.SetActive(false);      
            return;
        }

        _damageTimer += Time.deltaTime;
        if (_damageTimer >= _damageInterval)
        {
            if(enemyManager != null)
            {
                // Apply damage to the enemy
                enemyManager.chainEffects.ActivateShockChainEffect(_electricityActiveTimer);
                enemyManager.DealDamageToEnemy(_electricityDamage, BaseManager.DamageType.UltimateElectricity);
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
            _enemyCollider = other.GetComponentInParent<CapsuleCollider>();
            Transform electricity = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Electricity");

            if (electricity.gameObject.activeInHierarchy) return;

            if (electricity != null)
            {
                // Get the ParticleSystem component
                ParticleSystem particleSystem = electricity.GetComponent<ParticleSystem>();
                ElectricitySpread spreadLogic = electricity.GetComponent<ElectricitySpread>();

                spreadLogic._electricityActiveDuration = InitialActiveDuration;
                spreadLogic._electricityDamage = _electricityDamage;
                spreadLogic.enemyManager = other.GetComponent<BaseManager>();
                spreadLogic._enemyCollider = _enemyCollider;
                // Access the Shape module
                ParticleSystem.ShapeModule shapeModule = particleSystem.shape;

                SkinnedMeshRenderer skinnedMeshRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>(false);

                if (skinnedMeshRenderer != null)
                {
                    if (skinnedMeshRenderer.sharedMesh != null)
                    {
                        shapeModule.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer; // Set to Skinned Mesh
                        shapeModule.skinnedMeshRenderer = skinnedMeshRenderer;

                        // Calculate the size of the skinned mesh
                        Bounds meshBounds = skinnedMeshRenderer.bounds;
                        Vector3 meshSize = meshBounds.size; // Get the size of the mesh

                        // Set particle system size based on mesh size
                        var mainModule = particleSystem.main;
                        mainModule.startSizeX = _enemyCollider.radius * electricityMultiplier; // Scale X
                        mainModule.startSizeY = _enemyCollider.height * electricityMultiplier; // Scale Y
                        mainModule.startSizeZ = _enemyCollider.radius * electricityMultiplier; // Scale Z
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

               electricity.gameObject.SetActive(true);
            }
        }
    }
}
