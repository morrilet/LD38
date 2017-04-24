using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Fades out an object, then shows a transmission, then fades in another object.
public class MainMenuTriggerObj : MonoBehaviour 
{
	////////// Variables //////////
	public GameObject controlsTextObj; //The object to fade out. (Controls text)
	public GameObject transmissionObj; //The transmission to show.
	public GameObject bgFader; //The object to fade in. (Background fader)
	public bool isQuitTrigger; //Whether or not this triger is the quit trigger.
	private GameObject[] menuTriggerObjs;

	////////// Primary Methods //////////
	void Start()
	{
		menuTriggerObjs = GameObject.FindGameObjectsWithTag ("MenuTriggerObj");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			PerformAction ();

			for (int i = 0; i < menuTriggerObjs.Length; i++) 
			{
				Destroy (menuTriggerObjs[i].GetComponent<SpriteRenderer> ());
				Destroy (menuTriggerObjs[i].GetComponent<Collider2D> ());
			}
		}
	}
		
	////////// Custom Methods //////////
	private void PerformAction()
	{
		GameObject.FindWithTag ("Player").GetComponent<Player> ().canJump = false;
		
		StartCoroutine (PerformAction_Coroutine ());
	}
		
	private IEnumerator PerformAction_Coroutine()
	{
		//Fade out the controls object.
		float controlTimer = 0.0f;
		while (controlTimer <= 2.0f) 
		{
			Color colorTemp = controlsTextObj.GetComponent<Text> ().color;
			colorTemp.a = 1.0f - (controlTimer / 2.0f);
			controlsTextObj.GetComponent<Text> ().color = colorTemp;

			controlTimer += Time.deltaTime;
			yield return null;
		}

		//Enable/start the transmission text.
		transmissionObj.SetActive (true);
		yield return new WaitForSeconds ((transmissionObj.GetComponent<Text>().text.Length *
			transmissionObj.GetComponent<TypewriterText>().charSpeed) + 3.0f);

		//Fade out to black using the bgFader.
		bgFader.GetComponent<Fader>().FadeIn (2.0f);
		yield return new WaitForSeconds (2.5f);

		//If we're the quit action, quit. Otherwise, go to the next level.
		if (isQuitTrigger) 
		{
			GameManager.instance.QuitGame ();
		}
		else
		{
			GameManager.instance.LoadNextLevel ();
		}
	}
}
