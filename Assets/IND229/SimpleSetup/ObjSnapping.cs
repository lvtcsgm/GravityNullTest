using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSnapping : MonoBehaviour
{
public GameObject staticObj;

private void OnTriggerEnter(Collider other)
{
        /*
        if (staticObj.activeInHierarchy == false)
    {*/

        if(other.GetComponent<Snap>() != null)
        {

            Destroy(other.gameObject);

            staticObj.SetActive(true);

            SCRAPS_MessageSystem.instance.NewMessage("Scrapper", "I snapped a tire in place!", SCRAPS_MessageSystem.msgType.standard);

            gameObject.SetActive(false);

        }
/*
    }
    */

}

    // Start is called before the first frame update
   void Start()
   {
        
   }

   //  Update is called once per frame
    void Update()
   {
           
   }





}
