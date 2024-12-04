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
    public BaseManager enemyManager { get; set; }

    public CapsuleCollider _enemyCollider { get; set; }
    private CapsuleCollider _fireSpreadRadius;

    private float InitialActiveDuration;

    private void Start()
    {
        InitialActiveDuration = _fireActiveDuration;
        _fireActiveTimer = InitialActiveDuration;
        _damageTimer = 1f;

        _fireSpreadRadius = gameObject.GetComponent<CapsuleCollider>();
        _fireSpreadRadius.radius = 1.5f;
        _fireSpreadRadius.height = 1.5f;
    }

    private void Update()
    {
        _fireActiveTimer -= Time.deltaTime;

        if (_fireActiveTimer <= 0)
        {
            _fireActiveTimer = InitialActiveDuration;
            gameObject.SetActive(false);
            return;
        }

        _damageTimer += Time.deltaTime;
        if (_damageTimer >= _damageInterval)
        {
            // Apply damage to the enemy
            enemyManager.DealDamageToEnemy(_fireDamage, BaseManager.DamageType.UltimateFire);
            _damageTimer = 0f; // Reset the damage timer
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerNameToCompare = "Enemy";

        int layerToCompare = LayerMask.NameToLayer(layerNameToCompare);

        if (other.gameObject.layer == layerToCompare)
        {
            _enemyCollider = other.GetComponentInParent<CapsuleCollider>();
            Transform fire = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Fire");

            if (fire.gameObject.activeInHierarchy) return;

            if (fire != null)
            {
                // Get the ParticleSystem component
                ParticleSystem particleSystem = fire.GetComponent<ParticleSystem>();
                FireSpread spreadLogic = fire.GetComponent<FireSpread>();

                spreadLogic._fireActiveDuration = InitialActiveDuration;
                spreadLogic._fireDamage = _fireDamage;
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

                fire.gameObject.SetActive(true);
            }
        }
    }
}
