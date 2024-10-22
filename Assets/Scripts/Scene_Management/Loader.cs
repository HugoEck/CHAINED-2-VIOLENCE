using UnityEngine.SceneManagement;

/// <summary>
/// This script handles the loading of the scenes
/// </summary>
public static class Loader
{
    public enum Scene
    {
        Main_Menu,
        MainScene,
        ArnTestScene, // FOR TESTING PURPOSES ONLY
        LobbyScene
    };

    private static Scene targetScene;

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="targetScene"></param>
    public static void LoadScene(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
