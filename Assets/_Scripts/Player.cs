﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//how quickly the player moves
	public float moveSpeed=3;
	//how quickly the player camera rotates with mouse
	public float cameraSpeed=12;

	//change in player position and camera rotation
	private Vector3 moveVector, lookVector;
	//center of screen for camera rotation
	private Vector3 centerOfScreen;
	//how far the mouse must be from the center to rotate camera
	private float minRotateDistance;
	//reference to character controller 
	private CharacterController charCon;
	//reference to camera transform
	private Transform playerCam;


	//speed of gravity
	public float gravity = 9.8f;

	void Start () {
		//find camera and character controller
		charCon = GetComponent<CharacterController> ();
		playerCam = transform.Find ("Camera");
		centerOfScreen = new Vector3 (Screen.width, Screen.height, 0) / 2;
		minRotateDistance = (Screen.width>Screen.height?Screen.height:Screen.width)*0.375f;
	}
	
	void Update () {
		//update camera rotation
		float rotateSpeedFactor = Vector3.Distance (centerOfScreen, Input.mousePosition) / minRotateDistance;
		if (rotateSpeedFactor>1) {
			lookVector = (new Vector3 (-Input.mousePosition.y + Screen.height / 2, Input.mousePosition.x - Screen.width / 2, 0)).normalized * Time.deltaTime* Mathf.Pow(rotateSpeedFactor,2) * cameraSpeed;
		} else {
			lookVector=Vector3.zero;
		}

		playerCam.rotation = Quaternion.Euler (playerCam.rotation.eulerAngles + lookVector);
		//face player towards camera
		//normalize transform forward to not have y data
		transform.forward = new Vector3(playerCam.forward.x,0,playerCam.forward.z).normalized;

		
		//set camera rotation to current transform rotation
		playerCam.localRotation = Quaternion.Euler( new Vector3(playerCam.localRotation.eulerAngles.x,0,0));

		//move player in direction transform is facing
		moveVector = (Input.GetAxis ("Horizontal")*transform.right + Input.GetAxis ("Vertical")*transform.forward-transform.up*gravity) * moveSpeed *(Input.GetAxis("Sprint")!=0?3:1)* Time.deltaTime;
		charCon.Move (moveVector);
	}
}
