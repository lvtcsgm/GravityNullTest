using UnityEngine;
using UnityEngine.Events;

public class SCRAPS_INTERNAL_Interactive : MonoBehaviour {

    /* SCRAPS_INTERNAL_Interactive.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_Interactive.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    private bool isNear = false;

    private MeshRenderer[] mRends;

    private Material[] oldMat;

    private bool glowing = false;
    private bool active = true;

    public UnityEvent onInteract;

    private void Awake()
    {
        mRends = GetComponentsInChildren<MeshRenderer>();
        oldMat = new Material[mRends.Length];

        for (int i = 0; i < mRends.Length; i++)
        {
            oldMat[i] = mRends[i].materials[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        if (isNear && SCRAPS_ObjectManager.instance.player != null &&
            Vector3.Dot(SCRAPS_ObjectManager.instance.player.transform.TransformDirection(Vector3.forward), transform.position - SCRAPS_ObjectManager.instance.player.transform.position) > 0.5f)
        {
            if (!glowing)
            {
                for (int i = 0; i < mRends.Length; i++)
                {
                    mRends[i].materials[0] = SCRAPS_ObjectManager.instance.interactive_glow_Mat;
                    mRends[i].materials[0].SetTexture("_MainTex", oldMat[i].GetTexture("_MainTex"));
                    mRends[i].materials[0].color = oldMat[i].color;
                }

                glowing = true;
            }

            SCRAPS_ObjectManager.instance.ShowInteract();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (onInteract != null)
                    onInteract.Invoke();
            }
        }
        else
        {
            if (glowing)
            {
                for (int i = 0; i < mRends.Length; i++)
                {
                    mRends[i].material = oldMat[i];
                }

                glowing = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isNear = false;
    }

    // Disables Interaction & Highlight
    public void DisableInteraction()
    {
        for (int i = 0; i < mRends.Length; i++)
        {
            mRends[i].materials[0] = oldMat[i];
        }

        glowing = false;
        active = false;
    }

    // Enables Interaction & Highlight
    public void EnableInteraction()
    {
        active = true;
    }
}
