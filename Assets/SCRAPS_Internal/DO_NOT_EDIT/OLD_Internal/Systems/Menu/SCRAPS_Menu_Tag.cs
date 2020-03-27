using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRAPS_Menu_Tag : MonoBehaviour {

    public string[] tags;
    private Text text;

	// Use this for initialization
	void Awake () {
        text = gameObject.GetComponent<Text>();
        text.text = tags[Random.Range(0, tags.Length)].ToUpper();
	}
}
