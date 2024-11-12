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

    [Header("Fire particles")]
    [SerializeField] private GameObject _fireParticles;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object's layer is within the specified layers to collide with
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
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
                newFire._fireParticles = particlesInstance;

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
                        Bounds meshBounds = skinnedMeshRenderer.bounds;
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
    public float GetFireDamage 
    { 
        get { return _fireDamage; } 
    }
    public float GetFireActiveDuration
    {
        get { return _fireActiveDuration; }
    }
}
