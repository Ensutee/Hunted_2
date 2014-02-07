using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private EventHandler eventHand;
	private CharacterMotor charMot;
	private PlayerInput playerInput;
	private CameraPosition camPos;

	public float intensity;
	private float triggerTimer;
	private float triggerDelay;
	public float triggerAVgDelay;

	private bool runningIntro;
	public bool RunningIntro { set { runningIntro = value; } }

	// Use this for initialization
	void Start () {
		eventHand = GetComponent<EventHandler>();
		charMot =  GameObject.Find("Player").GetComponent<CharacterMotor>();
		playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
		camPos = GameObject.Find("Main Camera").GetComponent<CameraPosition>();
		charMot.canMove = false;
		camPos.canLook = false;

		triggerDelay = triggerAVgDelay +Random.Range(-5,5);

		runningIntro = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!runningIntro) {
			charMot.canMove = true;
			camPos.canLook = true;
		}

		if (!runningIntro) {
			if (Time.time > triggerTimer +triggerDelay) {
				eventHand.TriggerEvent(intensity);
				triggerTimer = Time.time;
				triggerDelay = triggerAVgDelay +Random.Range(-triggerAVgDelay/2,triggerAVgDelay/2);
			}
		}
	}
}
