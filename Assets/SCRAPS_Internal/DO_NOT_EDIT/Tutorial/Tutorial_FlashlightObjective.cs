using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_FlashlightObjective : MonoBehaviour {

    public SCRAPS_Objective objective;
    private Light flashlight;

    public bool done = false;

    public GameObject vfx;

    private void Update()
    {
        if (done == false)
        {
            if (vfx.activeSelf != objective.isHere)
                vfx.SetActive(objective.isHere);
        }
        else
        {
            if (vfx.activeSelf != false)
                vfx.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (flashlight == null)
                flashlight = GameObject.FindObjectOfType<FlashLight>().GetComponent<Light>();
            else
            {
                if (flashlight.enabled == true)
                {
                    //the player is here and using flashlight
                    if (objective.isComplete == false)
                    {
                        objective.UpdateObjective(1);
                        done = true;
                    }
                }
            }
        }
    }
}
