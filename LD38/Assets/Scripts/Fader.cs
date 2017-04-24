using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour 
{
	////////// Variables //////////
	private Image img;

	////////// Primary Methods //////////
	void Start()
	{
		img = GetComponent<Image> ();
	}

	////////// Custom Methods //////////
	public void FadeIn(float duration)
	{
		StartCoroutine (FadeIn_Coroutine (duration));
	}

	public void FadeOut(float duration)
	{
		StartCoroutine (FadeOut_Coroutine (duration));
	}

	private IEnumerator FadeIn_Coroutine(float duration)
	{
		for (float i = 0; i <= duration; i += Time.deltaTime) 
		{
			Color color = img.color;
			color.a = i / duration;
			img.color = color;
			yield return null;
		}
	}

	private IEnumerator FadeOut_Coroutine(float duration)
	{
		for (float i = 0; i <= duration; i += Time.deltaTime) 
		{
			Color color = img.color;
			color.a = 1.0f - (i / duration);
			img.color = color;
			yield return null;
		}
	}
}
