using UnityEngine;

/// <summary>
/// This script handles the collision logic for the electric chain segments
/// </summary>
public class ElectricChainSegment : MonoBehaviour
{
    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;

    [Header("Electric Segment attributes")]
    [SerializeField] private float _electricityDamage;
    [SerializeField] private float _electricityActiveDuration;
    [SerializeField] private float _additionalElectricitySpreadRadius = 1.0f;
    private float electricityMultiplier = 10;


    private CapsuleCollider _enemyCollider;

    private void OnTriggerEnter(Collider other)
    {
        

        // Check if the collided object's layer is within the specified layers to collide with
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
        {
            _enemyCollider = other.GetComponentInParent<CapsuleCollider>();
            Transform electricity = ObjectHierarchyHelper.FindChildWithTag(other.gameObject, "Electricity");

            if (electricity.gameObject.activeInHierarchy) return;
         
            if (electricity != null)
            {
                // Get the ParticleSystem component
                ParticleSystem particleSystem = electricity.GetComponent<ParticleSystem>();
                ElectricitySpread spreadLogic = electricity.GetComponent<ElectricitySpread>();

                spreadLogic._electricityActiveDuration = _electricityActiveDuration;
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
