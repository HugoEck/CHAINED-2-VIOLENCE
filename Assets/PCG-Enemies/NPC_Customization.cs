using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    [Header("Rock thrower")]
    public List<GameObject> rockThrowerHelmets;
    public List<GameObject> rockThrowerCapes;
    public List<GameObject> rockThrowerBodies;
    public List<GameObject> rockThrowerWeapons;
    public List<GameObject> rockThrowerShields;
    //Add future classes under new Header

}

[System.Serializable]
public class ThemeDataPair
{
    public NPC_Customization.NPCTheme theme;
    public ThemeData themeData;
}
public class NPC_Customization : NetworkBehaviour
{

    [Header("NPC GameObjects")]
    [SerializeField] GameObject Rock;


    [SerializeField] private Dictionary<NPCTheme, ThemeData> themeDataDict;


    [Header("Theme Data")]
    [SerializeField] private List<ThemeDataPair> themeDataList;
    //[SerializeField] private ThemeData romanTheme;
    //[SerializeField] private ThemeData farmTheme;
    //[SerializeField] private ThemeData miniTheme;
    //[SerializeField] private ThemeData fantasyTheme;
    public enum NPCTheme
    {
        Roman,
        Fantasy,
        SciFi,
        Farm,
        Mini
        //Add more Themes here
    };

    public enum NPCClass
    {
        Basic,
        Tank,
        Runner,
        Charger,
        RockThrower,
        Warrior
        //Add more classes here
    }
    public NPCTheme Theme;
    public NPCClass Class;

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
    [SerializeField] public GameObject currentBody;


    //Animations
    Animator currentAnimator;
    [SerializeField] RuntimeAnimatorController animController;

    //private void Awake()
    //{
    //    themeDataDict = new Dictionary<NPCTheme, ThemeData>()
    //    {
    //        { NPCTheme.Roman, romanTheme },
    //        { NPCTheme.Farm, farmTheme },
    //        { NPCTheme.Mini, miniTheme },
    //        { NPCTheme.Fantasy, fantasyTheme }
    //        //Connect future themes here
    //    };
    //}

