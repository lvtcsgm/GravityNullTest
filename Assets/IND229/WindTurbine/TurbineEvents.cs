using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineEvents : MonoBehaviour
{
    //Game object for the Turbine
    public GameObject TurbineObj;

    //variables for the Audio Source Components
    private AudioSource turbineAudio;
    private AudioSource stalkAudio;

    //Variables for the Audio CLips
    public AudioClip LiftUp;
    public AudioClip LiftDown;

    // Start is called before the first frame update
    void Start()
    {
        turbineAudio = TurbineObj.GetComponent<AudioSource>();

        stalkAudio = GetComponent<AudioSource>();

    }
    void PlayLiftUp()
    {
        stalkAudio.clip = LiftUp;

        stalkAudio.Play();
    }

    void PlayLiftDown()
    {
        stalkAudio.clip = LiftDown;

        stalkAudio.Play();

        turbineAudio.Stop();
    }

    void PlayWindAudio()
    {
        turbineAudio.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
