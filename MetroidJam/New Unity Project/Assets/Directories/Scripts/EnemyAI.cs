using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; 

public class EnemyAI : MonoBehaviour {


	Animator enemyAnime; 

	Rigidbody2D rigidBlob; 

	float enemyTriggerDistance = 3f; 

	float enemyAttackDistance = 1f; 

	float speed = 4f; 

	public Transform targetProtagonist;  

	void Start () {
		//enemyAnime = GetComponent<Animator> (); 
		rigidBlob = GetComponent<Rigidbody2D> (); 
		targetProtagonist = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> (); 
	}
	

	void Update () 
	{
		if (Vector2.Distance (transform.position, targetProtagonist.position) > 1) {

			transform.position = Vector2.MoveTowards (transform.position, targetProtagonist.position, speed * Time.deltaTime); 

		}

	}
}
