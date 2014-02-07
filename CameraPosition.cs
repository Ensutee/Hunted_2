using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {

	private LineOfSight los;
	private GameObject player;
	public bool canLook;

	public float lookSensitivity;
	public float degreeLimitY;
	public Transform positionTarget;
	public Transform lookTarget;
	public Transform cameraPosTop;
	public Transform cameraPosFPS;
	private float lookX;
	private float lookY;

	// Use this for initialization
	void Start () {
		los = GetComponent<LineOfSight>();
		player = GameObject.Find("Player");
		positionTarget = cameraPosTop;
		lookTarget = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.GetComponent<PlayerInput>().IsLooking) {
			lookX = 0;
			lookY = 0;
			transform.LookAt(lookTarget);
			GetComponent<Light>().enabled = false;
		}

		UpdatePosition();
	}

	void UpdatePosition() {
		transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime *10);
	}

	public void LookAround (float x, float y) {
		if (canLook) {
			if (x != 0) lookY += x *lookSensitivity *Time.deltaTime;
			if (y != 0) lookX += y *lookSensitivity *Time.deltaTime;
			if (lookX > degreeLimitY) lookX = degreeLimitY;
			else if (lookX < -degreeLimitY) lookX = -degreeLimitY;

			Vector3 lookAt = new Vector3(cameraPosFPS.rotation.eulerAngles.x +lookX, cameraPosFPS.rotation.eulerAngles.y +lookY, 0);
			if (transform.position.y < cameraPosFPS.position.y +0.5f) {
				GetComponent<Light>().enabled = true;
				los.viewMode = LineOfSight.viewModes.NOFOG;
			 	transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lookAt), Time.deltaTime *10);
			}
		}
	}
}
