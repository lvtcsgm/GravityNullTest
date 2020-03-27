using UnityEngine;

public class SCRAPS_Gravnull_Effector : MonoBehaviour {

    public Material oldMat;
    public Material glowMat;
    public Transform gravLoc;
    private SCRAPS_Gravnull gravnull;

    private float distance = 5.0f;
    private MeshRenderer mRend;

    public bool isAdded = false;
    public bool disabled = false;

    private float glowAmount = 10;

    void Update()
    {
        if(isAdded)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                mRend = GetComponent<MeshRenderer>();
                glowMat.mainTexture = oldMat.mainTexture;
                glowMat.SetColor(Shader.PropertyToID("_ColorTint"), oldMat.color);
            }

            if(mRend != null)
            {
                if (disabled == false)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        if (gravLoc != null)
                        {
                            if (Vector3.Distance(gravLoc.position, transform.position) <= distance && gravLoc.GetComponent<SCRAPS_Gravnull>().target == null)
                            {
                                GlowMat();
                            }
                            else
                            {
                                NormalMat();
                            }
                        }
                    }
                    else
                    {
                        NormalMat();
                    }
                }
                else
                {
                    //NormalMat();
                }
            }
        }
    }

    public void DisableWhenGrav()
    {
        disabled = true;
        NormalMat();
    }

    private void GlowMat()
    {
        if (glowAmount > 2)
            glowAmount -= Time.deltaTime;
        else
            glowAmount = 2;

        glowMat.SetFloat(Shader.PropertyToID("_RimPower"), glowAmount);
        mRend.material = glowMat;
    }

    private void NormalMat()
    {
        glowAmount = 10;
        glowMat.SetFloat(Shader.PropertyToID("_RimPower"), glowAmount);
        mRend.material = oldMat;
    }
}
