using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> songList;


    private static MusicManager _instance;
    public static MusicManager Instance
    // Property to access the instance
    {
        #region Singleton
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("MusicManager").GetComponent<MusicManager>();

                // If no instance is found, create a new GameObject and add the SingletonManager to it
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MusicManager");
                    _instance = singletonObject.AddComponent<MusicManager>();
                }

                // Make sure this instance persists across scene changes
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
        #endregion Singleton
    }

    // Start is called before the first frame update
    void Awake()
    {
        #region Singleton
        // If there's already an instance, destroy this duplicate
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign the instance to this object if it's the first one
        _instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion Singleton
    }
}
