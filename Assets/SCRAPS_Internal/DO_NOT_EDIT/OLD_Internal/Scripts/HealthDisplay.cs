using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HealthDisplay : MonoBehaviour {

	[SerializeField]
	private Health CurHealth = null;

	[SerializeField]
	private TextMeshProUGUI myGuiText = null;

    [SerializeField]
    private Image myBar = null;

    private float displayHealth = 100;
    private float speed = 2;

    public bool isDelayed = false;

	void Update() {

        if (isDelayed == true)
        {
            if (displayHealth > CurHealth.currentHealth)
            {
                displayHealth -= ((displayHealth - CurHealth.currentHealth) * speed) * Time.deltaTime;
            }
            else
            {
                displayHealth = CurHealth.currentHealth;
            }

            if (myBar != null)
            {
                myBar.fillAmount = ((float)displayHealth / CurHealth.maxHealth);
            }
        }
        else
        {
            if (myBar != null)
            {
                myBar.fillAmount = ((float)CurHealth.currentHealth / CurHealth.maxHealth);
            }

            if (myGuiText != null)
            {
                myGuiText.text = "" + CurHealth.currentHealth + "/" + CurHealth.maxHealth;
            }
        }
	}
}

