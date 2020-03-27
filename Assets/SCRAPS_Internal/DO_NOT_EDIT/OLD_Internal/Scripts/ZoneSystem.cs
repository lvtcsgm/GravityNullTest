using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZoneSystem : MonoBehaviour {

    public TextMeshProUGUI msgBody;

    public void NewMessage(string body)
    {
        //fix new lines for Windows
        body = body.Replace("/n", "\n");
        msgBody.text = body.ToUpper();
    }
}
