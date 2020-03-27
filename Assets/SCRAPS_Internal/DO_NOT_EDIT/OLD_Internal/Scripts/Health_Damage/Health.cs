using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour 
{
	public int maxHealth = 10;
	public int currentHealth;
	public Transform respawnMarker = null;
	public bool triggerExplosion = false;

    public AudioClip[] hurtSFX;

	// Use this for initialization
	void Start () 
	{
		currentHealth = maxHealth;
	}

	public void ApplyDamage(int amount) 
	{
		currentHealth -= amount;

        SCRAPS_AudioManager.instance.PlaySoundOnce(hurtSFX[Random.Range(0, hurtSFX.Length)], 1);

		if(currentHealth <= 0)
		{
            currentHealth = 0;

            SCRAPS_MessageSystem.instance.NewMessage("PLAYER", "You've died... respawning to <b>" + respawnMarker.parent.GetComponent<Checkpoint>().locName + "</b>", SCRAPS_MessageSystem.msgType.bad);

			if(triggerExplosion)
			{
				SendMessage("DoDamage", SendMessageOptions.DontRequireReceiver);
			}

			if(respawnMarker)
			{
                if (transform.GetComponent<Rigidbody>() != null)
                    transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

				transform.position = respawnMarker.position;
				currentHealth = maxHealth;
			}
			else
			{
                gameObject.SetActive(false);
			}
		}
	}
}
