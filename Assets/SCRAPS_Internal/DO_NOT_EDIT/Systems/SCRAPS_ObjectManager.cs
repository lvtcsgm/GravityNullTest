using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_ObjectManager : MonoBehaviour {

    private static SCRAPS_ObjectManager _instance;
    public static SCRAPS_ObjectManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SCRAPS_ObjectManager>();
            return _instance;
        }
    }

    public GameObject player;

    public GameObject gravGun;
    public GravityWell gravWell;

    public Material grav_nearGlow_Mat;
    public Material interactive_glow_Mat;

    public GameObject interactMessage;
    private bool showInteract = false;

    private float displayInteractTime = 0.5f;
    private float displayInteractTimer = 0;

    public GameObject compassMarker;
    public Material checkpointIcon;
    public Material objectiveIcon;

    // Update is called once per frame
    void Update () {

        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (gravGun == null) { 
            gravGun = GameObject.FindObjectOfType<GravityGun_V2>().gameObject;
            gravWell = gravGun.GetComponent<GravityGun_V2>().GravWell;
        }

        if (interactMessage.activeInHierarchy != showInteract)
            interactMessage.SetActive(showInteract);

        if(displayInteractTimer > 0)
        {
            displayInteractTimer -= Time.deltaTime;
        }
        else
        {
            showInteract = false;
        }
    }

    public void ShowInteract()
    {
        displayInteractTimer = displayInteractTime;
        showInteract = true;
    }
}
