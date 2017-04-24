using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour 
{
	////////// Variables //////////
	public GameObject fadeBackground;

	////////// Primary Methods //////////
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			PullPlayerIn (other.gameObject.GetComponent<Player> (), 60.0f);
			ChangeScene (3.0f, 4.0f);
			Destroy (this.GetComponent<Collider2D> ());
		}
	}

	////////// Custom Methods //////////
	void ChangeScene(float duration, float delay)
	{
		StartCoroutine (ChangeScene_Coroutine (duration, delay));
	}

	void PullPlayerIn(Player player, float duration)
	{
		this.GetComponent<Planet> ().gravRadius = 0.0f;
		player.canJump = false;
		player.KeepFeetOnPlanet = false;
		player.SpinAndShrink (duration, 10.0f);
		StartCoroutine (PullPlayerIn_Coroutine (player, duration));
	}

	IEnumerator ChangeScene_Coroutine(float duration, float delay)
	{
		yield return new WaitForSeconds (delay);
		fadeBackground.GetComponent<Fader> ().FadeIn (duration);
		yield return new WaitForSeconds (duration);
		GameManager.instance.LoadNextLevel ();
	}

	IEnumerator PullPlayerIn_Coroutine(Player player, float duration)
	{
		Rigidbody2D playerRB = player.GetComponent<Rigidbody2D> ();
		for (float i = 0; i <= duration; i += Time.deltaTime) 
		{
			playerRB.velocity = Vector2.zero;
			player.transform.position = 
				Vector3.Lerp (player.transform.position, transform.position, i / duration);
			yield return null;
		}
	}
}
