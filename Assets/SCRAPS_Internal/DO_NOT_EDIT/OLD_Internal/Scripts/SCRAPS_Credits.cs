using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRAPS_Credits : MonoBehaviour {

    private Text creditsText;

	// Use this for initialization
	void Start () {
        if (creditsText == null)
            creditsText = GetComponent<Text>();

        creditsText.text = INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value1) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value2) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value3) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value4) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value5) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value6) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value7) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value8) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value9) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value10) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value11) + "\n" +
                           INIWorker.IniReadValue(INIWorker.Sections.Credits, INIWorker.Keys.value12) + "\n";

    }
}
