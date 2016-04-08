using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour
{

		public float horizontalSpeed;
		public float verticalSpeed;
		public float jumpStrength;
		public float moveX;
		public float moveZ;
		public float posX;
		public float posZ;
		
		public bool ground = false;
		bool dropping = true;
		float gravity;
		
		bool hitWall = false;
		
		void Start ()
		{
//				//maxY = 0;
//				posX = 0;
//				posZ = 0;
		
		}

		//void Movement(float h, float v)
		void Movement ()
		{
				
				posX = transform.position.x;
				posZ = transform.position.z;
			
				moveX = Input.GetAxisRaw ("Horizontal");
				moveZ = Input.GetAxisRaw ("Vertical");
				gravity = GetComponent<Rigidbody>().velocity.y;
				GetComponent<Rigidbody>().velocity = new Vector3 (moveX * horizontalSpeed, gravity, moveZ * verticalSpeed);
				

	}
	
	void FixedUpdate ()
		{
				Movement ();
		}
		
		void Jumping ()
		{
		
		Debug.Log ("You Jumped");
		
		//old jump method, not good for friction, might work in other situations..
		//rigidbody.AddForce (Vector3.up * jumpStrength); //Add force on y axis
		
		GetComponent<Rigidbody>().velocity += new Vector3(0, jumpStrength,0);
		}
		
		void Dropping ()
		{
				Debug.Log ("You are dropping");
		
				GetComponent<Rigidbody>().AddForce (Vector3.down * 20);
		}
				
		void Update ()
		{
				//Debug.Log (ground);
					
				if (Input.GetButtonDown ("Jump") && ground == true && GetComponent<Rigidbody>().velocity.y <= -0.0) {
						Jumping ();
						ground = false;
				} 

//				Will check the position of the object in Y axis
//				if (transform.position.x >= posX && transform.position.z >= posZ) {
//						posX = transform.position.x;
//						posZ = transform.position.z;
//				}

				
				if (!ground && GetComponent<Rigidbody>().velocity.y < -0.0001) {

						dropping = true;
						Dropping ();
							
				}

		}

		void OnCollisionEnter (Collision col)
		{
		if (col.gameObject.tag == "World" || col.gameObject.tag == "Platforms") {
						ground = true;
						dropping = false;
						Debug.Log ("You are on ground");
				}
				
				if(col.gameObject.tag == "Platforms")
				{
					hitWall = true;
				}
				
		}
		
		void OnCollisionStay (Collision other)
		{
			if(other.gameObject.tag == "World")
				ground = true;
				dropping = false;
		
		}
		
		void OnCollisionExit (Collision platforms)
		{
		
			if(platforms.gameObject.tag == "Platforms" && GetComponent<Rigidbody>().velocity.y < -0.1)
				hitWall = !hitWall;
				ground = false;
				dropping = true;
		}
		
	
	
	
				
				
		
			
				
		
}
