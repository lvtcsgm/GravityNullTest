using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SCRAPS_INTERNAL_RenderCable : MonoBehaviour {

    /* SCRAPS_INTERNAL_RenderCable.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_RenderCable.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    private LineRenderer lRend;

    [Header("What are we connected to?")]
    public Transform connect;

    // Use this for initialization
    void Start()
    {
        lRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lRend.SetPosition(0, connect.position);
        lRend.SetPosition(1, transform.position);
    }
}