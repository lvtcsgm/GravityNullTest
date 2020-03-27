using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_LightingController : MonoBehaviour {

    public bool blockedAbove = false;
    public bool blockedTopRight = false;
    public bool blockedTopLeft = false;
    public bool blockedTopForward = false;
    public bool blockedTopReverse = false;

    public float distance = 10;

    public int maxBlockers = 5;

	// Update is called once per frame
	void Update () {

        Vector3 modPos = transform.position;
        modPos.y += 1;

        RaycastHit hit;

        //check up
        if (Physics.SphereCast(modPos, 1, transform.TransformDirection(Vector3.up), out hit, distance))
        {
            blockedAbove = true;

            Debug.DrawRay(modPos, transform.TransformDirection(Vector3.up) * hit.distance, Color.red);
        }
        else
        {
            blockedAbove = false;
            Debug.DrawRay(modPos, transform.TransformDirection(Vector3.up) * distance, Color.white);
        }

        //check top right
        if (Physics.SphereCast(modPos, 1, transform.TransformDirection(new Vector3(1, 0.75f, 0)), out hit, distance /2))
        {
            blockedTopRight = true;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(1, 0.75f, 0)) * hit.distance, Color.red);
        }
        else
        {
            blockedTopRight = false;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(1, 0.75f, 0)) * distance /2, Color.white);
        }

        //check top left
        if (Physics.SphereCast(modPos, 1, transform.TransformDirection(new Vector3(-1, 0.75f, 0)), out hit, distance /2))
        {
            blockedTopLeft = true;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(-1, 0.75f, 0)) * hit.distance, Color.red);
        }
        else
        {
            blockedTopLeft = false;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(-1, 0.75f, 0)) * distance /2, Color.white);
        }

        //check top forward
        if (Physics.SphereCast(modPos, 1, transform.TransformDirection(new Vector3(0, 0.75f, 1)), out hit, distance /2))
        {
            blockedTopForward = true;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(0, 0.75f, 1)) * hit.distance, Color.red);
        }
        else
        {
            blockedTopForward = false;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(0, 0.75f, 1)) * distance /2, Color.white);
        }

        //check top reverse
        if (Physics.SphereCast(modPos, 1, transform.TransformDirection(new Vector3(0, 0.75f, -1)), out hit, distance /2))
        {
            blockedTopReverse = true;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(0, 0.75f, -1)) * hit.distance, Color.red);
        }
        else
        {
            blockedTopReverse = false;

            Debug.DrawRay(modPos, transform.TransformDirection(new Vector3(0, 0.75f, -1)) * distance /2, Color.white);
        }

        if (blockedAbove)
        {
            //under something

            int blockedCount = 0;

            if (blockedTopLeft)
                blockedCount++;
            if (blockedTopRight)
                blockedCount++;
            if (blockedTopForward)
                blockedCount++;
            if (blockedTopReverse)
                blockedCount++;

            //print(blockedCount);

            if (blockedCount >= maxBlockers)
            {
                SCRAPS_LightingManager.currentLighting = SCRAPS_LightingManager.lType.indoors;
            }
            else
            {
                SCRAPS_LightingManager.currentLighting = SCRAPS_LightingManager.lType.outdoors;
            }
                
        }
        else
        {
            SCRAPS_LightingManager.currentLighting = SCRAPS_LightingManager.lType.outdoors;
        }
    }
}
