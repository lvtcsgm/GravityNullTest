using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SCRAPS_INTERNAL_GravnullObject : MonoBehaviour {

    /* SCRAPS_INTERNAL_GravnullObject.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_GravnullObject.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    private bool isNear = false;
    private float maxDist = 5.0f;

    private MeshRenderer[] mRends;
    private Material[] oldMat;

    private bool glowing = false;
    private bool canGlow = true;

    private void Awake()
    {
        mRends = GetComponentsInChildren<MeshRenderer>();
        oldMat = new Material[mRends.Length];

        for (int i = 0; i < mRends.Length; i++)
        {
            oldMat[i] = mRends[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            float distance = Vector3.Distance(SCRAPS_ObjectManager.instance.gravGun.transform.position, transform.position);

            if (distance < maxDist)
                isNear = true;
            else
                isNear = false;
        }
        else
            isNear = false;

        if (isNear && !SCRAPS_ObjectManager.instance.gravGun.GetComponent<GravityGun_V2>().GravWell.IsCarryingObj())
        {
            if (!glowing && canGlow)
            {
                for (int i = 0; i < mRends.Length; i++)
                {
                    mRends[i].material = SCRAPS_ObjectManager.instance.grav_nearGlow_Mat;
                    mRends[i].material.SetTexture("_MainTex", oldMat[i].GetTexture("_MainTex"));
                    mRends[i].material.color = oldMat[i].color;
                }

                glowing = true;
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

    // Disables Glow Effect & the Ability to Glow
    public void DisableVFX()
    {
        for (int i = 0; i < mRends.Length; i++)
        {
            mRends[i].material = oldMat[i];
        }

        glowing = false;
        canGlow = false;
    }

    // Enable the Ability to Glow
    public void EnableVFX()
    {
        canGlow = true;
    }
}
