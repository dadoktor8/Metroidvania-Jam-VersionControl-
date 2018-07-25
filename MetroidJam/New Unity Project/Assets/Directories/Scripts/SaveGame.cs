using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.G)) 
		{
			SaveController.saveController.Delete (); 

		}

		if (Input.GetKeyDown (KeyCode.Y))
		{
			SaveController.saveController.playerPositionX = transform.position.x; 
			SaveController.saveController.playerPositionY = transform.position.y; 

		}

		if (Input.GetKeyDown (KeyCode.U)) 
		{
			SaveController.saveController.Load (); 
			transform.position = new Vector3 (
			SaveController.saveController.playerPositionX,
			SaveController.saveController.playerPositionY); 


		}


	}
}
