using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	////////// Variables //////////
	public static AudioManager instance;
	public List<AudioClip> music;
	public List<AudioClip> soundEffects;
	private AudioSource source;

	////////// Primary Methods //////////
	void Awake()
	{
		if (instance != null) 
		{
			Destroy(instance);
		} 
		instance = this;
	}

	void Start()
	{
		source = GetComponent<AudioSource> ();
	}

	////////// Custom Methods //////////
	public void PlaySoundEffect(string clipName)
	{
		foreach (AudioClip clip in soundEffects) 
		{
			if (clip.name == clipName) 
			{
				source.PlayOneShot (clip);
				break;
			}
		}
	}

	public void PlayMusic()
	{
		
	}
}
