using UnityEngine;

public class Tutorial_GravnullObjective : MonoBehaviour {

    public SCRAPS_Objective objective;
    public Rigidbody[] rbs;

    public GameObject vfx;

    private void Update()
    {
        if (objective.isComplete == false)
        {
            if (vfx.activeSelf != objective.isHere)
                vfx.SetActive(objective.isHere);
        }
        else
        {
            if (vfx.activeSelf != false)
                vfx.SetActive(false);
        }
    }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        objective.objectiveMaxValue = rbs.Length;
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody bodyCheck = null;

        if(other.GetComponent<Rigidbody>() != null)
        {
            bodyCheck = other.GetComponent<Rigidbody>();
        }

        if (bodyCheck != null)
        {
            foreach (Rigidbody rb in rbs)
            {
                if(rb.gameObject == bodyCheck.gameObject)
                {
                    bodyCheck.transform.parent = null;
                    objective.UpdateObjective(1);
                    rbs = GetComponentsInChildren<Rigidbody>();
                }
            }
        }
    }
}
