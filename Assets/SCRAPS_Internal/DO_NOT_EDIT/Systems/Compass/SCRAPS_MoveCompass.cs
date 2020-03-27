using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_MoveCompass : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 newPos = SCRAPS_ObjectManager.instance.player.transform.position;
        newPos.y = 10000;
        transform.position = newPos;
	}
}
