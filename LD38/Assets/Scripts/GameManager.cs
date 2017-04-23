using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	////////// Primary Methods //////////
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			RestartLevel ();
		}
	}

	////////// Custom Methods //////////
	void RestartLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
