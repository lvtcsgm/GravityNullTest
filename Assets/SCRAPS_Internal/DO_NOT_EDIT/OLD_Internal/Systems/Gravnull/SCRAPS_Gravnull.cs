using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SCRAPS_Gravnull : MonoBehaviour {

    /*---------------------------------------------------------------------
    SCRAPS_Gravnull.cs by Derek Allen Marunowski

    The gravnull will be able to draw a line straight ahead to the center of the camera from the gun.
    Anything it collides with with light up with a special shader to show it can be lifted.
    Left click with pull the object to a specific point which can be carried.  It still has collision which much resolve.
    Right click will launch the object straight ahead from you.
    ---------------------------------------------------------------------*/

    public LayerMask gravLayer;
    public GameObject target;
    public Material glowMat;
    public Material nearGlowMat;
    //private float distance = 10.0f;

    private MeshRenderer[] rends;
    private Material[] oldMat;

    private float force = 25.0f;

    public bool used = false;

    public Light gravLight;
    public ParticleSystem gravPart;

    public SCRAPS_GravnullRadius gRad;

    private Transform player;
    private Transform gravNode;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gravNode = GameObject.Find("CamNode_Gravnull").transform;
    }

    void FixedUpdate()
    {
        Transform[] objs = GameObject.FindObjectsOfType<Transform>();

        foreach (Transform obj in objs)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("Gravnull"))
            {
                if (obj.gameObject.GetComponent<SCRAPS_Gravnull_Effector>() == null)
                {
                    obj.gameObject.AddComponent<SCRAPS_Gravnull_Effector>();
                    SCRAPS_Gravnull_Effector sge = obj.gameObject.GetComponent<SCRAPS_Gravnull_Effector>();
                    if (sge.GetComponent<MeshRenderer>() != null)
                    {
                        sge.oldMat = sge.GetComponent<MeshRenderer>().material;
                        sge.glowMat = nearGlowMat;
                        sge.gravLoc = transform;
                        sge.isAdded = true;
                    }
                }
            }
        }

        if(target != null)
        {
            Rigidbody rb = target.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;

            rb.transform.position = Vector3.Slerp(rb.transform.position, gRad.transform.position, Vector3.Distance(rb.transform.position, gRad.transform.position) * 100.0f * Time.deltaTime);
        }

        if (Input.GetButton("Fire2"))
        {
            // launch objects
            if (target != null)
            {
                Rigidbody rb = target.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = transform.forward * force;
                RemoveEffects(rb.transform);
                target = null;
                gRad.target = null;
            }

            gravLight.intensity = 0.0f;
        }
        else if (Input.GetButton("Fire1") && target == null)
        {
            // highlight our objects
            Ray ray = new Ray(new Vector3(player.position.x, player.position.y + 1.5f, player.position.z), gravNode.transform.forward);
            RaycastHit hit;

            gRad.transform.position = transform.position;

            if (Physics.Raycast(ray, out hit, 16.0f))
            {
                if(hit.transform.gameObject.isStatic == true)
                    gRad.transform.position = hit.point;

                if(gRad.target != null)
                {
                    target = gRad.target.gameObject;
                    target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    AddEffects(target.transform);
                }
            }

            gravLight.intensity = 0.2f;
            gravPart.Emit(1);
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            // release object
            if (target != null)
            {
                Rigidbody rb = target.GetComponent<Rigidbody>();
                rb.useGravity = true;
                RemoveEffects(rb.transform);
                target = null;
                gRad.target = null;
            }

            used = false;

            gravLight.intensity = 0.0f;
        }
        else
        {
            gRad.target = null;
        }
    }

    void AddEffects(Transform to)
    {
        rends = to.GetComponentsInChildren<MeshRenderer>();
        oldMat = new Material[rends.Length];

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i].GetComponent<SCRAPS_Gravnull_Effector>() != null)
                rends[i].GetComponent<SCRAPS_Gravnull_Effector>().DisableWhenGrav();
            oldMat[i] = rends[i].material;
            rends[i].material = glowMat;
            rends[i].material.mainTexture = oldMat[i].mainTexture;
            rends[i].material.SetColor(Shader.PropertyToID("_ColorTint"), oldMat[i].color);
        }
    }

    void RemoveEffects(Transform to)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != null)
            {
                rends[i].material = oldMat[i];
                if (rends[i].GetComponent<SCRAPS_Gravnull_Effector>() != null)
                    rends[i].GetComponent<SCRAPS_Gravnull_Effector>().disabled = false;
            }
        }
    }
}
