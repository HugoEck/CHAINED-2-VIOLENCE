using UnityEngine;

// Singleton script for helper functions that are used for managing object hierarchies
public class ObjectHierarchyHelper : MonoBehaviour
{
    private static ObjectHierarchyHelper instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it is not the singleton
            return;
        }

        instance = this; // Assign this instance to the singleton
        DontDestroyOnLoad(gameObject); // Make this object persist across scenes
    }

    public static ObjectHierarchyHelper Instance
    {
        get
        {           
            if (instance == null)
            {
                Debug.LogWarning("ObjectHierarchyHelper instance is null. Make sure it is attached to a GameObject in the scene.");
            }
            return instance;
        }
    }
    /// <summary>
    /// Find first instance of child with a specific tag
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static Transform FindChildWithTag(GameObject parent, string tag)
    {
        // Check each child object for specific tag
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        // No child with tag is found
        return null;
    }
}
