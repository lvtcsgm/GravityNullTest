using UnityEngine;

public class ZoneVolume : MonoBehaviour {

    [Header("Name to display for your zone")]
    public string levelName;
    private ZoneSystem zs;

    void Start()
    {
        zs = GameObject.FindObjectOfType<ZoneSystem>();
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zs.NewMessage(levelName);
            SCRAPS_MessageSystem.instance.NewMessage("", "Now Entering: <b>" + levelName+"</b>", SCRAPS_MessageSystem.msgType.system);
        }
    }
}
