using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour {

    [SerializeField]
    private int thisObjectiveID = 0;

    

    // Use this for initialization
    void Start () {
        MainGameManager.instance.IsObjectiveDone = false ;
        MainGameManager.instance.ObjectiveID = thisObjectiveID;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Got the Keys!");
            MainGameManager.instance.IsObjectiveDone = true;
            Destroy(gameObject);
        }
    }
}
