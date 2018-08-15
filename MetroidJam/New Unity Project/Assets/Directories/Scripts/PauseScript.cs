using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {


	public static bool gameIsPaused = false; 
	public GameObject pauseMenuUI; 

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (gameIsPaused)
			{
				Resume (); 
			} else
			{

				Pause ();
			}
		}
	}

	public void Resume() 
	{

		pauseMenuUI.SetActive (false); 
		Time.timeScale = 1f; 
		gameIsPaused = false; 



	}

	public void Pause()
	{
		pauseMenuUI.SetActive (true); 
		Time.timeScale = 0f; 
		gameIsPaused = true; 


	}




	public void QuitGame()
	{
		Application.Quit (); 

	}


}
