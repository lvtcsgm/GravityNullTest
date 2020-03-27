///////////////////////////////////////////////////////////////////////////////////////////////
// GravityWell.cs
// Created By: Anthony Lowder
//
// Description:
// Responsible for collecting and effecting physics objects. 
// While active checks for collision with rigibodies, collects them, pulls them toward world
// location, and checks for input to push away collected rigidbodies.
///////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour {

    //////////////////////////////////////////////////
    // INTERNAL VARIABLES
    //////////////////////////////////////////////////

    // Contains the rigidbody the GravWell will affect
    private struct PhysObj
    {
        public Rigidbody obj;
        public bool inside;
        public float timer;
        public float initDrag;
        public bool initGrav;
    }

    private List<PhysObj> objects = new List<PhysObj>(); // Holds the collection of effected objects
    private bool push; // If the player wishes to push effected objects away
    private bool OnlyOneObj = true; // Is the GravWell only allowed to effect one object
    private int layerIndex = 27; // The index of the Gravnull layer

    //////////////////////////////////////////////////
    // EXTERNAL VARIABLES
    //////////////////////////////////////////////////

    //[Header("Effected Objs")]
    //[Tooltip("Will only affect objects with this \"Tag\". Leave blank to affect all rigidbodies.")]
    //[SerializeField] string objTag = "Phys";

    [Header("Carry Parameters")]
    [Tooltip("The magnitude of the force pulling effected objects.")]
    [SerializeField] private float pullForce = 1.0f;
    [Tooltip("The magnitude of the force pushing effected objects away.")]
    [SerializeField] private float pushForce = 1.0f;
    [Tooltip("How quickly the effected objects will stop rotating.")]
    [SerializeField] private float angDrag = 1.0f;
    [Tooltip("Is multi object carrying allowed?")]
    [SerializeField] private bool multiCarry = false;

    [SerializeField] float pushCooldown = 1.0f;
    private bool cooldown;
    private float cooldownTimer;

    [Header("Drop Parameters")]
    [Tooltip("How far away effected objects need to be before uneffected.")]
    [SerializeField] private float dropDist = 1.0f;
    [Tooltip("How long effected objects need to be away before uneffected.")]
    [SerializeField] private float dropTime = 1.0f;
    [Tooltip("Mask for raycast below player's feet to drop carried objects.")]
    [SerializeField] private LayerMask dropMask = 0;
    [SerializeField] private Transform player = null;

    [Header("Input Commands")]
    [Tooltip("The name of the button to toggle multi object carrying.")]
    [SerializeField] private KeyCode multiButton = KeyCode.Mouse2;
    [Tooltip("The name of the button to invoke push.")]
    [SerializeField] private KeyCode pushButton = KeyCode.Mouse1;

    [Header("Debugging")]
    [Tooltip("Enable debug prints.")]
    [SerializeField] private bool printDebug = false;

    [Header("Feedback")]
    [Tooltip("Visual and Audio feedback.")]
    [SerializeField] private ParticleSystem push_VFX = null;
    private AudioSource sfx_source;
    [SerializeField] private AudioClip loop_SFX = null;
    [SerializeField] private AudioClip push_SFX = null;

    //////////////////////////////////////////////////
    // MONOBEHAVIOUR FUNCTIONS
    //////////////////////////////////////////////////

    private void OnEnable()
    {
        if (Time.timeScale != 0)
        {
            sfx_source = GetComponent<AudioSource>();

            if (sfx_source.isPlaying == false)
            {
                sfx_source.loop = true;
                sfx_source.clip = loop_SFX;
                sfx_source.volume = 0.25f;
                sfx_source.pitch = 0.5f;
                sfx_source.Play();
            }
        }
    }
    private void OnDisable()
    {
        if (sfx_source.isPlaying == true && sfx_source.clip == loop_SFX)
        {
            sfx_source.Stop();
            sfx_source.loop = false;
            sfx_source.clip = null;
        }

        DropObject();
    }

    private void Update() {
        NullChecks();
        UpdateInput();

        if(objects.Count > 0 && Input.GetKeyDown(pushButton))
        {
            SCRAPS_AudioManager.instance.PlaySoundOnce(push_SFX, 0.12f);
        }
    }
    private void FixedUpdate() { if (objects.Count > 0 && !cooldown) { UpdateTimers(); UpdateForce(); DropUnderneath(); } }

    //////////////////////////////////////////////////
    // MAIN UPDATES
    //////////////////////////////////////////////////

    private void NullChecks() {
        int i = 0;
        while (i < objects.Count) {
            if (objects[i].obj == null) { RemoveFromList(i); }
            else { i++; }
        }
    } // Removes affected objects that are destroyed

    private void UpdateInput() {
        if (Input.GetKeyDown(pushButton) && objects.Count > 0 && !cooldown) { DebugLog("GravityWell :: Push Activated"); push = true; }
        else if (Input.GetKeyDown(multiButton) && multiCarry) { ToggleMultiCarry(); DebugLog("GravityWell :: Multi-Carry Toggled - " + !OnlyOneObj); }

        if (cooldown) CooldownTimer();
    } // Player input check
    private void UpdateForce() {
        for (int i = 0; i < objects.Count; i++) { if (push) { PushObject(objects[i].obj); } else { PullObject(objects[i].obj); } }
        if (push) {
            for (int i = 0; i < objects.Count; i++) {
                objects[i].obj.angularDrag = objects[i].initDrag;
                objects[i].obj.useGravity = objects[i].initGrav;
            }
            objects.Clear(); push = false;
            cooldown = true;
        }
    } // Applies the pull or push effect to affected objects
    private void UpdateTimers() {
        for (int i = 0; i < objects.Count; i++) {
            if (!objects[i].inside) {
                RunObjTimer(i);
                float dist = Vector3.Distance(objects[i].obj.transform.position, transform.position);
                if (objects[i].timer >= dropTime && dist >= dropDist) {
                    DebugLog("GravityWell :: " + objects[i].obj.name + " Dropped - Out of Range");
                    RemoveFromList(i);
                }
            }
        }
    } // Drops affected objects that are out of range for too long

    private void DropUnderneath() {
        RaycastHit hit;
        Vector3 direction = (player.position + new Vector3(0, -0.25f, 0)) - player.position;
        Ray ray = new Ray(player.position, direction);
        Debug.DrawRay(player.position, direction, Color.blue);

        if (Physics.Raycast(ray, out hit, 1f, dropMask)) {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null) {
                int index = CheckListFor(rb);
                if (index >= 0) { RemoveFromList(index); }
            }
        }
    } // Drops affected objects that are directly below the player

    private void CooldownTimer()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer < pushCooldown) return;
        cooldown = false;
    }

    //////////////////////////////////////////////////
    // EFFECTING COLLECTED OBJS
    //////////////////////////////////////////////////

    private void PullObject(Rigidbody rb) {
        Vector3 direction = transform.position - rb.transform.position;
        rb.velocity = (direction * pullForce);
        DebugLog("GravityWell :: " + rb.name + " Pulled");
    }
    private void PushObject(Rigidbody rb) {
        rb.velocity = Vector3.zero;
        Vector3 dirNorm = transform.parent.forward;
        Vector3 force = dirNorm * (pushForce * rb.mass);
        rb.AddForce(force, ForceMode.Impulse);
        //DebugLog("GravityWell :: " + rb.name + " Pushed");

        push_VFX.Emit(100);
    }
    public void DropObject() {
        for (int i = 0; i < objects.Count; i++) { RemoveFromList(i); }
        //DebugLog("GravityWell :: All Objs Dropped");
    }

    private void ToggleMultiCarry() { if (OnlyOneObj) { OnlyOneObj = false; } else { OnlyOneObj = true; DropObject(); } }

    //////////////////////////////////////////////////
    // EFFECTED OBJ TRACKING
    //////////////////////////////////////////////////

    private int CheckListFor(Rigidbody rb) {
        for (int i = 0; i < objects.Count; i++) { if (rb == objects[i].obj) { return i; } }
        return -1;
    }
    private void AddToList(Rigidbody rb) {
        PhysObj newObj = new PhysObj();
        newObj.obj = rb;
        newObj.inside = true;
        newObj.timer = 0;
        newObj.initDrag = rb.angularDrag;
        newObj.initGrav = rb.useGravity;
        rb.useGravity = false;
        rb.angularDrag = angDrag;
        SetCarryLayer(rb);
        objects.Add(newObj);
    }
    private void RemoveFromList(int index) {
        if (index >= 0 && index < objects.Count) {
            if (objects[index].obj != null) {
                objects[index].obj.useGravity = objects[index].initGrav;
                objects[index].obj.angularDrag = objects[index].initDrag;
            }
            objects.RemoveAt(index);
        }
    }

    private void StartObjTimer(int index) {
        PhysObj newObj = objects[index];
        newObj.inside = false;
        objects[index] = newObj;
    }
    private void RunObjTimer(int index) {
        PhysObj newObj = objects[index];
        newObj.timer += Time.deltaTime;
        objects[index] = newObj;
    }
    private void ResetObjTimer(int index) {
        PhysObj newObj = objects[index];
        newObj.inside = true;
        newObj.timer = 0;
        objects[index] = newObj;
    }

    private void SetCarryLayer(Rigidbody obj) {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        foreach (Transform child in children) { child.gameObject.layer = layerIndex; }
    }
    public int GetObjCount() { return objects.Count; }

    public bool IsCarryingObj()
    {
        if (objects.Count > 0) return true;
        return false;
    }

    //////////////////////////////////////////////////
    // COLLISION EVENTS
    //////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<SCRAPS_GravnullObject>() != null) {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) {
                int index = CheckListFor(rb);
                if (index >= 0) { ResetObjTimer(index); }
                else { if (OnlyOneObj && objects.Count <= 0) { AddToList(rb); } else if (!OnlyOneObj) { AddToList(rb); } }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<SCRAPS_GravnullObject>() != null) {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) { int index = CheckListFor(rb); if (index >= 0) { StartObjTimer(index); } }
        }
    }

    //////////////////////////////////////////////////
    // DEBUGGING
    //////////////////////////////////////////////////

    private void DebugLog(string message) { if (printDebug) { Debug.Log(message); } }

} // End of Class