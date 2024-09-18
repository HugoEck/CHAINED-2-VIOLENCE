using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ThemeData
{
    [Header("Basic")]
    public List<GameObject> basicHelmets;
    public List<GameObject> basicCapes;
    public List<GameObject> basicBodies;
    public List<GameObject> basicWeapons;
    public List<GameObject> basicShields;
    [Header("Runner")]
    public List<GameObject> runnerHelmets;
    public List<GameObject> runnerCapes;
    public List<GameObject> runnerBodies;
    public List<GameObject> runnerWeapons;
    public List<GameObject> runnerShields;
    [Header("Warrior")]
    public List<GameObject> warriorHelmets;
    public List<GameObject> warriorCapes;
    public List<GameObject> warriorBodies;
    public List<GameObject> warriorWeapons;
    public List<GameObject> warriorShields;
    [Header("Tank")]
    public List<GameObject> tankHelmets;
    public List<GameObject> tankCapes;
    public List<GameObject> tankBodies;
    public List<GameObject> tankWeapons;
    public List<GameObject> tankShields;
    [Header("Charger")]
    public List<GameObject> chargerHelmets;
    public List<GameObject> chargerCapes;
    public List<GameObject> chargerBodies;
    public List<GameObject> chargerWeapons;
    public List<GameObject> chargerShields;

}
public class NPC_Customization : MonoBehaviour
{

    [SerializeField] private Dictionary<NPCTheme, ThemeData> themeDataDict;

    [Header("Theme Data")]
    [SerializeField] private ThemeData romanTheme;
    [SerializeField] private ThemeData farmTheme;
    [SerializeField] private ThemeData miniTheme;
    public enum NPCTheme
    {
        Roman,
        Fantasy,
        SciFi,
        Farm,
        Mini
    };

    public enum NPCClass
    {
        Basic,
        Tank,
        Running,
        Charger
    }
    public NPCTheme Theme;
    public NPCClass Class;

    public int maxPoints = 10;
    private int currentPoints;
    public int[] stats;

    [SerializeField] TextMeshProUGUI attackStat;
    [SerializeField] TextMeshProUGUI defenseStat;
    [SerializeField] TextMeshProUGUI speedStat;

    [Header("Spawn Points")]
    [SerializeField] Transform weaponPoint;
    [SerializeField] Transform capePoint;
    [SerializeField] Transform helmetPoint;
    [SerializeField] Transform bodyPoint;
    [SerializeField] Transform shieldPoint;

    private GameObject currentHelmet;
    private GameObject currentWeapon;
    private GameObject currentCape;
    private GameObject currentShield;
    [SerializeField] GameObject currentBody;

    private void Awake()
    {
        // Initialize the dictionary
        themeDataDict = new Dictionary<NPCTheme, ThemeData>()
        {
            { NPCTheme.Roman, romanTheme },
            { NPCTheme.Farm, farmTheme },
            { NPCTheme.Mini, miniTheme }
        };
    }

    void Start()
    {
        stats = new int[3];
        attackStat.text = "Attack: " + stats[0];
        defenseStat.text = "Defense: " + stats[1];
        speedStat.text = "Speed: " + stats[2];
    }
    public void ChangeTheme(Dropdown change)
    {
        Theme = (NPCTheme)change.value;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Randomize();
        }
        transform.Rotate(0, 0.3f, 0);
    }
    public void Randomize()
    {
        if (themeDataDict.TryGetValue(Theme, out ThemeData themeData))
        {
            // Destroy previously instantiated assets
            DestroyAssets();

            // Get the appropriate asset lists based on the selected class
            List<GameObject> helmets = GetAssetsByClass(themeData.basicHelmets, themeData.tankHelmets, themeData.chargerHelmets);
            List<GameObject> weapons = GetAssetsByClass(themeData.basicWeapons, themeData.tankWeapons, themeData.chargerWeapons);
            List<GameObject> bodies = GetAssetsByClass(themeData.basicBodies, themeData.tankBodies, themeData.chargerBodies);
            List<GameObject> capes = GetAssetsByClass(themeData.basicCapes, themeData.tankCapes, themeData.chargerCapes);
            List<GameObject> shields = GetAssetsByClass(themeData.basicShields, themeData.tankShields, themeData.chargerShields);

            // Instantiate assets if they exist
            if (helmets != null && helmets.Count > 0)
            {
                currentHelmet = InstantiateRandomAsset(helmets, helmetPoint);
            }

            if (weapons != null && weapons.Count > 0)
            {
                currentWeapon = InstantiateRandomAsset(weapons, weaponPoint);
            }

            if (bodies != null && bodies.Count > 0)
            {
                currentBody = InstantiateRandomAsset(bodies, bodyPoint);
            }

            if (capes != null && capes.Count > 0)
            {
                currentCape = InstantiateRandomAsset(capes, capePoint);
            }

            if (shields != null && shields.Count > 0)
            {
                currentShield = InstantiateRandomAsset(shields, shieldPoint);
            }
        }
    }

    

    private List<GameObject> GetAssetsByClass(List<GameObject> basic, List<GameObject> tank, List<GameObject> charger)
    {
        switch (Class)
        {
            case NPCClass.Basic:
                return basic;
            case NPCClass.Tank:
                return tank;
            case NPCClass.Charger:
                return charger;
            default:
                return null;
        }
    }

    private void DistributePoints()
    {
        currentPoints = maxPoints;
        stats[0] = 0;
        stats[1] = 0;
        stats[2] = 0;

        while (currentPoints > 0)
        {
            stats[Random.Range(0, 3)]++;
            currentPoints--;
        }

        attackStat.text = "Attack: " + stats[0];
        defenseStat.text = "Defense: " + stats[1];
        speedStat.text = "Speed: " + stats[2];
    }

    private GameObject InstantiateRandomAsset(List<GameObject> assets, Transform spawnPoint)
    {
        int r = Random.Range(0, assets.Count);
        return Instantiate(assets[r], spawnPoint);
    }


    private void DestroyAssets()
    {
        if (currentHelmet != null) Destroy(currentHelmet);
        if (currentWeapon != null) Destroy(currentWeapon);
        if (currentCape != null) Destroy(currentCape);
        if (currentBody != null) Destroy(currentBody);
        if (currentShield != null) Destroy(currentShield);
    }
}
