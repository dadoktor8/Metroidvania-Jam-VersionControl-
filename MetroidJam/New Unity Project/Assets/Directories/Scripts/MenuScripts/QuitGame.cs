using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

    [SerializeField] GameObject text;

    bool isQuit = false;

    void Update()
    {
        if (isQuit)
        {
            Debug.Log("Quit");
            Application.Quit();

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
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

        if (Input.GetKeyDown(KeyCode.X))
        {
                  
            isQuit = true;

        }

    }

}
