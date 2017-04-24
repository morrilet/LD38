using UnityEngine;
using System.Collections;

public class TransmissionTrigger : MonoBehaviour 
{
	////////// Variables //////////
	public GameObject transmissionObj;

	////////// Primary Methods //////////
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Player> ().canJump = false;
			StartTransmission ();

			AudioManager.instance.PlaySoundEffect ("Transmit");
		}
	}

	////////// Custom Methods //////////
	private void StartTransmission()
	{
		transmissionObj.SetActive (true);
		Destroy (this.gameObject);
	}
}
