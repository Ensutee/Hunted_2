using UnityEngine;
using System.Collections;

public class PushObj : MonoBehaviour {

	public PlayerInput playerInput;
	public float pushPower;

	void Start() {
		playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;

		if (body == null || body.isKinematic) {
			return;
		}

		Vector3 normalInverse = Vector3.zero -hit.normal;
		Vector3 movDir = playerInput.MovDir;

		Vector3 force = (normalInverse +movDir).normalized *pushPower;

		body.AddForceAtPosition(force, hit.point);
	}
}