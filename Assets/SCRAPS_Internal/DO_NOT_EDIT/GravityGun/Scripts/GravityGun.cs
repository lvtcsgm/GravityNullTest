using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityGun : MonoBehaviour 
{
    //////////////////////////////////////////////////////////////
    // VARIABLES EXTERNAL
    //////////////////////////////////////////////////////////////

    [Header("Basic")]

    [Tooltip("The magnitude of the force pushing objects away.")]
    [SerializeField]
    float m_pushForceMag = 40.0f;

    [Tooltip("The magnitude of the force grabbing objects.")]
    [SerializeField]
    float m_grabForceMag = 30.0f;

    //[Tooltip("Reduces the \"oscillating\" nature of objects forced towards a point.")]
    //[SerializeField]
    //float m_dampingForce = 10.0f;

    [Tooltip("The magnitude of the force used when rotating objects.")]
    [SerializeField]
    float m_rotationForce = 1.0f;

    [Tooltip("Reduces the continued spinning of an object due to rotation.")]
    [SerializeField]
    float m_rotationDamping = 10.0f;

    [Tooltip("Limits it to picking up / controlling only one object.")]
    [SerializeField]
    bool m_OnlyOneItem = false;

    [Header("Advanced")]

    [Tooltip("The \"Tag\" assigned to objects the gun can affect. Blank for everything with a rigid body.")]
    [SerializeField]
    string m_strTag = "Phys";

    [Tooltip("The amount of time for an item to remain out of the sphere of influence before it is no longer affected by the forces. Set to zero if items should drop as soon as they exit the collider.")]
    [SerializeField]
    float m_releaseTime = 1.0f;

    [Tooltip("Should gravity be disabled on the items actively held / grabbed by the gun?")]
    [SerializeField]
    bool m_disableGravity = true;

    enum RotationStyle
    {
        World,
        Local,
    }
    [Tooltip("How should rotation be applied?")]
    [SerializeField]
    RotationStyle m_rotStyle = RotationStyle.Local;

    // How should the input activate the gun
    enum InputStyle
    {
        ToggleOnPress,
        MustHoldButton,
    }
    [Tooltip("How should input activate the gun?")]
    [SerializeField]
    InputStyle m_inputStyle = InputStyle.MustHoldButton;

    [Tooltip("The name of the button to activate the grab.")]
    [SerializeField]
    //string m_buttonNameGrab = "GravityGun";
    KeyCode m_buttonGrab = KeyCode.Mouse0;

    [Tooltip("The name of the button to invoke a push.")]
    [SerializeField]
    //string m_buttonNamePush = "GravityPush";
    KeyCode m_buttonPush = KeyCode.Mouse1;

    [Tooltip("The name of the button to rotate RIGHT.")]
    [SerializeField]
    //string m_buttonNameRotRight = "A";
    KeyCode m_buttonRotRight = KeyCode.E;

    [Tooltip("The name of the button to rotate LEFT.")]
    [SerializeField]
    //string m_buttonNameRotLeft = "D";
    KeyCode m_buttonRotLeft = KeyCode.Q;

    [Tooltip("The name of the button to rotate FORWARD.")]
    [SerializeField]
    //string m_buttonNameRotForward = "W";
    KeyCode m_buttonRotForward = KeyCode.Z;

    [Tooltip("The name of the button to rotate BACKWARD.")]
    [SerializeField]
    //string m_buttonNameRotBack = "S";
    KeyCode m_buttonRotBack = KeyCode.X;

    [Header("Debug")]

    [Tooltip("Enable debug prints.")]
    [SerializeField]
    bool m_bDebugPrint = false;

    /*[Tooltip("Colors actively \"grabbed\" items as red. Green is used to indicate that the item is still under influence but will time-out since it is out of range.")]
    [SerializeField]
    bool m_bDebugColor = false;*/

    //////////////////////////////////////////////////////////////
    // VARIABLES INTERNAL
    //////////////////////////////////////////////////////////////

    // Activation status of the gun's forces
    enum State
    {
        STATE_Active,
        STATE_Inactive,
    }
    private State m_eState;

    // Is the user wishing to push the objects?
    private bool m_bDoPush = false;

    // A reference to our own collider component for activation / deactivation
    private Collider m_myCollider = null;

    // A reference to our own mesh renderer component for activation / deactivation
    private MeshRenderer m_myMeshRenderer = null;

    // INTERNAL work variables for applying forces in a particular direction
    // Class scope instead of function just to save the small overhead on the stack
    Vector3 dir, dirNorm, force;

    // Contains the game object to which the gun is apply the effect
    struct Item
    {
        public GameObject obj;
        public Rigidbody body;
        public float releaseAge;
        public float initialDrag;
        public float initialRotDrag;
        public float initialWeight;
        //public Color initialColor;
    }
    List<Item> m_items = new List<Item>();

    public ParticleSystem pSys;

    //////////////////////////////////////////////////////////////
    // UNITY API
    //////////////////////////////////////////////////////////////

	// Use this for initialization
	void Start () 
    {
        m_myCollider = GetComponent<Collider>();
        if (m_myCollider)
            m_myCollider.enabled = false;

        m_myMeshRenderer = GetComponent<MeshRenderer>();
        if (m_myMeshRenderer)
            m_myMeshRenderer.enabled = false;

        SetState(State.STATE_Inactive);
	}

    public int ItemCount { get { return m_items.Count; } }
	
    public Vector3 AverageItemPosition()
    {
        Vector3 ret = Vector3.zero;

        if(0 < m_items.Count)
        { 

            foreach(Item i in m_items)
            {
                ret += i.body.position;
            }

            ret *= 1.0F / (float)m_items.Count;

        }

        return ret;
    }

	// Update is called once per frame
	void Update () 
    {
        NullChecks();
        UpdateInput();
        UpdateForce();
        UpdateAge();
	}

    //////////////////////////////////////////////////////////////
    // UPDATES: Internal
    //////////////////////////////////////////////////////////////

    void NullChecks()
    {
        int i = 0;
        while(i < m_items.Count)
        {
            if(m_items[i].obj == null)
            {
                m_items.RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }
    }

    void UpdateInput()
    {
        // ACTIVATION
        if (m_inputStyle == InputStyle.ToggleOnPress)
        {
            if (Input.GetKeyDown(m_buttonGrab))
            {
                if (m_eState == State.STATE_Inactive)
                {
                    SetState(State.STATE_Active);
                }
                else
                {
                    SetState(State.STATE_Inactive);
                }
            }

            
        }
        else if (m_inputStyle == InputStyle.MustHoldButton)
        {
            if (Input.GetKeyDown(m_buttonGrab))
            {
                pSys.Play();

                SetState(State.STATE_Active);
            }
            else if (Input.GetKeyUp(m_buttonGrab))
            {
                pSys.Stop();
                SetState(State.STATE_Inactive);
            }
        }

        // PUSH
        if (Input.GetKey(m_buttonPush))
        {
            m_bDoPush = true;
            expireAllAges();
        }
        else
            m_bDoPush = false;

        // ROTATION
        if (m_items.Count > 0)
        {
            if (Input.GetKey(m_buttonRotRight))
            {
                ApplyRotationToAll(0.0f, -m_rotationForce * m_items[0].obj.transform.lossyScale.z, 0.0f);
            }
            else if (Input.GetKey(m_buttonRotLeft))
            {
                ApplyRotationToAll(0.0f, m_rotationForce * m_items[0].obj.transform.lossyScale.z, 0.0f);
            }
            if (Input.GetKey(m_buttonRotForward))
            {
                ApplyRotationToAll(0.0f, 0.0f, -m_rotationForce * m_items[0].obj.transform.lossyScale.z);
            }
            else if (Input.GetKey(m_buttonRotBack))
            {
                ApplyRotationToAll(0.0f, 0.0f, m_rotationForce * m_items[0].obj.transform.lossyScale.z);
            }
        }
    }

    void UpdateForce()
    {
        if (m_eState != State.STATE_Active)
            return;

        for (int i = 0; i < m_items.Count; ++i)
        {
            if (m_bDoPush)
                ApplyPushTo(m_items[i].obj);
            else
                ApplyForceTo(m_items[i].obj);
        }
    }

    void UpdateAge()
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            Item item = m_items[i];
            if (item.releaseAge > 0)
            {
                item.releaseAge -= Time.deltaTime;

                if(item.releaseAge <= 0)
                {
                    //DebugColor(item.obj, item.initialColor);
                    SetGravity(item.obj, true);
                    ResetDrag(item);
                    m_items.RemoveAt(i);
                    --i;
                }
                else
                {
                    m_items[i] = item;
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////
    // GENERAL management
    //////////////////////////////////////////////////////////////

    void SetState(State _state)
    {
        if (m_eState == _state)
            return;

        m_eState = _state;
        
        if (m_myCollider)
        {
            m_myCollider.enabled = (m_eState == State.STATE_Active);

            if (!m_myCollider.enabled)
                expireAllAges();

            if(m_myMeshRenderer)
                m_myMeshRenderer.enabled = m_myCollider.enabled;
        }
    }

    //////////////////////////////////////////////////////////////
    // PHYSICS
    //////////////////////////////////////////////////////////////

    void ApplyRotationToAll(float _x, float _y, float _z)
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            Rigidbody body = m_items[i].obj.GetComponent<Rigidbody>();
            if(body != null)
            {
                if (m_rotStyle == RotationStyle.World)
                    body.AddTorque(_x, _y, _z, ForceMode.Force);
                else
                    body.AddRelativeTorque(_x, _y, _z, ForceMode.Force);
                body.angularDrag = m_rotationDamping; // TODO: Determine if it should be set at "add" time.
            }
        }
    }

    void ApplyPushTo(GameObject _obj)
    {
        Rigidbody body = _obj.GetComponent<Rigidbody>();
        if (body)
        {
            body.velocity = Vector3.zero;

            Vector3 dirNorm = transform.parent.forward;
            Vector3 force = dirNorm * (m_pushForceMag * body.mass);

            body.AddForce(force, ForceMode.Impulse);
        }
    }

    // http://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
    void ApplyForceTo(GameObject _obj, bool _dampen = true)
    {
        Rigidbody objRigBody = _obj.GetComponent<Rigidbody>();
        HingeJoint hinge = _obj.GetComponent<HingeJoint>();
        SpringJoint spring = _obj.GetComponent<SpringJoint>();
        if (objRigBody)
        {
            /*Vector3 dir = transform.position - _obj.transform.position;
            Vector3 dirNorm = dir;// dir.normalized;
            Vector3 force = dirNorm * m_grabForceMag;

            objRigBody.AddForce(force, ForceMode.Force);

            if(_dampen)
            {
                float dampenThreshold = -0.5f;
                float dot = Vector3.Dot(objRigBody.velocity.normalized, dirNorm.normalized);
                
                if(dot < dampenThreshold)
                {
                    objRigBody.velocity.Set(0, 0, 0);
                    objRigBody.drag = m_dampingForce;
                }
            }*/

            Vector3 moveLocal = transform.position;

            if (hinge == null && spring == null) {
                RigidbodyConstraints constraints = objRigBody.constraints;

                if ((constraints & RigidbodyConstraints.FreezePositionX) != RigidbodyConstraints.None)
                    moveLocal = new Vector3(_obj.transform.position.x, moveLocal.y, moveLocal.z);

                if ((constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.None)
                    moveLocal = new Vector3(moveLocal.x, _obj.transform.position.y, moveLocal.z);

                if ((constraints & RigidbodyConstraints.FreezePositionZ) != RigidbodyConstraints.None)
                    moveLocal = new Vector3(moveLocal.x, moveLocal.y, _obj.transform.position.z);

                _obj.transform.position = Vector3.MoveTowards(_obj.transform.position, moveLocal, Time.deltaTime * m_grabForceMag);
            }
            else {
                Vector3 dir = transform.position - _obj.transform.position;
                Vector3 force = dir * m_grabForceMag;
                objRigBody.AddForce(force, ForceMode.Force);
            }

        }
    }

    void SetGravity(GameObject _obj, bool _enable)
    {
        Rigidbody objRigBody = _obj.GetComponent<Rigidbody>();
        if (m_disableGravity && objRigBody)
            objRigBody.useGravity = _enable;
    }

    void ResetDrag(Item _item)
    {
        if (_item.body != null)
        {
            _item.body.drag = _item.initialDrag;
            _item.body.angularDrag = _item.initialRotDrag;
        }
    }

    //////////////////////////////////////////////////////////////
    // TRIGGER events
    //////////////////////////////////////////////////////////////

    void OnTriggerEnter(Collider _other)
    {
        DebugLog("ENTER: name(" + _other.name + ") tag(" + _other.tag + ")");

        if (m_OnlyOneItem && m_items.Count > 0)
        {
            SetItemAge(_other.gameObject, 0);
            SaveWeight(_other.gameObject);
        }
        else if (m_strTag == "" || m_strTag == _other.tag)
        {
            if (_other.gameObject.GetComponent<Rigidbody>())
            {
                SetGravity(_other.gameObject, false);
                AddToList(_other.gameObject);
                SetItemAge(_other.gameObject, 0);
                SaveWeight(_other.gameObject);
            }
        }
    }

    void OnTriggerStay(Collider _other)
    {
        DebugLog("STAY: name(" + _other.name + ") tag(" + _other.tag + ")");
        
        #if false
            if (_other.tag == "Phys")
            {
                if (m_bDoPush)
                    ApplyPushTo(_other.gameObject);
                else
                    ApplyForceTo(_other.gameObject);
            }
        #endif
    }

    void OnTriggerExit(Collider _other)
    {
        DebugLog("EXIT: name(" + _other.name + ") tag(" + _other.tag + ")");

        if (m_strTag == "" || m_strTag == _other.tag)
        {
            float age = (m_releaseTime <= 0.0f ? 0.001f : m_releaseTime);
            SetItemAge(_other.gameObject, age);
            ReverseWeight(_other.gameObject);
        }
    }

    //////////////////////////////////////////////////////////////
    // LIST Management
    //////////////////////////////////////////////////////////////

    void AddToList(GameObject _obj)
    {
        if (!IsInList(_obj))
        {
            Item item = new Item();
            item.obj = _obj;
            item.releaseAge = 0;

            /*Renderer theRenderer = _obj.GetComponent<Renderer>();
            if (theRenderer != null)
            {
                item.initialColor = theRenderer.material.color;
            }*/

            Rigidbody theBody = _obj.GetComponent<Rigidbody>();
            if (theBody != null)
            {
                item.body = theBody;
                item.initialDrag = theBody.drag;
                item.initialRotDrag = theBody.angularDrag;
            }

            m_items.Add(item);
        }
    }

    bool IsInList(GameObject _obj)
    {
        foreach (Item item in m_items)
        {
            if (item.obj.Equals(_obj))
                return true;
        }
        return false;
    }

    void RemoveFromList(GameObject _obj)
    {
        for(int i=0; i<m_items.Count; ++i)
        {
            if(m_items[i].obj.Equals(_obj))
            {
                m_items[i].body.mass = m_items[i].initialWeight;
                m_items.RemoveAt(i);
                return;
            }
        }
    }

    //////////////////////////////////////////////////////////////
    // LIST Helpers
    //////////////////////////////////////////////////////////////

    void SetItemAge(GameObject _obj, float _age)
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            if (m_items[i].obj.Equals(_obj))
            {
                Item item = m_items[i];
                item.releaseAge = _age;
                m_items[i] = item;
                return;
            }
        }
    }

    void expireAllAges()
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            Item item = m_items[i];
            item.releaseAge = 0.001f;
            if(item.body.mass != item.initialWeight)
                item.body.mass = item.initialWeight;
            m_items[i] = item;
        }
    }

    void SaveWeight(GameObject _obj)
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            if (m_items[i].obj.Equals(_obj))
            {
                Item item = m_items[i];
                item.initialWeight = item.body.mass;
                item.body.mass = 1;
                m_items[i] = item;
                return;
            }
        }
    }

    void ReverseWeight(GameObject _obj)
    {
        for (int i = 0; i < m_items.Count; ++i)
        {
            if (m_items[i].obj.Equals(_obj))
            {
                Item item = m_items[i];
                item.body.mass = item.initialWeight;
                m_items[i] = item;
                return;
            }
        }
    }

    //////////////////////////////////////////////////////////////
    // DEBUG Helpers
    //////////////////////////////////////////////////////////////

    void DebugLog(string _msg)
    {
        if (m_bDebugPrint)
            print(_msg);
    }
}
