using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_Bastion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SCRAPS_ObjectiveList.instance.CreateObjective("ascore_bastion", "Get to the Bastion");
	}
}
