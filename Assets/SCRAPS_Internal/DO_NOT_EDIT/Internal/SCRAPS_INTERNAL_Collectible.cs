using UnityEngine;

public class SCRAPS_INTERNAL_Collectible : MonoBehaviour {

    /* SCRAPS_INTERNAL_Collectible.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_Collectible.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    public SCRAPS_Inventory.valueType value = SCRAPS_Inventory.valueType.low;

    public bool isKeyItem = false;
    public string keyItemName = "";

    public void Collect()
    {
        if (!isKeyItem)
        {
            SCRAPS_Inventory.instance.Collect(value);
            SCRAPS_MessageSystem.instance.LocalMessage("You found something of <b>value</b>.");
            SCRAPS_AudioManager.instance.PlaySound(SCRAPS_AudioManager.soundType.collect, 1);
        }
        else
        {
            SCRAPS_Inventory.instance.CollectKey(keyItemName);
            SCRAPS_MessageSystem.instance.LocalMessage("You found: <b>" + keyItemName + "</b>");
            SCRAPS_AudioManager.instance.PlaySound(SCRAPS_AudioManager.soundType.positive, 1);
        }

        Destroy(gameObject);
    }
}