﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// XGL S17 Wk10

// This has been adjusted to have a faster rotate speed,
// as well as use raycasting in order to make the camera not get stuck behind walls.

public class XGL_CamFollow10 : MonoBehaviour {


	public Transform playerTransform;
	// How far the camera always is from the player. You can adjust this for a different
	// initial angle.
	Vector3 offset;
	Vector3 targetPosition;
	float targetYRotation = 0f;

	// Camera's rotation around the player
	float yRotation = 0f;
	public float rotateSpeed = 400f;


	// Use this for initialization, this code runs once when the attached GameObject is made.
	void Start () {

		// Hides the cursor
		Cursor.lockState = CursorLockMode.Locked;

		offset = new Vector3 (0, 4.5f, -14);

		// You can also use this to assign the playerTransform, BUT there must be a gameobject in the
		// current scene's hierarchy named HoverCube or you will get a null value
		//playerTransform = GameObject.Find ("HoverCube").GetComponent <Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Gives me a value of how far the mouse has moved since last frame
		float mouseX = Input.GetAxis ("Mouse X");

		// ~note~ mouse screen axes distances work like 2D games
		// Positive mouse x movement = negative y rotation.


		// Don't rotate unless mouse moving fast enough.
		if (Mathf.Abs (mouseX) > 0.1f) {
			// Keep mouseX between -1 and 1 so camera doesnt rotate too fast
			mouseX = Mathf.Clamp (mouseX,-2,2);
			targetYRotation += rotateSpeed * mouseX * Time.deltaTime;
		}

		// Keep rotation values between 0 and 360f
		if (targetYRotation > 360f) {
			targetYRotation -= 360f;
		} else if (targetYRotation < 0f) {
			targetYRotation += 360f;
		}

		// Smoothly linerally interpolate the current value of yRotation to targetYRotation.
		// Remove this for a more instant movement of the camera angle.

		// If the 3rd parameter, t, is 0, then LerpAngle returns yRotation.
		// If the 3rd parameter is 1, then LerpAngle returns targetYRotation
		// The formula is (roughly) RETURN_VALUE = t * (targetYRotation - yRotation) + yRotation

		//yRotation = targetYRotation;
		yRotation = Mathf.LerpAngle(yRotation, targetYRotation, 5f * Time.deltaTime);

		// Multiply this by a vector to rotate that vector by yRotation degrees about the world's y Axis
		Quaternion qRot = Quaternion.Euler (0, yRotation, 0);

		// Move the camera to the defualt distance from the target
		transform.position = playerTransform.position + (qRot*offset);

		// Rotate the transform of the camera to view the player
		transform.LookAt (playerTransform);

		// If you uncomment this, it draws a ray in the Scene View for debugging
		//Debug.DrawRay (playerTransform.position,transform.position-playerTransform.position,Color.red);


		// Stores information about the raycast we are about to perform
		RaycastHit hit = new RaycastHit();


		// Origin at the player, points towards the camera
		Ray ray = new Ray (playerTransform.position, transform.position - playerTransform.position);

		// If the raycast hits, move the camera to where the ray hit some collider
		// << is the bit shifting operator (e.g., 1<<2 means that 1 in binary becomes 100)
		if (Physics.Raycast (ray,out hit,offset.magnitude,1<<0)) {
			transform.position = hit.point;
		}

		// Bitshifting example
		// 11b = 3 (base 10)
		// 11b << 1 = 110b
		// 3 << 1 = 6


	}
}
