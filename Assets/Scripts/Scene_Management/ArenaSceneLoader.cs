using UnityEngine;
using UnityEngine.SceneManagement; // You need this for scene loading

public class ArenaSceneLoader : MonoBehaviour
{
    [SerializeField]
    private Loader.Scene sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            LoadNewScene();
        }
    }

    void LoadNewScene()
    {
        Loader.LoadScene(sceneToLoad);
        Chained2ViolenceGameManager.Instance.UpdateSceneState(Chained2ViolenceGameManager.SceneState.ArenaScene);
    }
}
