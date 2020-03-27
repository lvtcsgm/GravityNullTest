using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using PivotBasedCameraRig = UnityStandardAssets.Cameras.PivotBasedCameraRig;

public class MultiviewCameraRig : PivotBasedCameraRig
{
    /* ******************************************************************************* */
    /*            MODIFIED VERSION OF UnityStandardAssets.Cameras.FreeLookCam
    /*              Includes 1st and 3rd person modes
    /* ******************************************************************************* */

    // This script is designed to be placed on the root object of a camera rig,
    // comprising 3 gameobjects, each parented to the next:

    // 	Camera Rig
    // 		Pivot
    // 			Camera

    private float m_LookAngle;                    // The rig's y axis rotation.
    private float m_TiltAngle;                    // The pivot's x axis rotation.
    private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
	private Vector3 m_PivotEulers;
	private Quaternion m_PivotTargetRot;
	private Quaternion m_TransformTargetRot;

    public UnityStandardAssets.Cameras.ProtectCameraFromWallClip protectCameraFromWallClip { get; protected set; }

    public Vector3 thirdPersonCameraOffset { get; protected set; }
    public Vector3 thirdPersonPivotOffset { get; protected set; }
    //private bool fpsToggleKey = false;

    public Transform gunPos;

    private bool toggleFirstPerson = false;

    protected override void Awake()
    {
        base.Awake();

        /* ******************************************************************************* */
        /* ******************************************************************************* */
		m_PivotEulers = m_Pivot.localEulerAngles;//.rotation.eulerAngles;
        /* ******************************************************************************* */

        protectCameraFromWallClip = GetComponent<UnityStandardAssets.Cameras.ProtectCameraFromWallClip>();

	    m_PivotTargetRot = m_Pivot.transform.localRotation;
		m_TransformTargetRot = transform.localRotation;

        thirdPersonPivotOffset = m_Pivot.localPosition;
        thirdPersonCameraOffset = m_Cam.transform.localPosition;

        m_gunPos = gunPos.localPosition;
    }

    protected void Update()
    {
        //fpsToggleKey = CrossPlatformInputManager.GetButton("ViewToggle");

        HandleRotationMovement();
        

        if (null != firstPersonNode)
        {
            if (CrossPlatformInputManager.GetButtonUp("ViewToggle") && !toggleFirstPerson)
            {
                if (null != protectCameraFromWallClip)
                    protectCameraFromWallClip.enabled = false;

                m_Cam.transform.localRotation =
                m_Pivot.localRotation = Quaternion.identity;
                m_Cam.transform.localPosition =
                m_Pivot.localPosition = Vector3.zero;
                m_Pivot.SetParent(firstPersonNode, false);

                gunPos.localPosition = new Vector3(m_gunPos.x, m_gunPos.y, m_gunPos.z -3);

                toggleFirstPerson = true;
            }
            else if (CrossPlatformInputManager.GetButtonUp("ViewToggle") && toggleFirstPerson)
            {
                m_Cam.transform.localRotation =
                m_Pivot.localRotation = Quaternion.identity;
                m_Cam.transform.localPosition = thirdPersonCameraOffset;
                m_Pivot.localPosition = thirdPersonPivotOffset;
                m_Pivot.SetParent(transform, false);

                gunPos.localPosition = new Vector3(m_gunPos.x, m_gunPos.y, m_gunPos.z);

                if (null != protectCameraFromWallClip)
                    protectCameraFromWallClip.enabled = true;

                toggleFirstPerson = false;
            }
        }

        if (null != Target)
        {
            Target.localRotation = transform.localRotation;
        }
    }


    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    protected override void FollowTarget(float deltaTime)
    {
        if (null == Target) 
            return;

        // Move the rig towards target position.
        transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime*m_MoveSpeed);
    }


    private void HandleRotationMovement()
    {
		if (Time.timeScale < float.Epsilon)
		    return;

        // Read the user input
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");

        if (invertYAxis)
            y = -y;

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        m_LookAngle += x*m_TurnSpeed;

        // Rotate the rig (the root object) around Y axis only:
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

        if (m_VerticalAutoReturn)
        {
            // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
            // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
            // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
            m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
        }
        else
        {
            // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
            m_TiltAngle -= y*m_TurnSpeed;
            // and make sure the new value is within the tilt range
            m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
        }

        // Tilt input around X is applied to the pivot (the child of this object)
		m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);

		if (m_TurnSmoothing > 0)
		{
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
		}
		else
		{
			m_Pivot.localRotation = m_PivotTargetRot;
			transform.localRotation = m_TransformTargetRot;
		}
    }

    [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
    [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    [SerializeField] private float m_TurnSmoothing = 0.1f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
    [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
    [SerializeField] private bool m_VerticalAutoReturn = false;           // set wether or not the vertical axis should auto return
    [SerializeField] protected Transform firstPersonNode;
    [SerializeField] protected bool invertYAxis;
    Vector3 m_gunPos;
    protected bool m_LockCursor = false;
}
