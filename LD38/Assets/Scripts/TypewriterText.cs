using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This attaches to a text object and applies a typewriter effect to it's text.
/// </summary>
public class TypewriterText : MonoBehaviour 
{
	////////// Variables //////////
	public float charSpeed; //Speed between characters.
	public float spaceSpeed; //Speed for a space.
	private string storedText;
	private Text text;

	////////// Primary Methods //////////
	void Start()
	{
		text = this.GetComponent<Text> ();
		storedText = text.text;
		text.text = "";

		ApplyEffect (text, storedText);
	}

	////////// Custom Methods //////////
	private void ApplyEffect(Text textObj, string input)
	{
		StartCoroutine (ApplyEffect_Coroutine (textObj, input));
	}

	private IEnumerator ApplyEffect_Coroutine(Text textObj, string input)
	{
		char[] chars = input.ToCharArray ();

		yield return new WaitForSeconds (charSpeed);
		for (int i = 0; i < chars.Length; i++) 
		{
			textObj.text += chars [i];
			if (chars [i] == ' ') 
			{
				yield return new WaitForSeconds (spaceSpeed);
			}
			else 
			{
				yield return new WaitForSeconds (charSpeed);
			}
		}
		yield return new WaitForSeconds (1.5f);
		StartCoroutine (FadeOut (1.5f));
		yield return new WaitForSeconds (2.0f);
		GameManager.instance.LoadNextLevel ();
	}

	private IEnumerator FadeOut(float speed)
	{
		for (float i = 0; i <= speed; i += Time.deltaTime) 
		{
			float opacity = 1.0f - (i / speed);
			text.color = new Color (text.color.r, text.color.g, text.color.b, opacity);
			yield return null;
		}
	}
}
