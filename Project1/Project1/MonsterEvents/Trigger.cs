using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public enum triggerType { LIGHTS, LIGHTSOFF, BARRELS, MONSTER, SMOKE, GLASS, SHADOW, ROAR, SCRATCH }
	public triggerType trigger;

	private Transform player;
	private EventHandler eventHandler;
	private LightController lightController;

	private bool isRepeatable;
	private bool hasRun;
	public bool run;
	public bool runable;

	private float activationDistMin;
	private float activationDistMax;
	private float activationTimer;
	private float repeatDelay;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
		eventHandler = GameObject.Find("Game Manager").GetComponent<EventHandler>();

		hasRun = false;
		run = false;
		repeatDelay = 0;

		if (trigger == triggerType.MONSTER) {
			isRepeatable = true;
			repeatDelay = 30;
			activationDistMin = 10;
			activationDistMax = 20;
		} else if (trigger == triggerType.BARRELS) {
			isRepeatable = false;
			activationDistMin = 5;
			activationDistMax = 10;
		} else if (trigger == triggerType.LIGHTS) {
			lightController = gameObject.GetComponent<LightController>();
			isRepeatable = true;
			repeatDelay = 10;
			activationDistMin = 0;
			activationDistMax = 10;
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEventHandler();
		UpdateStatus();

		if (run) {
			if (trigger == triggerType.MONSTER) {
				MonsterSpawn();
			} else if (trigger == triggerType.BARRELS) {
				Crates();
			} else if (trigger == triggerType.LIGHTS) {
				Lights();
			}
		}
	}

	void UpdateEventHandler() {
		Vector3 distance = player.position -transform.position;
		if (distance.magnitude > activationDistMin && distance.magnitude < activationDistMax) {
			RaycastHit hit;
			Ray Charles = new Ray(transform.position, player.position -transform.position);
			Physics.Raycast(Charles, out hit);

			if (hit.collider.transform == player) {
				if (!eventHandler.eventsList.Contains(this)) eventHandler.eventsList.Add(this);
			} else {
				if (eventHandler.eventsList.Contains(this)) eventHandler.eventsList.Remove(this);
			}
		} else {
			if (eventHandler.eventsList.Contains(this)) eventHandler.eventsList.Remove(this);
		}
	}

	void UpdateStatus() {
		if(run) runable = false;
		else if (hasRun && !isRepeatable) runable = false;
		else runable = true;
	}

	void MonsterSpawn() {
		GameObject enemy = (GameObject)Instantiate(Resources.Load("Prefabs/Monster"));
		enemy.transform.position = transform.position;

		End();
	}

	void Crates() {
		for (int i = 0; i < transform.childCount; i++) {
			Vector3 playerDir = new Vector3(player.position.x +Random.Range(-15, 15), 0, player.position.z +Random.Range(-15, 15)) -transform.position;
			float force = Random.Range (150, 200);
			transform.GetChild(i).rigidbody.isKinematic = false;
			transform.GetChild(i).rigidbody.AddForce(playerDir.normalized *force, ForceMode.Impulse);
		}
		End();
	}

	void Lights() {
		if (!hasRun) {
			if (Time.time < activationTimer +repeatDelay) {
				lightController.flicker = true;
			} else {
				lightController.flicker = false;
				End();
			}
		} else {
			lightController.lightOn = false;
			isRepeatable = false;
			End();
		}
	}

	void End() {
		hasRun = true;
		run = false;
	}

	public void Activate() {
		if (!hasRun || (isRepeatable && Time.time > activationTimer +repeatDelay)) {
			activationTimer = Time.time;
			run = true;
		}
	}
}
