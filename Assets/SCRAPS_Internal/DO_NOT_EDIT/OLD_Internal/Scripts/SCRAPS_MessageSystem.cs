using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SCRAPS_MessageSystem : MonoBehaviour {

    private static SCRAPS_MessageSystem _instance;
    public static SCRAPS_MessageSystem instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SCRAPS_MessageSystem>();
            return _instance;
        }
    }

    public TextMeshProUGUI chatText;
    private int maxLines = 6;

    public enum msgType
    {
        standard,
        system,
        emote,
        bad
    }

    public string standardHex;
    public string goodHex;
    public string badHex;
    public string systemHex;
    public bool debugSpam = false;

    List<string> messages = new List<string>();

    void Start()
    {
        NewMessage("", "Welcome to SCRAPS v" + INIWorker.IniReadValue(INIWorker.Sections.Version, INIWorker.Keys.value1), msgType.system);

        if(debugSpam == true)
        {
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
            NewMessage("SPAM", "This is debug spam, filling up space to test the message system.  This should wrap at the end of the window.", msgType.standard);
        }
    }

    public void SystemMessage(string body)
    {
        NewMessage("System", body, msgType.system);
    }

    public void LocalMessage(string body)
    {
        NewMessage("Player", body, msgType.emote);
    }

    public void NewMessage(string from, string body, msgType type)
    {
        //remove returns
        body = body.Replace("/n", "");
        body = body.Replace("\n", "");

        if (type == msgType.system)
            from = "SYSTEM";

        //check for blank from
        if (from == "")
        {
            //format color
            from = "<color=" + systemHex + ">SYSTEM</color>";

            //format from
            from = "<b>" + from + "</b>:";

            messages.Add(from + "Error sending message, sender is null.");
            UpdateChat();
        }
        else
        {
            //color format
            if (type == msgType.standard)
                from = "<color=" + standardHex + ">" + from + "</color>";
            else if (type == msgType.system)
                from = "<color=" + systemHex + ">" + from + "</color>";
            else if (type == msgType.bad)
                from = "<color=" + badHex + ">" + from + "</color>";
            else
                from = "";

            //format from
            from = "<b>" + from + "</b>:";

            if (type == msgType.emote)
            {
                messages.Add(body);
            }
            else
            {
                messages.Add(from + " " + body);
            }
            
            UpdateChat();
        }
    }

    void UpdateChat()
    {
        if(messages.Count == maxLines)
        {
            messages.RemoveAt(0);
            messages.TrimExcess();
        }

        string[] chatMessages = messages.ToArray();
        string chatBox = "";

        foreach(string msg in chatMessages)
        {
            chatBox = chatBox + "\n" + msg;
        }

        chatText.text = chatBox;
    }
}
