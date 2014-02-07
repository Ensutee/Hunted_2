using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {

	private CharacterController controller;
	public Transform characterModel;

	private float speedY = 0;

	public bool canMove;
	private bool isFalling;
	private Vector3 moveDir = Vector3.zero;
	public float speedMove;
	private float stepSize;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		stepSize = controller.stepOffset;
	}
	
	// Update is called once per frame
	void Update () {
		GroundCheck();
		ActionCheck();
		Gravity();
		UpdateMovement();
	}

	void GroundCheck() {
		if (controller.isGrounded) {
			controller.stepOffset = stepSize;
			speedY = 0;
			isFalling = false;
		}
		else {
			controller.stepOffset = 0;
			isFalling = true;
		}
	}
	
	void ActionCheck() {
	}

	void Gravity() {
		speedY -= (-1 *Physics.gravity.y) *Time.deltaTime;
		moveDir.y = speedY;
	}

	void UpdateMovement() {
		controller.Move(moveDir);
		if(controller.isGrounded) moveDir = Vector3.zero;
		Vector3 rot = new Vector3(0, characterModel.rotation.eulerAngles.y, 0);
		characterModel.rotation = Quaternion.Euler(rot);
	}

	public void Move(Vector3 dir) {
		if (canMove && !isFalling) {
			ActionMove(dir);
		}
	}

	void ActionMove(Vector3 dir) {
		moveDir = dir *speedMove *Time.deltaTime;
		characterModel.LookAt(characterModel.position +moveDir.normalized);
	}
}
