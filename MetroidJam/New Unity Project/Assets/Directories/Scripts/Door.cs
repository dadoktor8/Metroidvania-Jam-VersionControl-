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

    AudioSource source;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D col)
	{

		if(col.gameObject.tag == "Player")
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
            Debug.Log("Press X to open!");
            if (col.gameObject.tag == "Player")
            {
                anim.SetBool("FadeIn", true);
                source.Play();
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
