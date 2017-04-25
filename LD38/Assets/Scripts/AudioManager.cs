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
	[HideInInspector]
	public AudioSource source;
	public AudioClip CurrentMusic { get { return (source.isPlaying) ? source.clip : null; } }
	private static float playTime;
	private static AudioClip storedClip;

	////////// Primary Methods //////////
	void Awake()
	{
		if (instance != null && instance != this) 
		{
			Destroy (this.gameObject);
		} 
		else 
		{
			instance = this;
		}

		DontDestroyOnLoad (this);

		source = GetComponent<AudioSource> ();
		source.clip = storedClip;
		source.time = playTime;
		source.loop = true;
		source.Play ();
	}

	void Update()
	{
		playTime = source.time;
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

	public void PlayMusic(string musicName)
	{
		foreach (AudioClip clip in music) 
		{
			if (clip.name == musicName) 
			{
				source.clip = clip;
				source.loop = true;
				source.Play ();
				storedClip = clip;
				playTime = 0.0f;
				return;
			}
		}

		Debug.Log ("Music not found :: " + musicName);
	}
}
