using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameFunction : MonoBehaviour {


	void Update()
	{

		if (Input.GetKeyDown(KeyCode.P)) 
		{
			SaveManager.saveManager.Delete (); 
			
		}

		if (Input.GetKeyDown (KeyCode.L)) 
		{
			SaveManager.saveManager.playerPositionX = transform.position.x; 
			SaveManager.saveManager.playerPositionY = transform.position.y; 


		}


		if (Input.GetKeyDown (KeyCode.K)) 
		{
			SaveManager.saveManager.Load (); 
			transform.position = new Vector2 (
			SaveManager.saveManager.playerPositionX, 
			SaveManager.saveManager.playerPositionY 
			); 
		}



	}

}
