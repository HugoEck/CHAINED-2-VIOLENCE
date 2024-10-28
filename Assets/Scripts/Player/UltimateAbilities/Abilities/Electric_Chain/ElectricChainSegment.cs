using UnityEngine;

public class ElectricChainSegment : MonoBehaviour
{
    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;

    [Header("Electric Segment attributes")]
    [SerializeField] private float _electricityDamage;
    [SerializeField] private float _electricityActiveDuration;

    [Header("Electricity particles")]
    [SerializeField] private GameObject _electricityParticles;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object's layer is within the specified layers to collide with
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
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

                // Instantiate the electricity particles and set it as a child of electricityObject
                GameObject particlesInstance = Instantiate(_electricityParticles, electricityObject.transform);
                newElectricity._electricityParticles = particlesInstance;

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
}
