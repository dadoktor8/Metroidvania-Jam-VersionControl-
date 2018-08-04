using System.Collections;
//using System.Collections;
using System.Runtime.Serialization.Formatters.Binary; 
using System; 
using System.IO; 
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour {

	public static SaveManager saveManager; 
	public int scene = SceneManager.GetActiveScene().buildIndex;
	public float playerPositionX; 
	public float playerPositionY; 


	public void Awake ()
	{
	// float scene = SceneManager.GetActiveScene().buildIndex;

		if (saveManager == null) {
			DontDestroyOnLoad (gameObject);
			saveManager = this; 


		} else if (saveManager != this) {

			Destroy (gameObject); 


		}
	}
		public void Save()
		{ 
		//float scene = SceneManager.GetActiveScene().buildIndex;

		BinaryFormatter bf = new BinaryFormatter (); 
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat"); 

		playerData data = new playerData (); 
		data.playerPosX = playerPositionX; 
		data.playerPosY = playerPositionY; 
		data.playerScene = scene; 

		bf.Serialize (file, data); 
		file.Close (); 

	}

	public void Load()
	{


		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) 
		{
			//float scene = SceneManager.GetActiveScene().buildIndex;
			BinaryFormatter bf = new BinaryFormatter (); 
			FileStream file = File.Open (Application.persistentDataPath + "/playerIndo.dat", FileMode.Open); 


			playerData data = (playerData)bf.Deserialize (file); 
			file.Close (); 

			playerPositionX = data.playerPosX; 
			playerPositionY = data.playerPosY;
			scene = data.playerScene;

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
	public int playerScene; 


}


