using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour {

    public string version = "scraps";

	// Use this for initialization
	void Start () {
        Time.timeScale = 1.0f;

        if(version == "scraps")
            GetComponent<Text>().text = INIWorker.IniReadValue(INIWorker.Sections.Version, INIWorker.Keys.value1);
        else if(version == "unity")
        {
            string current = Application.unityVersion;
            string iniVer = INIWorker.IniReadValue(INIWorker.Sections.Version, INIWorker.Keys.value2);

            if(current == iniVer)
            {
                GetComponent<Text>().text = INIWorker.IniReadValue(INIWorker.Sections.Version, INIWorker.Keys.value2);
            }
            else
            {
                GetComponent<Text>().text = "<color=red>" + Application.unityVersion + "</color>";
            }
            
        }
    }
}
