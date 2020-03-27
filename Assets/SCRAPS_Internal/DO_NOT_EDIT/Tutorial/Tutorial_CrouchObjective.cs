using UnityEngine;

public class Tutorial_CrouchObjective : MonoBehaviour {

    public SCRAPS_Objective objective;
    public GameObject vfx;

    private void Update()
    {
        if(objective.isComplete == false)
        {
            if (vfx.activeSelf != objective.isHere)
                vfx.SetActive(objective.isHere);
        }
        else
        {
            if(vfx.activeSelf != false)
                vfx.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKey(KeyCode.C))
        {
            //the player is here and crouching
            if(objective.isComplete == false)
                objective.UpdateObjective(1);
        }
    }
}