using UnityEngine;
using System.Collections.Generic;

public class Monster : MonoBehaviour {

	private GameObject player;
	private float destroyTimer;
	private float creationTimer;
	private bool speedUp;

	private NavMeshAgent agent;
	public float stepDur;
	private bool stepL;
	private float stepTimer;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player");
		stepTimer = Time.time;
		destroyTimer = Time.time;
		creationTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > stepTimer +stepDur) {
			if (stepL) {
				GameObject footStep = (GameObject)Instantiate(Resources.Load("Prefabs/MonsterStep"));
				footStep.transform.position = transform.position +transform.TransformDirection(Vector3.left) *0.5f;
				footStep.transform.rotation = transform.rotation;
			} else {
				GameObject footStep = (GameObject)Instantiate(Resources.Load("Prefabs/MonsterStep"));
				footStep.transform.position = transform.position +transform.TransformDirection(Vector3.right) *0.5f;
				footStep.transform.rotation = transform.rotation;
			}
			stepTimer = Time.time;
			stepL = !stepL;
		}

		//set step duration to a function of the speed
		stepDur = 1.75f/agent.speed;

		//chase the player and roar occatianally
		agent.SetDestination(player.transform.position);

		if (Time.time > creationTimer +15) {
			if (!speedUp) GetComponent<AudioSource>().Play ();
			speedUp = true;
		}

		if (speedUp) agent.speed += Time.deltaTime;

		//if LoS'ed destroy
		RaycastHit hit;
		Physics.Raycast(transform.position, player.transform.position -transform.position, out hit);
		if (hit.collider.gameObject == player) destroyTimer = Time.time;
		else if (Time.time > destroyTimer +5) GetComponent<AudioSource>().volume -= Time.deltaTime;
		if (GetComponent<AudioSource>().volume <= 0) Destroy(gameObject);
	}
}
