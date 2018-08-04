using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LoadDatScript : MonoBehaviour {

	[SerializeField] GameObject text; 

 public static SaveManager saveManager; 

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player") 
		{
			text.SetActive (true); 

		}

	}

	void OnTrigerStay2D (Collider2D col)
	{

		if (Input.GetKeyDown (KeyCode.X)) {
			if (col.gameObject.tag == "Player") {
				
				text.SetActive (true); 

				SaveManager.saveManager.Load (); 
				transform.position = new Vector2 (
					SaveManager.saveManager.playerPositionX, 
					SaveManager.saveManager.playerPositionY 
				);

				//SceneManager.LoadScene (SaveManager.saveManager.scene.ToString);
				Application.LoadLevel(SaveManager.saveManager.scene); 
				 
			}
		}
	}
}