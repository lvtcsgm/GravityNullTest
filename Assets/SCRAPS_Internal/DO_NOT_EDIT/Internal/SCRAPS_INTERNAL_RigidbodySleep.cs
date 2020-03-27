using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SCRAPS_INTERNAL_RigidbodySleep : MonoBehaviour {

    /* SCRAPS_INTERNAL_RigidbodySleep.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_RigidbodySleep.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    void Awake()
    {
        GetComponent<Rigidbody>().Sleep();
    }
}