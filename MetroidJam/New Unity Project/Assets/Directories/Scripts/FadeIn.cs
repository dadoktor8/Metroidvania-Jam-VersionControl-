using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {
	AudioManager sound; 
    Animator anim;


    [SerializeField] float fadeWait = 0.5f;




    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
		sound = FindObjectOfType<AudioManager>();  
	}


    public IEnumerator FadeTo()
    {
        anim.SetBool("FadeIn", true);
        yield return new WaitForSeconds(fadeWait);
		FindObjectOfType<AudioManager> ().Play ("doorClose"); 
        anim.SetBool("FadeIn", false);
    }
}
