using UnityEngine;
using System.Collections;

// -----------------------
// Add to the moveable object which is controlled by the player.
// -----------------------

public class PlayerMovement : MonoBehaviour {

	// Player movement. Attach it to the player object.

	public float sprintMultiplier = 2;
	public Vector3 oldPos = new Vector3 (0,0,0);
	public float speed = 5f;
	public float jumpStrength = 5f;


	private Vector3 playerBoundary;
	private float allowedFallSpeed = -0.04f;
	private Rigidbody rb;
	private MouseLook ml;
	private Vector3 moveVector;
	private float speedOriginal = 5f;
	private Vector3 startRot;
	private float distToGround;
	private Collider col;

	void Start () {
		startRot = new Vector3(0.0f, 0.0f, 0.0f);
		transform.rotation = Quaternion.Euler(startRot);
		ml = GameObject.Find("Eyes").GetComponent<MouseLook>();
		rb = GetComponent<Rigidbody>();
		speed = speedOriginal;
		col = GetComponent<CapsuleCollider> ();
		distToGround = col.bounds.extents.y;
	}

	void Update () {

		//if (Input.GetKeyDown (KeyCode.Escape))
		//	Application.Quit ();

		controlFall ();
		sprintButton ();
		JumpFeature ();

		//Rotating with the camera
		rb.transform.rotation = Quaternion.Euler(0f, ml.CurrentYRotation, 0f);

		//Walking the direction, of the camera
		moveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		rb.transform.Translate(moveVector * speed * Time.deltaTime);

	}

	public float GetPlayerSpeed()
	{
		return speed;
	}

	public void SetPlayerSpeed(float x)
	{
		speedOriginal = x;
	}

	void controlFall()
	{
		float yDif = transform.position.y - oldPos.y;

		if (yDif < allowedFallSpeed) {
			speed = speedOriginal * 0.5f;
		} 
		else {
			speed = speedOriginal;
		}
		oldPos = transform.position;
	}
	void sprintButton()
	{
		if (Input.GetKey (KeyCode.LeftShift))
			speed = speedOriginal * sprintMultiplier;
	}

	void JumpFeature()
	{
		if (Input.GetKey (KeyCode.Space) && IsGrounded()) {

			GetComponent<Rigidbody> ().velocity = new Vector3(0, jumpStrength, 0);
		}
		
	}

	bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

}