using System.Collections;
using UnityEngine;
namespace walkAnim{
public class WalkScript : MonoBehaviour {

		Animator Anim; 
		SpriteRenderer SpriteBoi; 


	void Start () {
		
			Anim = GetComponent<Animator> (); 

			SpriteBoi = GetComponent<SpriteRenderer> (); 

	}
	
	
	void Update () 
		{

			Movement ();
			{

				float move = Input.GetAxis ("Horizontal"); 
				Anim.SetFloat ("Speed", move); 

			}

		
	}


		void Movement()
		{
			if (Input.GetKey (KeyCode.D)) {

				SpriteBoi.flipX = false; 
				MovementRight (); 
		

			}

			if (Input.GetKey (KeyCode.A)) {

				SpriteBoi.flipX = true; 
				MovementRight (); 



			}
		}


			void MovementRight () 
			{

				transform.Translate (Vector2.right * 3f * Time.deltaTime);
				transform.eulerAngles = new Vector2 (0, 0); 
				

			}





		}

}
