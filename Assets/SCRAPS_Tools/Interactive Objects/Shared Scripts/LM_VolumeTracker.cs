using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LM_VolumeTracker : MonoBehaviour
{
    [Header("Volume Events")]
    public UnityEvent CountUpdate;

    // Object Tracking
    private List<Rigidbody> inVolume = new List<Rigidbody>();
    public int InVolumeCount { get => inVolume.Count; }

    private void Awake() { GetComponent<Collider>().isTrigger = true; }
    private void OnEnable() { InvokeRepeating("UpdateNulls", 0, 0.5f); }
    private void OnDisable() { CancelInvoke("UpdateNulls"); }

    // Remove Nulls from List
    private void UpdateNulls()
    {
        int startCount = InVolumeCount;
        for(int i = inVolume.Count - 1; i >= 0; i--)  {
            if (!inVolume[i]) inVolume.RemoveAt(i);
        }

        if (startCount != InVolumeCount) CountUpdate.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (!rb) return;

        inVolume.Add(rb);
        CountUpdate.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (!rb) return;

        inVolume.Remove(rb);
        CountUpdate.Invoke();
    }
}
