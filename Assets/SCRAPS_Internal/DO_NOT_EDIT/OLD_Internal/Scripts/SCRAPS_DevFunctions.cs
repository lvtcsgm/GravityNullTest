using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_DevFunctions : MonoBehaviour {

    public static List<GameObject> GetObjectsInLayer(GameObject root, int layer)
    {
        var ret = new List<GameObject>();

        foreach (Transform t in FindObjectsOfType<Transform>())
        {
            if (t.gameObject.layer == layer)
            {
                ret.Add(t.gameObject);
            }
        }
        return ret;
    }
}