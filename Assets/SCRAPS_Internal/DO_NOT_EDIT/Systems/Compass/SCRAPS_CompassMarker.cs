using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_CompassMarker : MonoBehaviour {

    private float yHeight = 10004;

    // Use this for initialization
    void Awake () {
        Vector3 newPos = transform.position;
        newPos.y = yHeight;
        transform.position = newPos;
	}
}
