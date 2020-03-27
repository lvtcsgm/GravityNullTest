using UnityEngine;

public class SCRAPS_INTERNAL_ParenterVolume : MonoBehaviour {

    /* SCRAPS_INTERNAL_ParenterVolume.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_ParenterVolume.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    void OnTriggerStay(Collider other)
    {
        //did the player step into the volume
        if (other.tag == "Player")
        {
            //if so, make them a child of the platform
            other.transform.parent = transform;
        }
        //did a Phys object land in our volume
        else if (other.tag == "Phys")
        {
            //make them a child of the platform
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //did the player step out of the volume
        if (other.tag == "Player")
        {
            //if so, the player has no parent
            other.transform.parent = null;
        }
        //did a Phys object exit our volume
        else if (other.tag == "Phys")
        {
            //the object has no parent
            other.transform.parent = null;
        }
    }
}
