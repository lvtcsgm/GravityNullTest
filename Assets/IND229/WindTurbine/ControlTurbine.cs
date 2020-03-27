using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTurbine : MonoBehaviour
{

    public GameObject animator;

    private Animator turbineAnim;



    // Start is called before the first frame update
    void Start()
    {

    turbineAnim = animator.GetComponent<Animator>();

if (turbineAnim == null)
{
    Debug.LogError("No Animator Component Found on " + animator.name);

}
else{
    Debug.Log("Animator Component Was Found!");
}

    }

        public void ToggleTurbineAnim()
    {
        if (turbineAnim.GetBool("toggleSpin") == false)
        {
            turbineAnim.SetBool("toggleSpin", true);

            SCRAPS_MessageSystem.instance.NewMessage("Scrapper", "I have activated the Wind Turbine!", SCRAPS_MessageSystem.msgType.standard);
        }

        else
        {
            turbineAnim.SetBool("toggleSpin", false);

            SCRAPS_MessageSystem.instance.NewMessage("Scrapper", "I have deactivated the Wind Turbine!", SCRAPS_MessageSystem.msgType.standard);

        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
