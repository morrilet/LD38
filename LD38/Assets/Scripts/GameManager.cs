using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	////////// Variables //////////
	public static GameManager instance;
	public string[] scenes;

	////////// Primary Methods //////////
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			RestartLevel ();
		}
	}

	////////// Custom Methods //////////
	public void RestartLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void LoadNextLevel()
	{
		for (int i = 0; i < scenes.Length; i++) 
		{
			if (SceneManager.GetActiveScene ().name.Equals (scenes [i])) 
			{
				if (i + 1 == scenes.Length) 
				{
					SceneManager.LoadScene (scenes[0]); //The main menu.
					break;
				}
				else
				{
					SceneManager.LoadScene (scenes [i + 1]); //Next level.
					break;
				}
			}
		}
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
