using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//how quickly the player moves
	public float moveSpeed=8;
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


	//references to the help text
	private GameObject smallHelp,fullHelp;
	void Start () {
		//find camera and character controller
		charCon = GetComponent<CharacterController> ();
		playerCam = transform.Find ("Camera");
		centerOfScreen = new Vector3 (Screen.width, Screen.height, 0) / 2;
		minRotateDistance = (Screen.width > Screen.height ? Screen.height : Screen.width) * 0.375f;
	}

	public void Awake(){
		smallHelp = GameObject.Find ("Small Help Text").gameObject;
		fullHelp = GameObject.Find ("Full Help Text").gameObject;
		fullHelp.SetActive (false);
	}
	
	void Update () {
		//update camera rotation
		float rotateSpeedFactor = Vector3.Distance (centerOfScreen, Input.mousePosition) / minRotateDistance;
		if (rotateSpeedFactor>1) {
			lookVector = (new Vector3 (-Input.mousePosition.y + Screen.height / 2, Input.mousePosition.x - Screen.width / 2, 0)).normalized * Time.deltaTime* Mathf.Pow(rotateSpeedFactor,2) * cameraSpeed;
		} else {
			lookVector=Vector3.zero;
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			fullHelp.SetActive(!fullHelp.activeSelf);
			smallHelp.SetActive(!smallHelp.activeSelf);
		}

		//playerCam.rotation = Quaternion.Euler (playerCam.rotation.eulerAngles + lookVector);
		playerCam.rotation *= Quaternion.Euler (lookVector);
		//face player towards camera
		//normalize transform forward to not have y data
		transform.forward = playerCam.forward;

		
		//set camera rotation to current transform rotation
		playerCam.localRotation = Quaternion.identity;

		//move player in direction transform is facing
		moveVector = (Input.GetAxis ("Horizontal")*transform.right + Input.GetAxis ("Vertical")*transform.forward+Vector3.up*(Input.GetAxis("Jump"))) * moveSpeed *(Input.GetAxis("Sprint")!=0?3:1)* Time.deltaTime;
		charCon.Move (moveVector);
	}
}
