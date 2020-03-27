using UnityEngine;

public class SCRAPS_INTERNAL_CameraFacingBillboard : MonoBehaviour {

    /* SCRAPS_INTERNAL_CameraFacingBillboard.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_CameraFacingBillboard.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    public Camera m_Camera;
    public bool amActive = false;
    public bool autoInit = false;
    GameObject myContainer;

    void Awake()
    {
        if (autoInit == true)
        {
            m_Camera = Camera.main;
            amActive = true;
        }

        myContainer = new GameObject();
        myContainer.name = "GRP_" + transform.gameObject.name;
        myContainer.transform.position = transform.position;
        transform.parent = myContainer.transform;
    }


    void Update()
    {
        if (amActive == true)
        {
            myContainer.transform.LookAt(myContainer.transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
        }
    }
}
