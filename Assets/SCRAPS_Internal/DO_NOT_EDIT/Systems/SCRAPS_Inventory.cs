using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCRAPS_Inventory : MonoBehaviour {

    private static SCRAPS_Inventory _instance;
    public static SCRAPS_Inventory instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SCRAPS_Inventory>();
            return _instance;
        }
    }

    private int scrapValue = 0;
    private int currentValue = 0;

    public enum valueType
    {
        low,
        medium,
        high
    }

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI keyItemsText;

    public List<KeyItem> keyItems = new List<KeyItem>();

    public void Collect(valueType value)
    {
        if(value == valueType.low)
        {
            scrapValue += Random.Range(10, 50);
        }
        else if(value == valueType.medium)
        {
            scrapValue += Random.Range(50, 100);
        }
        else if (value == valueType.high)
        {
            scrapValue += Random.Range(100, 1000);
        }
    }

    public void CollectKey(string name)
    {
        KeyItem i = new KeyItem();
        i.keyItemName = name;

        keyItems.Add(i);
    }

    public int GetKeyItemAmount(string name)
    {
        KeyItem[] keyArray;
        keyArray = keyItems.ToArray();

        int count = 0;

        for (int i = 0; i < keyArray.Length; i++)
        {
            if (keyArray[i].keyItemName == name)
                count++;
        }

        return count;
    }

    public bool CanConsumeKeyItem(string name, int amount)
    {
        KeyItem[] keyArray;
        keyArray = keyItems.ToArray();

        KeyItem[] selectedKeys = new KeyItem[keyArray.Length];

        int count = 0;

        for (int i = 0; i < keyArray.Length; i++)
        {
            if (keyArray[i].keyItemName == name)
            {
                count++;
                selectedKeys[count -1] = keyArray[i];
            }
        }

        if (count >= amount)
        {
            //we can use it
            for (int i = 0; i < selectedKeys.Length; i++)
            {
                keyItems.Remove(selectedKeys[i]);

                if (i == amount -1)
                    break;
            }

            return true;
        }
        else
            return false;
    }

    private void Update()
    {
        valueText.text = "$" + currentValue;

        if(currentValue < scrapValue)
        {
            currentValue += (int)(100 * Time.deltaTime);
        }
        else
        {
            currentValue = scrapValue;
        }

        KeyItem[] keyArray;
        keyArray = keyItems.ToArray();

        string display = "";

        for (int i = 0; i < keyArray.Length; i++)
        {
            display += "● "+keyArray[i].keyItemName + "\n";
        }

        keyItemsText.text = display;
    }
}

public class KeyItem
{
    public string keyItemName;
}