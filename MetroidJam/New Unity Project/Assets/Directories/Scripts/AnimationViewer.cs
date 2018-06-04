using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationViewer : MonoBehaviour {

    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("1"))
        {
            anim.Play("WakingUP");
        }

        if (Input.GetKeyDown("2") )
        {
            anim.Play("Walk_boxers");
        }

        if (Input.GetKeyDown("3"))
        {
            anim.Play("PickUpTable");
        }

        if (Input.GetKeyDown("4"))
        {
            anim.Play("PickUpFloor");
        }


    }
}
