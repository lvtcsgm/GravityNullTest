using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
public GameObject animatorL;

public GameObject animatorR;
private Animator openLeftDoor;
private Animator openRightDoor;

public AudioSource openSFX;

public AudioSource closeSFX;

//public static bool hasFallen = false;

public GameObject padlock;

    // Start is called before the first frame update
    void Start()
    {
        openRightDoor = animatorR.GetComponent<Animator>();

        openLeftDoor = animatorL.GetComponent<Animator>();


        //logic check for right door
        if(openLeftDoor == null)
        {
            Debug.LogError("No Animator Component Found on " + animatorL.name);
        }
        else 
        {
            Debug.Log("Animator Component was Found!");
        }
        if(openRightDoor == null)
        {
            Debug.LogError("No Animator Component Found on " + animatorR.name);
        }
        else 
        {
            Debug.Log("Animator Component was Found!");
        }
    }

public void ToggleDoorOpen()
{

if (openLeftDoor == true)
{
    
  padlock.SetActive (true);
    
    // change rb parameters


    // play audio
    
   // hasFallen = true;
}

    if(openLeftDoor.GetBool("isDoorOpen") == false)
    {
        Debug.Log("Door is open!");
        openSFX.Play();
        openLeftDoor.SetBool("isDoorOpen", true);
    }
     else
     {
         Debug.Log("Door is closed");
         openLeftDoor.SetBool("isDoorOpen" , false);
     }

     // if/else
   if(openRightDoor.GetBool("isDoorOpen") == false)
    {
        Debug.Log("Door is open!");
        openRightDoor.SetBool("isDoorOpen", true);
    }
     else
     {
         Debug.Log("Door is closed");
         closeSFX.Play();
         openRightDoor.SetBool("isDoorOpen" , false);
     }

}


    // Update is called once per frame
    void Update()
    {
        
    }
}
