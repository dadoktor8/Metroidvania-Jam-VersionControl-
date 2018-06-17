using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobmanAI : MonoBehaviour {

	public Transform[] patrolPoints; 
	int currentPoint ; 
	public float speed = 0.5f; 
	public float timeStill = 1f;
	public float sight = 3f; 
	public float force = 100f; 
	Animator anim; 



	void Start () {
		StartCoroutine ("Patrol"); 
		anim = GetComponent<Animator> (); 

	}
	

	void Update () {

		RaycastHit2D projectileScan = Physics2D.Raycast (transform.position, transform.localScale.x * Vector2.right, sight); 

		if (projectileScan.collider != null  && (projectileScan.collider.tag == "Player"))
			


				GetComponent<Rigidbody2D> ().AddForce (Vector3.up*force + projectileScan.collider.transform.position - transform.position);



		

			

		
	}


	void OnDrawGizmos ()
	{


		Gizmos.color = Color.red; 

		Gizmos.DrawLine (transform.position, transform.transform.position + transform.localScale.x * Vector3.right * sight);


	}



	IEnumerator Patrol()
	{

		while (true) 
		{

			if (transform.position.x == patrolPoints [currentPoint].position.x)
			{

				currentPoint++; 

				anim.SetBool ("Walking", false); 

				yield return new WaitForSeconds (timeStill); 

				anim.SetBool ("Walking", true); 
			}

			if(currentPoint >=patrolPoints.Length)
			{

				currentPoint = 0; 

			}



			transform.position = Vector2.MoveTowards(transform.position , new Vector2(patrolPoints[currentPoint].position.x,transform.position.y),speed * Time.deltaTime ); 


			if (transform.position.x > patrolPoints [currentPoint].position.x) {

				transform.localScale = new Vector3 (-1, 1, 1); 


			} else if (transform.position.x > patrolPoints [currentPoint].position.x)
			{

				transform.localScale = new Vector3 (1, 1, 1); 
			}




			yield return null; 

		}

	
	
	
	}



}
