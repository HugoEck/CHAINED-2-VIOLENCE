using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
        ArnTestScene // FOR TESTING PURPOSES ONLY
    };

    private static Scene targetScene;

    /// <summary>
    /// Load a scene over the network
    /// </summary>
    /// <param name="targetScene"></param>
    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

}
