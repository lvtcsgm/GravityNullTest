using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LM_PressurePlate : MonoBehaviour
{
    [Header("Pressure Plate Settings")]
    [Tooltip("[Allowed to Edit] Set the number of objects are required to be placed on the pressure plate.")]
    [SerializeField] int requiredObjectCount = 1;

    [Header("Pressure Plate SFX")]
    [Tooltip("[Allowed to Edit] Set what sound should play when the pressure plate activates.")]
    [SerializeField] AudioClip activateSFX = null;
    [Tooltip("[Allowed to Edit] Set what sound should play when the pressure plate deactivates.")]
    [SerializeField] AudioClip deactivateSFX = null;

    [Header("Pressure Plate Components")]
    [Tooltip("[DO NOT EDIT] Changing this reference will break the plate's animation.")]
    [SerializeField] Transform plate = null;
    [Tooltip("[DO NOT EDIT] Changing this reference will break the meter's animation.")]
    [SerializeField] Transform meter = null;

    [Header("Pressure Plate Events")]
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    // Added for legacy support
    [HideInInspector]
    public bool isPressed = false;
    [HideInInspector]
    public bool isActivated = false;

    // Pressure Plate Initialization
    private void Awake()
    {
        volume = GetComponentInChildren<LM_VolumeTracker>();
        if (!volume) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>LM_VolumeTracker</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this object has a child object with a <b>Collider Component</b> (set to trigger)"
                + " and the <b>LM_VolumeTracker</b> script component. Include this script's <b>UpdatePressurePlate</b> function in its <b>CountUpdate Event</b>.";
            Debug.LogErrorFormat(gameObject, msg, empty);
        }

        myAudio = GetComponentInChildren<AudioSource>();
        if (!myAudio) {
            object[] empty = new object[0];
            string msg = "<b><color=red>[PROBLEM]</color> " + name + "</b> is missing its <b>Audio Source</b> reference.";
            msg += "\n<b><color=green>[SOLUTION]</color></b> Make sure that this game object has a child object with an <b>Audio Source Component</b>.";
        }

        upPosition = plate.localPosition;
        upPosition.y += pressDistance;
        float percent = Mathf.Clamp(volume.InVolumeCount, 0f, requiredObjectCount) / requiredObjectCount;
        StartCoroutine(MovePlate(percent));
    }

    // Pressure Plate Functionality
    public void UpdatePressurePlate()
    {
        if (active) return;

        float percent = Mathf.Clamp(volume.InVolumeCount, 0f, requiredObjectCount) / requiredObjectCount;

        if (isActive && percent != 1) {
            isActive = false;
            PlaySoundEffect(deactivateSFX);
            OnDeactivate.Invoke();

            // Added for legacy support
            isPressed = false;
            isActivated = false;
        }
        else if (!isActive && percent == 1) {
            isActive = true;
            PlaySoundEffect(activateSFX);
            OnActivate.Invoke();

            // Added for legacy support
            isPressed = true;
            isActivated = true;
        }

        StopAllCoroutines();
        StartCoroutine(MovePlate(percent));
    }

    // Plate Visual Feedback
    private IEnumerator MovePlate(float percent)
    {
        active = true;

        float startTime = Time.time;
        float amount = pressDistance * percent;
        Vector3 origin = plate.localPosition;
        Vector3 target = new Vector3(upPosition.x, upPosition.y - amount, upPosition.z);
        float distance = Vector3.Distance(origin, target);

        while (plate.localPosition != target) {
            float covered = (Time.time - startTime) * lerpSpeed;
            float fraction = covered / distance;
            plate.localPosition = Vector3.Lerp(origin, target, fraction);
            yield return new WaitForEndOfFrame();
        }

        Material myMat = meter.GetComponent<MeshRenderer>().material;
        meter.localScale = new Vector3(percent, 1, 1);
        myMat.color = Color.Lerp(Color.red, Color.green, percent);
        active = false;
    }

    // Pressure Plate Audio Feedback
    private void PlaySoundEffect(AudioClip clip)
    {
        if (!myAudio || !clip) return;
        myAudio.PlayOneShot(clip);
    }

    // Internal Variables
    private LM_VolumeTracker volume;
    private AudioSource myAudio;

    private Vector3 upPosition;
    readonly float pressDistance = 0.08f;
    readonly float lerpSpeed = 10.0f;
    private bool isActive, active;

} // End of Class
