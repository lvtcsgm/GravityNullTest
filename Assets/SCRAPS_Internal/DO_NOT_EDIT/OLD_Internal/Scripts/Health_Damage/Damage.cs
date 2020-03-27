using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class Damage : MonoBehaviour
{
	public int baseDamage = 10;
	//public GameObject fxObj = null;
	//public AudioClip sound = null;
}