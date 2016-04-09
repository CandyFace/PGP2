using UnityEngine;
using System.Collections;

// -----------------------
// Add to the camera object.
// -----------------------

public class MouseLook : MonoBehaviour {

	// Attach this to the MainCamera

	private float lookSensitivity = 35f;
	private float yRotation;
	private float xRotation;
	private float yRotationV;
	private float xRotationV;
	private float lookSmoothDamp = 0.1f;
	// Needed to move player currently
	private float currentYRotation;
	private float currentXRotation;
	private bool isUsingMouse;

	void Start()
	{
		currentXRotation = transform.rotation.x;
		currentYRotation = transform.rotation.y;
	}

	void Update () {

		// Getting inputs from mouse, and storing the values
		yRotation += Input.GetAxis("Mouse X");
		xRotation -= Input.GetAxis("Mouse Y");
	

		yRotation += Input.GetAxis("PS4_RightAnalogHorizontal") * lookSensitivity;
		//xRotation += Input.GetAxis("PS4_RightAnalogVertical") * lookSensitivity;

		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // let the player have a 180 degree vertical view

		currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
		currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

		this.transform.rotation = Quaternion.Euler(this.transform.rotation.x + currentXRotation, this.transform.rotation.y + currentYRotation, this.transform.rotation.z);
	}

	public float CurrentXRotation
	{
		get { return currentXRotation; }
	}

	public float CurrentYRotation
	{
		get { return currentYRotation; }
	}
}