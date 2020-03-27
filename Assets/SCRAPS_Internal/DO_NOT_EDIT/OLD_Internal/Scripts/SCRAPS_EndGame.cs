using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCRAPS_EndGame : MonoBehaviour {

    private bool lockPlayer = false;
    private Vector3 lockPos = Vector3.zero;
    private Animator cutsceneAnimator;

    private void Start()
    {
        if(cutsceneAnimator == null)
        {
            //bandage because we've already exported the package
            cutsceneAnimator = GameObject.Find("Monorail_Front_Pivot").GetComponentInParent<Animator>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("LockPlayer", 0.5f);
            Invoke("GoToCredits", 6);
        }
    }

    void LockPlayer()
    {
        if(lockPlayer == false)
        {
            SCRAPS_ObjectManager.instance.player.transform.parent = transform;
            lockPos = SCRAPS_ObjectManager.instance.player.transform.localPosition;
            cutsceneAnimator.SetTrigger("go");
            lockPlayer = true;
        }
    }

    private void Update()
    {
        if(lockPlayer == true)
        {
            SCRAPS_ObjectManager.instance.player.transform.localPosition = lockPos;
        }
    }

    void GoToCredits()
    {
        SceneManager.LoadScene("SCRAPS_CREDITS");
    }

    public void MessageNote(string note)
    {
        SCRAPS_AudioManager.instance.PlayInspectSound();
        SCRAPS_MessageSystem.instance.NewMessage("NOTICE", note, SCRAPS_MessageSystem.msgType.emote);
    }
}
