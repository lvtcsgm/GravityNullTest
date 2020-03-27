using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRAPS_AudioManager : MonoBehaviour {

    private static SCRAPS_AudioManager _instance;
    public static SCRAPS_AudioManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SCRAPS_AudioManager>();
            return _instance;
        }
    }

    private AudioSource source;

    public AudioClip systemSFX;
    public AudioClip collectSFX;
    public AudioClip positiveSFX;
    public AudioClip negativeSFX;
    public AudioClip inspectSFX;

    public enum soundType
    {
        system,
        positive,
        negative,
        collect,
        inspect
    }

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	public void PlaySound(AudioClip clip, float pitch)
    {
        source.Stop();
        source.clip = clip;
        source.pitch = pitch;
        source.Play();
    }

    public void PlaySoundOnce(AudioClip clip, float volume)
    {
        source.PlayOneShot(clip, volume);
    }

    public void PlaySound(soundType sound, float pitch)
    {
        source.Stop();
        if (sound == soundType.system)
            source.clip = systemSFX;
        else if (sound == soundType.positive)
            source.clip = positiveSFX;
        else if (sound == soundType.negative)
            source.clip = negativeSFX;
        else if (sound == soundType.collect)
            source.clip = collectSFX;
        else if (sound == soundType.inspect)
            source.clip = inspectSFX;

        source.pitch = pitch;
        source.Play();
    }

    public void PlaySystemSound()
    {
        source.Stop();
        source.clip = systemSFX;
        source.pitch = 1.0f;
        source.Play();
    }

    public void PlayPositiveSound()
    {
        source.Stop();
        source.clip = positiveSFX;
        source.pitch = 1.0f;
        source.Play();
    }

    public void PlayNegativeSound()
    {
        source.Stop();
        source.clip = negativeSFX;
        source.pitch = 0.5f;
        source.Play();
    }

    public void PlayInspectSound()
    {
        source.Stop();
        source.clip = inspectSFX;
        source.pitch = 1.0f;
        source.Play();
    }
}