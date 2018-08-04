using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameFunction : MonoBehaviour {

	[SerializeField] GameObject text; 


	public static SaveManager saveManager; 

	/*	if (Input.GetKeyDown(KeyCode.P)) 
		{
			SaveManager.saveManager.Delete (); 
			
		}*/



		/*if (Input.GetKeyDown (KeyCode.K)) 
		{
			SaveManager.saveManager.Load (); 
			transform.position = new Vector2 (
			SaveManager.saveManager.playerPositionX, 
			SaveManager.saveManager.playerPositionY 
			); 
		}*/



	

	private void OnTriggerEnter2D(Collider2D col)
	{

		if(col.gameObject.tag == "Player")
		{
			text.SetActive(true);
		}

	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			text.SetActive(false);
		}
	}




	private void OnTriggerStay2D(Collider2D col)
	{
		if (Input.GetKeyDown (KeyCode.X)) {
			if (col.gameObject.tag == "Player") {

				SaveManager.saveManager.playerPositionX = transform.position.x; 
				SaveManager.saveManager.playerPositionY = transform.position.y; 
				SaveManager.saveManager.scene = SceneManager.GetActiveScene().buildIndex; 
			}
		}
	}

}
