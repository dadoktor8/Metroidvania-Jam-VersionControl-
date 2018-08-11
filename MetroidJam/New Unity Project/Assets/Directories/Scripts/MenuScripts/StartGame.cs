using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartGame : MonoBehaviour {

	[SerializeField] GameObject text;
    [SerializeField]
    int Level = 0;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadFirstLevel(int x)
	{

        SceneManager.LoadScene(x);

    }

	void OnTriggerEnter2D(Collider2D col)
	{

		if(col.gameObject.tag == "Player")
		{
			text.SetActive(true);
		}

	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			text.SetActive(false);
		}
	}

	private void OnTriggerStay2D(Collider2D col)
	{

		if (Input.GetKeyDown (KeyCode.X)) 
		{
            //Application.LoadLevel ("Cinematic 1_EP"); 
            LoadFirstLevel(Level);

           

        }

	}


}