    private ThemeData GetThemeDataByTheme(NPCTheme theme)
    {
        foreach (var pair in themeDataList)
        {
            if (pair.theme == theme)
            {
                return pair.themeData;
            }
        }
        return null;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Randomize();
        }
        transform.Rotate(0, 0.3f, 0);
    }
    public void Randomize(NPCTheme npcTheme, NPCClass npcClass)
    {
        DestroyAssets();
        
        Theme = npcTheme;
        Class = npcClass;

        ThemeData themeData = GetThemeDataByTheme(npcTheme);


        if (themeData != null)
        {
            // Connect future assets and classes in their lists
            List<GameObject> helmets = GetAssetsByClass(themeData.basicHelmets, themeData.runnerHelmets, themeData.tankHelmets, themeData.chargerHelmets, themeData.rockThrowerHelmets, themeData.warriorHelmets);
            List<GameObject> weapons = GetAssetsByClass(themeData.basicWeapons, themeData.runnerWeapons, themeData.tankWeapons, themeData.chargerWeapons, themeData.rockThrowerWeapons, themeData.warriorWeapons);
            List<GameObject> bodies = GetAssetsByClass(themeData.basicBodies, themeData.runnerBodies, themeData.tankBodies, themeData.chargerBodies, themeData.rockThrowerBodies, themeData.warriorBodies);
            List<GameObject> capes = GetAssetsByClass(themeData.basicCapes, themeData.runnerCapes, themeData.tankCapes, themeData.chargerCapes, themeData.rockThrowerCapes, themeData.warriorCapes);
            List<GameObject> shields = GetAssetsByClass(themeData.basicShields, themeData.runnerShields, themeData.tankShields, themeData.chargerShields, themeData.rockThrowerShields, themeData.warriorShields);

            // Instantiate body and set up animator
            if (bodies != null && bodies.Count > 0)
            {
                
                currentBody = InstantiateRandomAsset(bodies, bodyPoint);

                currentAnimator = currentBody.GetComponent<Animator>();
                if (currentAnimator != null)
                {
                    currentAnimator.runtimeAnimatorController = animController;
                }

                // Attach helmet
                // Attach the helmet, weapon, cape, and shield as needed
                if (helmets != null && helmets.Count > 0)
                {
                    Transform headBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head");
                    if (headBone != null)
                    {
                        currentHelmet = InstantiateRandomAsset(helmets, headBone);
                    }
                }

                if (weapons != null && weapons.Count > 0)
                {
                    Transform rHandBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
                    if (rHandBone != null)
                    {
                        currentWeapon = InstantiateRandomAsset(weapons, rHandBone);
                    }
                }

                if (capes != null && capes.Count > 0)
                {
                    Transform neckBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck");
                    if (neckBone != null)
                    {
                        currentCape = InstantiateRandomAsset(capes, neckBone);
                    }
                }

                if (shields != null && shields.Count > 0)
                {
                    Transform lHandBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L");
                    if (lHandBone != null)
                    {
                        currentShield = InstantiateRandomAsset(shields, lHandBone);
                    }
                }
            }

        }
    }

    public void AddBehaviourToClass(GameObject enemy)
    {
        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        enemy.AddComponent<NetworkObject>();
        NetworkTransform networkTransform = enemy.AddComponent<NetworkTransform>();
        networkTransform.SyncRotAngleX = false;
        networkTransform.SyncRotAngleZ = false;
        networkTransform.SyncScaleX = false;
        networkTransform.SyncScaleY = false;
        networkTransform.SyncScaleZ = false;

        collider.isTrigger = true;
        enemy.tag = "Enemy";

        if (Class == NPCClass.Basic)
        {
            GoblinManager behaviour = enemy.AddComponent<GoblinManager>();
            behaviour.speed = 3;
            behaviour.attackRange = 3;
            behaviour.maxHealth = 1;
        }
        if (Class == NPCClass.Runner)
        {
            RunnerManager behaviour = enemy.AddComponent<RunnerManager>();
            behaviour.speed = 6;
            behaviour.attackRange = 3;
            behaviour.maxHealth = 1;
        }
        if (Class == NPCClass.RockThrower)
        {
            RockThrowerManager behaviour = enemy.AddComponent<RockThrowerManager>();

            GameObject throwPoint = new GameObject("throwPoint");
            throwPoint.transform.SetParent(enemy.transform, false);
            throwPoint.transform.localPosition = new Vector3(0.5f, 2, 1);

            behaviour.throwPoint = throwPoint.transform;
            behaviour.rockPrefab = Rock;

            behaviour.attackSpeed = 3;
            behaviour.maxHealth = 2;
            behaviour.attackRange = 20;
            behaviour.speed = 3;
            behaviour.damage = 2;
        }
    }

    private List<GameObject> GetAssetsByClass(List<GameObject> basic, List<GameObject> runner, List<GameObject> tank, List<GameObject> charger, List<GameObject> rockThrower, List<GameObject> warrior /*add future classes in paramter*/)
    {
        switch (Class)
        {
            case NPCClass.Basic:
                return basic;
            case NPCClass.Runner:
                return runner;
            case NPCClass.Tank:
                return tank;
            case NPCClass.Charger:
                return charger;
            case NPCClass.RockThrower:
                return rockThrower;
            case NPCClass.Warrior:
                return warrior;
            //Add future classes in switch case
            default:
                return null;
        }
    }

    private GameObject InstantiateRandomAsset(List<GameObject> assets, Transform parentBone)
    {
        if (assets == null || assets.Count == 0 || parentBone == null)
        {
            return null;
        }

        int randomIndex = Random.Range(0, assets.Count);
        GameObject newAsset = Instantiate(assets[randomIndex], parentBone.position, parentBone.rotation, parentBone);
        return newAsset;
    }


    private void DestroyAssets()
    {
        // Destroy the old attachments without affecting the root objects
        DestroyChildren(helmetPoint);
        DestroyChildren(weaponPoint);
        DestroyChildren(capePoint);
        DestroyChildren(shieldPoint);
        DestroyChildren(bodyPoint);

        // Reset asset references (this is optional depending on how you track them)
        currentHelmet = null;
        currentWeapon = null;
        currentCape = null;
        currentShield = null;
        currentBody = null;
    }

    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
