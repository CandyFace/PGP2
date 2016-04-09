using UnityEngine;
using System.Collections;

// -----------------------
// Add to the moveable object which is controlled by the player.
// -----------------------

public class PlayerMovement : MonoBehaviour {

	// Player movement. Attach it to the player object.

	//	public Vector3 velocityShow;
	//	public float yDifShow;

	public float sprintMultiplier = 2;

	public Vector3 oldPos = new Vector3 (0,0,0);
	public float speedOriginal = 5f;
	public float speed = 5f;
	private float allowedFallSpeed = -0.04f;
	private Rigidbody rb;
	private MouseLook ml;
	private Vector3 moveVector;

	// ---- Start Menu Variables ----
	public float xStartRotation = 275.0f; // Change this Value in the inspector while testing.

	private float threshold = 0.7f;
	private Vector3 startRot;

	// ------------------------------

	void Start () {
		this.gameObject.layer = 2;
		startRot = new Vector3(xStartRotation, 0.0f, 0.0f);
		transform.rotation = Quaternion.Euler(startRot);
		ml = GameObject.Find("Eyes").GetComponent<MouseLook>();
		rb = GetComponent<Rigidbody>();
		speed = speedOriginal;
	}

	void Update () {

		//if (Input.GetKeyDown (KeyCode.Escape))
		//	Application.Quit ();

		controlFall ();
		sprintButton ();

		//Rotating with the camera
		rb.transform.rotation = Quaternion.Euler(0f, ml.CurrentYRotation, 0f);

		//Walking the direction, of the camera
		moveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		rb.transform.Translate(moveVector * speed * Time.deltaTime);


		//--------- START MENU SETTINGS --------
		// Push forward to get up.

		//		velocityShow = GetComponent<Rigidbody> ().velocity;
		//-------------------------------------
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
		if (Input.GetKey (KeyCode.Space))
			speed = speedOriginal * sprintMultiplier;
	}

}