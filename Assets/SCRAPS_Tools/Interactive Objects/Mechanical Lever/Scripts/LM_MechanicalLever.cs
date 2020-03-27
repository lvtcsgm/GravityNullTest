using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LM_MechanicalLever : MonoBehaviour
{
    [Header("Lever Settings")]
    [Tooltip("[Allowed to Edit] Select the starting state of the lever.")]
    [SerializeField] LEVERSTATE leverState = LEVERSTATE.REVERSE;

    [Header("Lever SFX")]
    [Tooltip("[Allowed to Edit] Set what sound should play when the lever is pulled forward.")]
    [SerializeField] AudioClip pullForwardSFX = null;
    [Tooltip("[Allowed to Edit] Set what sound should play when the lever is pulled reverse.")]
    [SerializeField] AudioClip pullReverseSFX = null;

    [Header("Lever Components")]
    [Tooltip("[DO NOT EDIT] Changing this reference will break the lever's animation.")]
    [SerializeField] Transform leverPivot = null;

    [Header("Lever Events")]
    public UnityEvent pullForward;
    public UnityEvent pullReverse;

    // Added for legacy support
    [HideInInspector]
    public bool isForward = false;


    // Lever Trigger
    public void Toggle() { if (!isActive) StartCoroutine(MoveLever()); }

    // Lever Initialization
    private void Awake()
    {
        myAudio = GetComponentInChildren<AudioSource>();
        if (!myAudio) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>Audio Source</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this game object has a child object with an <b>Audio Source Component</b>.";
            Debug.LogErrorFormat(gameObject, msg, empty);
        }

        myInteraction = GetComponentInChildren<SCRAPS_Interactive>();
        if (!myInteraction) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>SCRAPS_Interactive</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this object has a child object with a <b>Collider Component</b> (set to trigger)"
                + " and the <b>SCRAPS_Interactive</b> script component. Include this script's <b>Toggle</b> function in its <b>OnInteract Event</b>.";
            Debug.LogErrorFormat(gameObject, msg, empty);
        }

        forwardRot = new Vector3(0, 0, rotateAngle);
        reverseRot = new Vector3(0, 0, -rotateAngle);

        if (leverState == LEVERSTATE.FORWARD)
        {
            leverPivot.localRotation = Quaternion.Euler(forwardRot);

            // Added for legacy support
            isForward = true;
        }
        else
        {
            leverPivot.localRotation = Quaternion.Euler(reverseRot);

            // Added for legacy support
            isForward = false;
        }
    }

    // Lever Functionality
    private IEnumerator MoveLever()
    {
        isActive = true;
        myInteraction.DisableInteraction();

        float startTime = Time.time;
        Quaternion origin = Quaternion.Euler((leverState == LEVERSTATE.REVERSE) ? reverseRot : forwardRot);
        Quaternion target = Quaternion.Euler((leverState == LEVERSTATE.REVERSE) ? forwardRot : reverseRot);
        float distance = Quaternion.Angle(origin, target);

        PlaySoundEffect((leverState == LEVERSTATE.REVERSE) ? pullForwardSFX : pullReverseSFX);

        if (leverState == LEVERSTATE.FORWARD) {
            pullReverse.Invoke();
            leverState = LEVERSTATE.REVERSE;
            // Added for legacy support
            isForward = true;
        }
        else {
            pullForward.Invoke();
            leverState = LEVERSTATE.FORWARD;
            // Added for legacy support
            isForward = false;
        }

        while (leverPivot.localRotation != target) {
            float covered = (Time.time - startTime) * rotateSpeed;
            float fraction = covered / distance;
            leverPivot.localRotation = Quaternion.Lerp(origin, target, fraction);
            yield return new WaitForEndOfFrame();
        }

        myInteraction.EnableInteraction();
        isActive = false;
    }

    // Lever Audio Feedback
    private void PlaySoundEffect(AudioClip clip)
    {
        if (!myAudio || !clip) return;
        myAudio.PlayOneShot(clip);
    }


    // Internal Variables
    private enum LEVERSTATE { FORWARD, REVERSE }
    private SCRAPS_Interactive myInteraction;
    private AudioSource myAudio;

    private Vector3 forwardRot, reverseRot;
    readonly float rotateAngle = 35.0f;
    readonly float rotateSpeed = 90.0f;
    private bool isActive;

} // End of Class
