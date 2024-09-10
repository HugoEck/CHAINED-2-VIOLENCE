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


    [SerializeField] List<GameObject> helmets = new List<GameObject>();
    [SerializeField] List<GameObject> weapon = new List<GameObject>();
    [SerializeField] List<GameObject> capes = new List<GameObject>();
    [SerializeField] List<GameObject> bodies = new List<GameObject>();

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

            int r = Random.RandomRange(0, helmets.Count);
            currentHelmet = Instantiate(helmets[r], helmetPoint);
            r = Random.Range(0, weapon.Count);
            currentWeapon = Instantiate(weapon[r], weaponPoint);
            r = Random.Range(0, capes.Count);
            if (capes[r] != null)
                currentCape = Instantiate(capes[r], capePoint);
            r = Random.Range(0, bodies.Count);
            currentBody = Instantiate(bodies[r], bodyPoint);
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
