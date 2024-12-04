using UnityEngine;

/// <summary>
/// This script handles how long the ground fire will be active and spreads the fire from the ground to the enemy that collides with it
/// </summary>
public class GroundFire : MonoBehaviour
{
    [SerializeField] private GameObject _fireParticles;

    [SerializeField] private float _groundFireActiveDuration;
    CapsuleCollider _enemyCollider;
    private float _groundFireActiveTimer;
    float fireMultiplier = 5;


    [HideInInspector]
    public bool bIsGroundFireActive = true;

    private FireChainSegment _fireChainSegment;

    private float _fireDamage;
    private float _fireActiveDuration;

    private void Start()
    {
        _fireChainSegment = FindObjectOfType<FireChainSegment>();

        _fireDamage = _fireChainSegment.GetFireDamage;
        _fireActiveDuration = _fireChainSegment.GetFireActiveDuration;

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
            _enemyCollider = other.GetComponentInParent<CapsuleCollider>();
            Transform fire = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Fire");

            if (fire.gameObject.activeInHierarchy) return;

            if (fire != null)
            {
                // Get the ParticleSystem component
                ParticleSystem particleSystem = fire.GetComponent<ParticleSystem>();
                FireSpread spreadLogic = fire.GetComponent<FireSpread>();

                spreadLogic._fireActiveDuration = _fireActiveDuration;
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
