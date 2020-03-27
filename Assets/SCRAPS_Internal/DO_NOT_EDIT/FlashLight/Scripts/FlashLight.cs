using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {

    [SerializeField] KeyCode toggleKey = KeyCode.F;
    Light lightComp;

	// Use this for initialization
	void Start () {
        lightComp = GetComponent<Light>();
        lightComp.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(toggleKey))
        {
            lightComp.enabled = !lightComp.enabled;
        }
	}
}
