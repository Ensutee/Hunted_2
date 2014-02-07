using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private CharacterMotor charMot;
	private LineOfSight lineOfSight;
	private CameraPosition cameraPos;

	private Vector3 movDir;
	public Vector3 MovDir { get { return movDir; } }
	private bool isLooking;
	public bool IsLooking { get { return isLooking; } }

	// Use this for initialization
	void Start () {
		charMot = GetComponent<CharacterMotor>();
		lineOfSight = GameObject.Find("Main Camera").GetComponent<LineOfSight>();
		cameraPos = GameObject.Find("Main Camera").GetComponent<CameraPosition>();
	}
	
	// Update is called once per frame
	void Update () {
		InputHandling();
	}

	void InputHandling() {
		if (charMot != null && charMot.enabled != false) 			GroundMov();
		if (cameraPos != null && cameraPos.enabled != false) 		CameraControls();
		if (charMot != null && charMot.enabled != false) 			Actions();
		if (lineOfSight != null && lineOfSight.enabled != false)	DeBugCommands();
	}
	
	void GroundMov() {
		if (!isLooking) {
			float x = 0;
			float z = 0;

			#region temp alt controls
			if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
			if (Input.GetKey(KeyCode.RightArrow)) x = 1;
			if (Input.GetKey(KeyCode.UpArrow)) z = -1;
			if (Input.GetKey(KeyCode.DownArrow)) z = 1;
			#endregion

			if (Input.GetAxis("Horizontal") != 0) x = Input.GetAxis("Horizontal");
			if (Input.GetAxis("Vertical") != 0) z = Input.GetAxis("Vertical");
			movDir = new Vector3(x, 0, -z);
			if (movDir != Vector3.zero) charMot.Move(movDir);
		} else {
			movDir = Vector3.zero;
		}
	}

	void CameraControls() {				  //TEMP KEY!
		if (cameraPos.canLook) {
			if (Input.GetButton("Action1") || Input.GetKey(KeyCode.X)) {
				float x = Input.GetAxis("Horizontal");
				float y = Input.GetAxis("Vertical");
				if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
				if (Input.GetKey(KeyCode.RightArrow)) x = 1;
				if (Input.GetKey(KeyCode.UpArrow)) y = -1;
				if (Input.GetKey(KeyCode.DownArrow)) y = 1;

				isLooking = true;
				cameraPos.LookAround(x, y);
				cameraPos.positionTarget = cameraPos.cameraPosFPS;
			} else if (Input.GetButtonUp("Action1") || Input.GetKeyUp(KeyCode.X)) {
				cameraPos.lookTarget = transform;
				cameraPos.positionTarget = cameraPos.cameraPosTop;
				lineOfSight.viewMode = LineOfSight.viewModes.VIEW;
				isLooking = false;
			}
		}
	}

	void Actions() {

	}

	void DeBugCommands() {
		if (Input.GetKeyDown(KeyCode.F1)) lineOfSight.viewMode = LineOfSight.viewModes.VIEW;
		if (Input.GetKeyDown(KeyCode.F2)) lineOfSight.viewMode = LineOfSight.viewModes.NOFOG;
		if (Input.GetKeyDown(KeyCode.F3)) lineOfSight.viewMode = LineOfSight.viewModes.LIGHTMAP;
	}
}
