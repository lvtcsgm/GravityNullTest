using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LM_SwitchPanel : MonoBehaviour
{
    [Header("Switch Settings")]
    [Tooltip("[Allowed to Edit] Select the starting state of the switch.")]
    [SerializeField] bool isActive;

    [Header("Switch SFX")]
    [Tooltip("[Allowed to Edit] Set what sound should play when the switch activates.")]
    [SerializeField] AudioClip activateSFX = null;
    [Tooltip("[Allowed to Edit] Set what sound should play when the switch deactivates.")]
    [SerializeField] AudioClip deactivateSFX = null;

    [Header("Switch Components")]
    [Tooltip("[DO NOT EDIT] Changing this reference will break the switch's animation.")]
    [SerializeField] Transform plate = null;
    [Tooltip("[DO NOT EDIT] Changing this reference will break the switch's visual feedback.")]
    [SerializeField] MeshRenderer lightRend = null;
    [Tooltip("[DO NOT EDIT] Light object's material while active.")]
    [SerializeField] Material lightOnMaterial = null;
    [Tooltip("[DO NOT EDIT] Light object's material while inactive.")]
    [SerializeField] Material lightOffMaterial = null;

    [Header("Switch Events")]
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    // Added for legacy support
    [HideInInspector]
    public bool isActivated = false;

    // Switch Initialization
    private void Awake()
    {
        volume = GetComponentInChildren<LM_VolumeTracker>();
        if (!volume) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>LM_VolumeTracker</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this object has a child object with a <b>Collider Component</b> (set to trigger)"
                + " and the <b>LM_VolumeTracker</b> script component. Include this script's <b>UpdateSwitch</b> function in its <b>CountUpdate Event</b>.";
            Debug.LogErrorFormat(gameObject, msg, empty);
        }

        myAudio = GetComponentInChildren<AudioSource>();
        if (!myAudio) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>Audio Source</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this game object has a child object with an <b>Audio Source Component</b>.";
        }

        downPosition = upPosition = plate.localPosition;
        upPosition.y += pressDistance;
        
        if (isActive) lightRend.material = lightOnMaterial;
        else lightRend.material = lightOffMaterial;
        StartCoroutine(MovePlate());
    }

    // Switch Functionality
    public void UpdateSwitch()
    {
        if (active) return;

        if (count == 0 && volume.InVolumeCount >= 1) {
            isActive = !isActive;

            if (isActive) {
                lightRend.material = lightOnMaterial;
                PlaySoundEffect(activateSFX);
                OnActivate.Invoke();

                // Added for legacy support
                isActivated = true;
            }
            else {
                lightRend.material = lightOffMaterial;
                PlaySoundEffect(deactivateSFX);
                OnDeactivate.Invoke();

                // Added for legacy support
                isActivated = false;
            }
        }

        StopCoroutine(MovePlate());
        StartCoroutine(MovePlate());
        count = volume.InVolumeCount;
    }

    // Switch Visual Feedback
    private IEnumerator MovePlate()
    {
        active = true;

        float startTime = Time.time;
        Vector3 origin = plate.localPosition;
        Vector3 target = (volume.InVolumeCount == 0) ? upPosition : downPosition;
        float distance = Vector3.Distance(origin, target);

        while (plate.localPosition != target) {
            float covered = (Time.time - startTime) * lerpSpeed;
            float fraction = covered / distance;
            plate.localPosition = Vector3.Lerp(origin, target, fraction);
            yield return new WaitForEndOfFrame();
        }

        active = false;
    }

    // Switch Audio Feedback
    private void PlaySoundEffect(AudioClip clip)
    {
        if (!myAudio || !clip) return;
        myAudio.PlayOneShot(clip);
    }

    // Internal Variables
    private LM_VolumeTracker volume;
    private AudioSource myAudio;

    private Vector3 upPosition, downPosition;
    readonly float pressDistance = 0.08f;
    readonly float lerpSpeed = 10.0f;
    private int count = 0;
    private bool active;

} // End of Class
