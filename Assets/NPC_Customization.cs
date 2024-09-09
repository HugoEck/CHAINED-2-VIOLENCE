using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Customization : MonoBehaviour
{
    [SerializeField] List<GameObject> helmets = new List<GameObject>();
    [SerializeField] List<GameObject> weapon = new List<GameObject> ();
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
        
    }

    public void Randomize()
    {
        if (currentHelmet != null) Destroy(currentHelmet);
        if (currentWeapon != null) Destroy(currentWeapon);
        if (currentCape != null) Destroy(currentCape);
        if (currentBody != null) Destroy(currentBody);

        int r = Random.RandomRange(0, helmets.Count);
        currentHelmet = Instantiate(helmets[r], helmetPoint);
        r = Random.Range(0, weapon.Count);
        currentWeapon = Instantiate(weapon[r], weaponPoint);
        r = Random.Range (0, capes.Count);
        if (capes[r] != null)
            currentCape = Instantiate(capes[r], capePoint);
        r = Random.Range(0, bodies.Count);
        currentBody = Instantiate(bodies[r], bodyPoint);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.3f, 0);
    }
}
