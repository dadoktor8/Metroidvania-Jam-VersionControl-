using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; 

public class EnemyAI : MonoBehaviour {


	Animator enemyAnime; 

	Rigidbody2D rigidBlob; 

	//float enemyTriggerDistance = 3f; 

	//float enemyAttackDistance = 1f; 

	float speed = 1f; 

	private bool movingRight = true; 

	public Transform groundDetection; 

	public Transform targetProtagonist;  

	void Start () {
		
		rigidBlob = GetComponent<Rigidbody2D> (); 
		targetProtagonist = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> (); 
	}
	

	void Update ()
	{

		 transform.Translate (Vector2.right * speed * Time.deltaTime); 

		RaycastHit2D groundInfo = Physics2D.Raycast (groundDetection.position, Vector2.down, 1f);


		if (groundInfo.collider == false)
		{

			if (movingRight == true) {

				transform.eulerAngles = new Vector3 (0, -180, 0); 

				movingRight = false; 

			} else 
			{

				transform.eulerAngles = new Vector3 (0, 0, 0); 
				movingRight = true; 
			}


				if (Vector2.Distance (transform.position, targetProtagonist.position) > 1) {

					transform.position = Vector2.MoveTowards (transform.position, targetProtagonist.position, speed * Time.deltaTime);

				}
			}

		}
}

