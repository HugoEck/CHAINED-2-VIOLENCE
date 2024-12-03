using UnityEngine;

/// <summary>
/// This script handles the collision logic for the fire chain segments
/// </summary>
public class FireChainSegment : MonoBehaviour
{
    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;

    [Header("Fire Segment attributes")]
    [SerializeField] private float _fireDamage;
    [SerializeField] private float _fireActiveDuration;
    float fireMultiplier = 5;
    private CapsuleCollider _enemyCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
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
    public float GetFireDamage 
    { 
        get { return _fireDamage; } 
    }
    public float GetFireActiveDuration
    {
        get { return _fireActiveDuration; }
    }
}
