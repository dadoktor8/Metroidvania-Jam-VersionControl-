using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

    [SerializeField] GameObject text;


    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.Quit();
            Debug.Log("Quit");



        }

    }

}
