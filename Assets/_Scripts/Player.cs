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
		lookVector = Vector3.zero;
		//find camera and character controller
		charCon = GetComponent<CharacterController> ();
		playerCam = transform.Find ("Camera");
		centerOfScreen = new Vector3 (Screen.width, Screen.height, 0) / 2;
		minRotateDistance = (Screen.width > Screen.height ? Screen.height : Screen.width) * 0.375f;
		smallHelp.SetActive (false);
		//print (playerCam.name);
	}

	public void Awake(){
		fullHelp = GameObject.Find("Full Help Text").gameObject;
		smallHelp = GameObject.Find ("Small_Help_Text").gameObject;//GameObject.FindGameObjectWithTag ("SmallHelp");
	}
	
	void Update () {
		//handle pausing and resuming the simulation
		if (Input.GetKeyDown (KeyCode.H)) {
			fullHelp.SetActive(!fullHelp.activeSelf);
			smallHelp.SetActive(!smallHelp.activeSelf);
		}

		//auto pause simulation if the mouse is off the screen
		if(Mathf.Abs((centerOfScreen-Input.mousePosition).x)*1.875>Screen.width||Mathf.Abs((centerOfScreen-Input.mousePosition).y)*1.875>Screen.height){
			smallHelp.SetActive (false);
			fullHelp.SetActive (true);
		}

		//if the help text is not visible
		if(smallHelp.activeSelf){
		//update camera rotation
		float rotateSpeedFactor = Vector3.Distance (centerOfScreen, Input.mousePosition) / minRotateDistance;
		if (rotateSpeedFactor>1) {
				lookVector = (new Vector3 (-2*Input.mousePosition.y/Screen.height + 1, 2*Input.mousePosition.x/Screen.width - 1, 0)).normalized * Time.deltaTime* Mathf.Pow(rotateSpeedFactor,2) * cameraSpeed;
		} else {
			lookVector=Vector3.zero;
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
}
