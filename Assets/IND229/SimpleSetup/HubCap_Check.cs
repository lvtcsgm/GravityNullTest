using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubCap_Check : MonoBehaviour
{
public GameObject HubCap;

public GameObject GB_HubCap;
public void AttachHubcap()
{

    if (SCRAPS_Inventory.instance.CanConsumeKeyItem("Hub Cap", 1))
    {
        HubCap.SetActive(true);

        GB_HubCap.SetActive(false);

        SCRAPS_MessageSystem.instance.NewMessage("Scrapper", "I snapped a hub cap in place!", SCRAPS_MessageSystem.msgType.standard);

        gameObject.GetComponent<SCRAPS_Interactive>().enabled = false;

    }

    else
    {
        SCRAPS_MessageSystem.instance.NewMessage("Scrapper", "There must be a hub cap around here..", SCRAPS_MessageSystem.msgType.standard);
    }

}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
