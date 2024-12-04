using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
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
    [Header("BannerMan")]
    public List<GameObject> BannermanHelmets;
    public List<GameObject> BannermanCapes;
    public List<GameObject> BannermanBodies;
    public List<GameObject> BannermanWeapons;
    public List<GameObject> BannermanShields;
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

    [Header("NPC GameObjects")]
    [SerializeField] GameObject RomanRock;
    [SerializeField] GameObject PirateRock;
    [SerializeField] GameObject FarmRock;

    [SerializeField] GameObject PirateBanner;


    [SerializeField] GameObject RagdollRoot;

    [Header("NPC Particles")]
    [SerializeField] GameObject bloodSplatter;
    [SerializeField] GameObject smokeTrail;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject electricityEffect;
    [SerializeField] GameObject fireEffect;

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
        Mini,
        Pirate,
        Cowboys,
        Indians,
        CurrentDay
        //Add more Themes here
    };

    public enum NPCClass
    {
        Basic,
        Tank,
        Runner,
        Charger,
        RockThrower,
        Warrior,
        Bannerman
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
            List<GameObject> helmets = GetAssetsByClass(themeData.basicHelmets, themeData.runnerHelmets, themeData.tankHelmets, themeData.chargerHelmets, themeData.rockThrowerHelmets, themeData.warriorHelmets, themeData.BannermanHelmets);
            List<GameObject> weapons = GetAssetsByClass(themeData.basicWeapons, themeData.runnerWeapons, themeData.tankWeapons, themeData.chargerWeapons, themeData.rockThrowerWeapons, themeData.warriorWeapons, themeData.BannermanWeapons);
            List<GameObject> bodies = GetAssetsByClass(themeData.basicBodies, themeData.runnerBodies, themeData.tankBodies, themeData.chargerBodies, themeData.rockThrowerBodies, themeData.warriorBodies, themeData.BannermanBodies);
            List<GameObject> capes = GetAssetsByClass(themeData.basicCapes, themeData.runnerCapes, themeData.tankCapes, themeData.chargerCapes, themeData.rockThrowerCapes, themeData.warriorCapes, themeData.BannermanCapes);
            List<GameObject> shields = GetAssetsByClass(themeData.basicShields, themeData.runnerShields, themeData.tankShields, themeData.chargerShields, themeData.rockThrowerShields, themeData.warriorShields, themeData.BannermanShields);

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
                        if(Theme == NPCTheme.Pirate)
                        {
                            currentWeapon.transform.localScale *= 100;
                        }
                        else if(Theme == NPCTheme.Roman)
                        {
                            currentWeapon.transform.rotation = Quaternion.Euler(90, 0, -3);
                        }
                        else if (Theme == NPCTheme.Farm)
                        {
                            currentWeapon.transform.rotation = Quaternion.Euler(90, 0, 0);
                            currentWeapon.transform.localPosition = new Vector3(0, 0, -0.5f);
                        }
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
                        currentShield.transform.rotation = Quaternion.Euler(0, -150, -95);
                    }
                    if (Theme == NPCTheme.Farm)
                    {
                        currentShield.transform.rotation = Quaternion.Euler(180, 90, 0);
                    }
                }
            }

        }
    }

    public void AddBehaviourToClass(GameObject enemy)
    {
        AIPath agent = enemy.AddComponent<AIPath>();
        AIDestinationSetter destinationSetter = enemy.AddComponent<AIDestinationSetter>();
        CapsuleCollider capsule = enemy.AddComponent<CapsuleCollider>();
        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        ObiCollider obiCollider = enemy.AddComponent<ObiCollider>();
        SimpleSmoothModifier smoothing = enemy.AddComponent<SimpleSmoothModifier>();
        BoxCollider triggerCollider = enemy.AddComponent<BoxCollider>();
        IgnoreCollisionWithAbilityChain ignoreChain = enemy.AddComponent<IgnoreCollisionWithAbilityChain>();
        ignoreChain.ObjectIgnoresLaserChain();
        

        enemy.transform.localScale *= 1.5f;
       
        GameObject bloodCopy = Instantiate(bloodSplatter, enemy.transform);
        GameObject hitEffectCopy = Instantiate(hitEffect, enemy.transform);
        GameObject electricityEffectParticles = Instantiate(electricityEffect, enemy.transform);
        GameObject fireEffectParticles = Instantiate(fireEffect, enemy.transform);
        //Physics.SyncTransforms();

        triggerCollider.isTrigger = true;
        enemy.tag = "Enemy";
        enemy.layer = 9;


        Rigidbody[] rigidbodies = enemy.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rbs in rigidbodies)
        {
            rbs.isKinematic = true; 
            rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        Collider[] capsuleColliders = enemy.GetComponentsInChildren<Collider>();
        foreach (Collider capsule1 in capsuleColliders)
        {
            capsule1.enabled = false; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
        }
        capsule.enabled = true;
        rb.isKinematic = false;


        if (Class == NPCClass.Basic)
        {
            PlebianManager behaviour = enemy.AddComponent<PlebianManager>();

        }
        else if (Class == NPCClass.Runner)
        {
            RunnerManager behaviour = enemy.AddComponent<RunnerManager>();

        }
        else if (Class == NPCClass.Warrior)
        {
            SwordsmanManager behaviour = enemy.AddComponent<SwordsmanManager>();
        }
        else if (Class == NPCClass.RockThrower)
        {
            RockThrowerManager behaviour = enemy.AddComponent<RockThrowerManager>();

            GameObject throwPoint = new GameObject("throwPoint");
            throwPoint.transform.SetParent(enemy.transform, false);
            throwPoint.transform.localPosition = new Vector3(0.5f, 2, 1);

            if(Theme == NPCTheme.Roman)
            {
            behaviour.throwPoint = throwPoint.transform;
            behaviour.rockPrefab = RomanRock;
            }
            else if (Theme == NPCTheme.Pirate)
            {
                behaviour.throwPoint = throwPoint.transform;
                behaviour.rockPrefab = PirateRock;
            }
            else if (Theme == NPCTheme.Farm)
            {
                behaviour.throwPoint = throwPoint.transform;
                behaviour.rockPrefab = FarmRock;
            }
        }
        else if(Class == NPCClass.Bannerman)
        {
            BannerManManager behaviour = enemy.AddComponent<BannerManManager>();

            behaviour.flagPrefab = PirateBanner;
        }

        else if (Class == NPCClass.Charger)
        {
            GameObject smokeTrailCopy = Instantiate(smokeTrail, enemy.transform);
            ChargerManager behaviour = enemy.AddComponent<ChargerManager>();

        }
    }

    private List<GameObject> GetAssetsByClass(List<GameObject> basic, List<GameObject> runner, List<GameObject> tank, List<GameObject> charger, List<GameObject> rockThrower, List<GameObject> warrior, List<GameObject> bannerman /*add future classes in paramter*/)
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
                case NPCClass.Bannerman:
                    return bannerman;
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
