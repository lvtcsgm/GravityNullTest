using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_GravnullV2 : MonoBehaviour {

    private GameObject target;
    private Rigidbody rb;

    [Header("Gravnull Settings")]
    [SerializeField] private float pullStrength = 1.0f;                    // How fast an object is pulled in
    private float travelSpeed = 0;
    [SerializeField] private float spinResistance = 1.0f;          // How fast the object will stop spinning
    [SerializeField] private float launchForce = 1.0f;             // How much force is added when launching the an object
    [SerializeField] private float gravRange = 1.0f;               // How far away the player can pickup an object
    [SerializeField] private float accuracy = 1.0f;                // Radius of the overlap sphere
    private float cooldown = 0;
    private Vector3 initialPosition;                                // In Local Space

    [Header("Rendering")]
    public Material glowMat;
    public Material nearGlowMat;
    private MeshRenderer[] rends;
    private Material[] oldMat;
    
    // Components
    private Light gravLight;
    private ParticleSystem particle;
    private Transform player;
    private Transform camNode;


    void Start() {
        initialPosition = transform.localPosition;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camNode = GameObject.Find("CamNode_Gravnull").transform;
        particle = GetComponentInChildren<ParticleSystem>();
        gravLight = GetComponent<Light>();
    }

    void FixedUpdate() {

        // Make Nearby Objects Glow

        if (target != null) { if (rb == null) { PickupObject(); } else { PullObject(); } }

        if (Input.GetButtonDown("Fire2") && target != null) { LaunchObject(); }
        else if (Input.GetButtonUp("Fire1") && target != null) { DropObject(); }
        else if (target == null) {
            if (cooldown > 0) { cooldown -= Time.deltaTime; }
            else if (Input.GetButton("Fire1")) { FindObject(); }
            else { if (gravLight.intensity != 0f) { gravLight.intensity = 0f; } }
        }
    }

    void FindObject() {
        RaycastHit hit;
        Vector3 origin = new Vector3(player.position.x, player.position.y + 1.5f, player.position.z);
        Ray ray = new Ray(origin, camNode.forward);

        if (Physics.Raycast(ray, out hit, gravRange)) {
            transform.position = hit.point;
            Collider[] objs = Physics.OverlapSphere(hit.point, accuracy);
            foreach (Collider col in objs) {
                if (col.GetComponent<Rigidbody>() && col.gameObject.layer == LayerMask.NameToLayer("Gravnull")) { target = col.gameObject; break; }
            }
        }

        else {
            travelSpeed = Vector3.Distance(transform.localPosition, initialPosition) * Time.deltaTime;
            transform.localPosition = Vector3.Slerp(transform.localPosition, initialPosition, travelSpeed);
        }

        if (gravLight.intensity != 0.7f) { gravLight.intensity = 0.7f; }
        particle.Emit(1);
    }

    void PickupObject() {
        rb = target.GetComponent<Rigidbody>();
        if (rb != null) { rb.useGravity = false; rb.velocity = Vector3.zero; rb.angularDrag = spinResistance; }
        // Add Effects ();
    }

    void PullObject() {
        travelSpeed = Vector3.Distance(rb.transform.position, transform.position) * pullStrength * Time.deltaTime;
        rb.transform.position = Vector3.Slerp(rb.transform.position, transform.position, travelSpeed);
    }

    void DropObject() {
        if (rb != null) {
            rb.useGravity = true;
            rb.angularDrag = 0.05f;
            rb.velocity = Vector3.zero;
            rb = null;
        }
        // Remove Effects ();
        target = null;
        gravLight.intensity = 0f;
    }

    void LaunchObject() {
        if (rb != null) {
            rb.useGravity = true;
            rb.angularDrag = 0.05f;
            rb.velocity = transform.forward * launchForce;
            rb = null;
        }
        // Remove Effects ();
        cooldown = 0.5f;
        target = null;
        gravLight.intensity = 0f;
    }






} // End of Class