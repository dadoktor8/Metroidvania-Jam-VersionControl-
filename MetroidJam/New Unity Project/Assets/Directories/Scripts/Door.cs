using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    [SerializeField] GameObject currentGrid;
    [SerializeField] GameObject nextGrid;
    [SerializeField] float fadeWait = 0.5f;
    [SerializeField] Animator anim;

    [SerializeField] GameObject text;

    // Use this for initialization
    void Start () {
       
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            text.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.X) )
        {
            Debug.Log("Press X to open!");
            if(col.gameObject.tag == "Player")
            {
                anim.SetBool("FadeIn", true);
                StartCoroutine(FadeTo());
            }
            

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
        /* if(col.gameObject.tag == "Player")
         {
             Debug.Log("Press X to open!");

             if (Input.GetKeyDown(KeyCode.X))
             {
                 anim.SetBool("FadeIn", true);
                 StartCoroutine(FadeTo() );

             }

         }*/

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Press X to open!");
            if (col.gameObject.tag == "Player")
            {
                anim.SetBool("FadeIn", true);
                StartCoroutine(FadeTo());
            }


        }
    }

    IEnumerator FadeTo()
    {
        yield return new WaitForSeconds(fadeWait);
        anim.SetBool("FadeIn", false);
        currentGrid.SetActive(false);
        nextGrid.SetActive(true);
    }
}
