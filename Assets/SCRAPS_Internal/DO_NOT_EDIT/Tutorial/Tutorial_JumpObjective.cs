using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_JumpObjective : MonoBehaviour {

    public SCRAPS_Objective objective;
    private Rigidbody playerRb;

    public bool done = false;

    public GameObject vfx;

    private void Update()
    {
        if(done == false)
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
        if (done == false)
        {
            if (other.tag == "Player")
            {
                if (playerRb == null)
                    playerRb = other.GetComponent<Rigidbody>();
                else
                {
                    if (playerRb.velocity.y > 0)
                    {
                        //the player is here and jumping
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
}
