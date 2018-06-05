using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNewScript : MonoBehaviour {

	private Rigidbody2D myRigidbody; 

	Animator anim; 

	[SerializeField]
	private float movementSpeed; 

	private bool facingRight ; 

	void Start () {

		myRigidbody = GetComponent<Rigidbody2D> (); 

		anim = GetComponent<Animator> (); 

		facingRight = true; 

	}
	

	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis("Horizontal"); 

		HandleMovement (horizontal); 

		Flip (horizontal); 
	}


	private void HandleMovement(float horizontal)
	{
		    
		   
		myRigidbody.velocity = new Vector2(horizontal * movementSpeed ,myRigidbody.velocity.y); 

		anim.SetFloat ("speed", Mathf.Abs( horizontal)); 
		
	}


	private void Flip (float horizontal)
	{

		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 
		{

			facingRight = !facingRight; 

			Vector3 theScale = transform.localScale; 

			theScale.x *= -1; 

			transform.localScale = theScale; 

		}
	}


}
