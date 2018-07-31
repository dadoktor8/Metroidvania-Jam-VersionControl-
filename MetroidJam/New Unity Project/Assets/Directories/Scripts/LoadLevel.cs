using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
	
	
}
