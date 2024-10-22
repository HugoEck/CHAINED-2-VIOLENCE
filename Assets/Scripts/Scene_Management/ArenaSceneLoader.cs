using UnityEngine;
using UnityEngine.SceneManagement; // You need this for scene loading

public class ArenaSceneLoader : MonoBehaviour
{
    // This function will be triggered when something enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone has the tag "Player"
        if (other.CompareTag("Player1"))
        {
            // Call the function to load the new scene
            LoadNewScene();
        }
    }

    // This function loads the new scene and automatically unloads the old one
    void LoadNewScene()
    {
        Loader.LoadScene(Loader.Scene.Arena);
    }
}
