using System.Collections;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary; 
using System; 
using System.IO; 
using UnityEngine;
//using UnityEditor.SceneManagement; 

public class SaveManager : MonoBehaviour {

	public static SaveManager saveManager; 

	public float playerPositionX; 
	public float playerPositionY; 



	void Awake ()
	{
		if (saveManager == null) {
			DontDestroyOnLoad (gameObject);
			saveManager = this; 


		} else if (saveManager != this) {

			Destroy (gameObject); 


		}
	}
		public void Save()
		{ 

		BinaryFormatter bf = new BinaryFormatter (); 
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat"); 

		playerData data = new playerData (); 
		data.playerPosX = playerPositionX; 
		data.playerPosY = playerPositionY; 


		bf.Serialize (file, data); 
		file.Close (); 

	}

	public void Load()
	{


		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter (); 
			FileStream file = File.Open (Application.persistentDataPath + "/playerIndo.dat", FileMode.Open); 


			playerData data = (playerData)bf.Deserialize (file); 
			file.Close (); 

			playerPositionX = data.playerPosX; 
			playerPositionY = data.playerPosY;

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

class playerData
{
	public float playerPosX; 
	public float playerPosY; 


}


