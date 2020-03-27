using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCRAPS_StopWatch : MonoBehaviour {

    public TextMeshProUGUI number;
    private float time = 0;
    private bool showTimer = false;

	// Use this for initialization
	void Start () {
        number.text = "";
	}
	
	// Update is called once per frame
	void Update () {

        if (showTimer)
        {
            time += Time.deltaTime;
            int mins = (int)time / 60;
            int secs = (int)time % 60;

            number.text = string.Format("{0}:{1}", mins, secs);
        }
        else
        {
            number.text = "";
        }
    }

    public void StartTimer()
    {
        showTimer = true;
        time = 0;
    }

    public void StopTimer()
    {
        showTimer = false;
    }
}
