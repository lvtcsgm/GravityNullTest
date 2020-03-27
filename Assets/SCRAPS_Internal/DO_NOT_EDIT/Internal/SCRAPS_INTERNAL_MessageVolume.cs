using UnityEngine;

public class SCRAPS_INTERNAL_MessageVolume : MonoBehaviour {

    /* SCRAPS_INTERNAL_MessageVolume.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use MessageVolume.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    [Header("Who/What said it?")]
    public string msgFrom; //who is sending the message?
    [Header("What is being said?")]
    public string msgBody; //what does the message say?

    public enum typeMessage
    {
        standard,
        bad
    }

    [Header("What type of message is being sent?")]
    public typeMessage msgT = typeMessage.standard;
    [Header("Does it vanish forever after being displayed?")]
    public bool onlyOnce = false; //do we only want this message to show one time?

    //When a collider enters our trigger volume
    void OnTriggerEnter(Collider other)
    {
        //is it the player?
        if (other.tag == "Player")
        {
            //format our message
            SCRAPS_MessageSystem.msgType myType = SCRAPS_MessageSystem.msgType.standard;

            if (msgT == typeMessage.standard)
                myType = SCRAPS_MessageSystem.msgType.standard;
            else if (msgT == typeMessage.bad)
                myType = SCRAPS_MessageSystem.msgType.bad;

            //send a new message
            SCRAPS_MessageSystem.instance.NewMessage(msgFrom, msgBody, myType);
        }
    }

    //When a collider exits our trigger volume
    void OnTriggerExit(Collider other)
    {
        //was it the player?
        if (other.tag == "Player")
        {
            //are we only showing this once?
            if (onlyOnce)
                Destroy(this.gameObject); //destroy this message volume
        }
    }
}