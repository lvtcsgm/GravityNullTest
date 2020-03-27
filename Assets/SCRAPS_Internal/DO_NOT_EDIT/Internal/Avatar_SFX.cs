using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar_SFX : MonoBehaviour
{
    private AudioSource aSource;

    public AudioClip[] footSFX, exertSFX;

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    void PlayFootStep()
    {
        aSource.PlayOneShot(footSFX[Random.Range(0, footSFX.Length)], Random.Range(0.03f, 0.05f));
    }

    public void PlayExert()
    {
        aSource.PlayOneShot(exertSFX[Random.Range(0, exertSFX.Length)], Random.Range(0.45f, 0.55f));
    }
}