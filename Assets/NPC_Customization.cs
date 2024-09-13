using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Customization : MonoBehaviour
{
    public enum NPCTheme
    {
        Roman,
        Fantasy,
        SciFi,
        Farm
    };
    public NPCTheme Theme;

    public int maxPoints = 10;
    private int currentPoints;
    public int[] stats;

    [SerializeField] TextMeshProUGUI attackStat;
    [SerializeField] TextMeshProUGUI defenseStat;
    [SerializeField] TextMeshProUGUI speedStat;


    [SerializeField] List<GameObject> romanHelmets = new List<GameObject>();
    [SerializeField] List<GameObject> romanWeapons = new List<GameObject>();
    [SerializeField] List<GameObject> romanCapes = new List<GameObject>();
    [SerializeField] List<GameObject> romanBodies = new List<GameObject>();

    [SerializeField] List<GameObject> farmHelmets = new List<GameObject>();
    [SerializeField] List<GameObject> farmWeapons = new List<GameObject>();
    [SerializeField] List<GameObject> farmBodies = new List<GameObject>();

    [SerializeField] Transform weaponPoint;
    [SerializeField] Transform capePoint;
    [SerializeField] Transform helmetPoint;
    [SerializeField] Transform bodyPoint;

    private GameObject currentHelmet;
    private GameObject currentWeapon;
    private GameObject currentCape;
    [SerializeField] GameObject currentBody;


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
    public void Randomize()
    {


        if (Theme == NPCTheme.Roman)
        {
            if (currentHelmet != null) Destroy(currentHelmet);
            if (currentWeapon != null) Destroy(currentWeapon);
            if (currentCape != null) Destroy(currentCape);
            if (currentBody != null) Destroy(currentBody);

            int r = Random.RandomRange(0, romanHelmets.Count);
            currentHelmet = Instantiate(romanHelmets[r], helmetPoint);
            r = Random.Range(0, romanWeapons.Count);
            currentWeapon = Instantiate(romanWeapons[r], weaponPoint);
            r = Random.Range(0, romanCapes.Count);
            if (romanCapes[r] != null)
                currentCape = Instantiate(romanCapes[r], capePoint);
            r = Random.Range(0, romanBodies.Count);
            currentBody = Instantiate(romanBodies[r], bodyPoint);
        }
        if (Theme == NPCTheme.Farm)
        {
            if (currentHelmet != null) Destroy(currentHelmet);
            if (currentWeapon != null) Destroy(currentWeapon);
            if (currentCape != null) Destroy(currentCape);
            if (currentBody != null) Destroy(currentBody);

            int r = Random.RandomRange(0, farmHelmets.Count);
            currentHelmet = Instantiate(farmHelmets[r], helmetPoint);
            r = Random.Range(0, farmWeapons.Count);
            currentWeapon = Instantiate(farmWeapons[r], weaponPoint);
            r = Random.Range(0, farmBodies.Count);
            currentBody = Instantiate(farmBodies[r], bodyPoint);
        }

            //POINT DISTRIBUTION

            //start with reset from earlier iterations:
            currentPoints = maxPoints;
        stats[0] = 0;
        stats[1] = 0;
        stats[2] = 0;

        //Actual point distribution
        while(currentPoints>0)
        {
        stats[Random.Range(0,3)]++;
        currentPoints--;
        }

        attackStat.text = "Attack: " + stats[0];
        defenseStat.text = "Defense: " + stats[1];
        speedStat.text = "Speed: " + stats[2];

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.3f, 0);
    }
}
