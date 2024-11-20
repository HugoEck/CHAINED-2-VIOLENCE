using UnityEngine.SceneManagement;

/// <summary>
/// This script handles the loading of the scenes
/// </summary>
public static class Loader
{
    public enum Scene
    {
        Main_Menu,
        Arena,
        ArnTestScene, // FOR TESTING PURPOSES ONLY
        LobbyScene,
        PlaygroundScene,
        ArenaDuplicate, // FOR TESTING PURPOSES ONLY
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
