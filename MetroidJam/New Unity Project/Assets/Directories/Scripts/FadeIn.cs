using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    Animator anim;
    [SerializeField] float fadeWait = 0.5f;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}


    public IEnumerator FadeTo()
    {
        anim.SetBool("FadeIn", true);
        yield return new WaitForSeconds(fadeWait);
        anim.SetBool("FadeIn", false);
    }
}
