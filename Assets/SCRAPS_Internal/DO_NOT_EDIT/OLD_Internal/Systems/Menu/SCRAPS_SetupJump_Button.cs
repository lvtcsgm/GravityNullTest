using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRAPS_SetupJump_Button : MonoBehaviour {

    private bool collectedSetups = false;
    public Text buttonText;
    public Button but;
    public Vector3 telepoint;
    private int num = 0;
    public ASCoreMain ascoreMain;

    void Update()
    {
        if (collectedSetups == false)
        {
            ASCoreMain ascoreMain = GameObject.FindObjectOfType<ASCoreMain>();

            if(ascoreMain == null)
                collectedSetups = true;
            else
            {
                if (ascoreMain.myPoints[num] != null)
                {
                    telepoint = ascoreMain.myPoints[num].respawnPos.position;
                    buttonText.text = ascoreMain.myPoints[num].locName;

                    if (num +1 >= ascoreMain.myPoints.Length)
                    {
                        collectedSetups = true;
                    }
                    else
                    {
                        GameObject nextButton = Instantiate(gameObject, transform.position, transform.rotation);
                        nextButton.GetComponent<SCRAPS_SetupJump_Button>().num = num + 1;
                        RectTransform newRect = nextButton.GetComponent<RectTransform>();
                        RectTransform myRect = GetComponent<RectTransform>();
                        newRect.SetParent(myRect.parent, false);
                        /*newRect.sizeDelta = myRect.sizeDelta;
                        newRect.anchorMin = myRect.anchorMin;
                        newRect.anchorMax = myRect.anchorMax;
                        newRect.anchoredPosition = myRect.anchoredPosition;
                        newRect.localScale = myRect.localScale;*/
                        //newRect.Translate(Vector3.down * 9.0f);
                        collectedSetups = true;
                    }
                }
            }
        }
    }

    public void TeleportToSetup()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = telepoint;
    }
}
