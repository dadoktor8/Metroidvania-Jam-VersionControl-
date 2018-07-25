using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO; 
using System; 

public class SaveController : MonoBehaviour {

	public static SaveController saveController; 

	public float playerPositionX; 
	public float playerPositionY; 
	// Include your health script

	public void Awake () {

		if (saveController == null) {
			DontDestroyOnLoad (gameObject);
			saveController = this; 
		} else if (saveController != this) 
		{
			Destroy (gameObject); 


		}
			
		
	}
	

	public void Save () {

		// Create  a Binary formatter along with a file 

		BinaryFormatter bf = new BinaryFormatter (); 
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat"); 

		//Creates a object to save game data 
		PlayerData data = new PlayerData (); 
		data.playerPosX = playerPositionX; 
		data.playerPosY = playerPositionY; 

		//Write and Close

		bf.Serialize (file, data); 
		file.Close (); 


	}

	public void Load() 
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat"))
		{

			BinaryFormatter bf = new BinaryFormatter (); 
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open); 


			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close (); 
			playerPositionX = data.playerPosX ; 
			playerPositionY = data.playerPosY ; 
			// Add health value

		}
		
			


	}

	public void Delete() 
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) 
		{

			File.Delete (Application.persistentDataPath + "/playerInfo.dat");

		}


	}


}


[Serializable]
class PlayerData 
{

	public float playerPosX;
	public float playerPosY; 
	// Add health value
}