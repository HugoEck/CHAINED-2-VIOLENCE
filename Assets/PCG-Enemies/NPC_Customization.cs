using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
public class NPC_Customization : MonoBehaviour
{

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
        Running,
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
    [SerializeField] AnimatorController animController;

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
        ThemeData themeData = GetThemeDataByTheme(npcTheme);
        DestroyAssets();

        if (themeData != null)
        {
            // Destroy previously instantiated assets

            //Connect future assets  and classes in their lists
            List<GameObject> helmets = GetAssetsByClass(themeData.basicHelmets, themeData.tankHelmets, themeData.chargerHelmets, themeData.rockThrowerHelmets, themeData.warriorHelmets);
            List<GameObject> weapons = GetAssetsByClass(themeData.basicWeapons, themeData.tankWeapons, themeData.chargerWeapons, themeData.rockThrowerWeapons, themeData.warriorWeapons);
            List<GameObject> bodies = GetAssetsByClass(themeData.basicBodies, themeData.tankBodies, themeData.chargerBodies, themeData.rockThrowerBodies, themeData.warriorBodies);
            List<GameObject> capes = GetAssetsByClass(themeData.basicCapes, themeData.tankCapes, themeData.chargerCapes, themeData.rockThrowerCapes, themeData.warriorCapes);
            List<GameObject> shields = GetAssetsByClass(themeData.basicShields, themeData.tankShields, themeData.chargerShields, themeData.rockThrowerShields, themeData.warriorShields);

            if (bodies != null && bodies.Count > 0)
            {
                currentBody = InstantiateRandomAsset(bodies, bodyPoint);

                currentAnimator = currentBody.GetComponent<Animator>();
                if (currentAnimator != null)
                {
                    currentAnimator.runtimeAnimatorController = animController;
                }
                

                if (helmets != null && helmets.Count > 0)
                {
                    Transform headBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head");
                    if (headBone != null)
                    {
                        // Preserve local position and rotation
                        Vector3 originalPosition = helmetPoint.localPosition;
                        Quaternion originalRotation = helmetPoint.localRotation;

                        // Reparent the helmetPoint
                        helmetPoint.SetParent(headBone);

                        // Restore original local position and rotation
                        helmetPoint.localPosition = Vector3.zero;
                        helmetPoint.localRotation = Quaternion.identity;

                        currentHelmet = InstantiateRandomAsset(helmets, helmetPoint);
                    }
                }

                // Attach weapon
                if (weapons != null && weapons.Count > 0)
                {
                    Transform rHandBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/Thumb_01 1/Thumb_02 1");
                    if (rHandBone != null)
                    {
                        // Preserve local position and rotation
                        Vector3 originalPosition = weaponPoint.localPosition;
                        Quaternion originalRotation = weaponPoint.localRotation;

                        // Reparent the weaponPoint
                        weaponPoint.SetParent(rHandBone);

                        // Restore original local position and rotation
                        weaponPoint.localPosition = Vector3.zero; 
                        weaponPoint.localRotation = Quaternion.identity; 
                        weaponPoint.localRotation = originalRotation;
                        weaponPoint.localPosition = originalPosition;

                        currentWeapon = InstantiateRandomAsset(weapons, weaponPoint);
                    }
                }

                // Attach cape
                if (capes != null && capes.Count > 0)
                {
                    Transform neckBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck");
                    if (neckBone != null)
                    {
                        // Preserve local position and rotation
                        Vector3 originalPosition = capePoint.localPosition;
                        Quaternion originalRotation = capePoint.localRotation;

                        // Reparent the capePoint
                        capePoint.SetParent(neckBone);

                        // Restore original local position and rotation
                        capePoint.localPosition = Vector3.zero;
                        capePoint.localRotation = Quaternion.identity;

                        currentCape = InstantiateRandomAsset(capes, capePoint);
                    }
                }

                // Attach shield
                if (shields != null && shields.Count > 0)
                {
                    Transform lHandBone = currentBody.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L/Thumb_01/Thumb_02/Thumb_03");
                    if (lHandBone != null)
                    {
                        // Preserve local position and rotation
                        Vector3 originalPosition = shieldPoint.localPosition;
                        Quaternion originalRotation = shieldPoint.localRotation;

                        // Reparent the shieldPoint
                        shieldPoint.SetParent(lHandBone);

                        // Restore original local position and rotation
                        shieldPoint.localPosition = Vector3.zero;
                        shieldPoint.localRotation = Quaternion.identity;
                        shieldPoint.localRotation = originalRotation;

                        currentShield = InstantiateRandomAsset(shields, shieldPoint);
                    }
                }
            }
        }
    }

    public void AddBehaviourToClass(GameObject enemy)
    {
        enemy.AddComponent<NavMeshAgent>();
        if (Class == NPCClass.Basic)
        {
            GoblinManager behaviour = enemy.AddComponent<GoblinManager>();
            behaviour.speed = 1;
            behaviour.maxHealth = 100;

        }
    }

    private List<GameObject> GetAssetsByClass(List<GameObject> basic, List<GameObject> tank, List<GameObject> charger, List<GameObject> rockThrower, List<GameObject> warrior /*add future classes in paramter*/)
    {
        switch (Class)
        {
            case NPCClass.Basic:
                return basic;
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
