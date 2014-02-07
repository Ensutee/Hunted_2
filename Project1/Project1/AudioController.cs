using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	private PlayerInput playerInput;
	private CharacterMotor charMot;

	//wind
	private AudioSource windSource;
	public AudioClip wind;
	public float windVolume;

	//Tinnitus
	private AudioSource tinnitusSource;
	public AudioClip tinnitus;
	public float tinnitusVolume;

	//Steps
	private AudioSource stepSourceL;
	private AudioSource stepSourceR;
	public float stepVolume;
	private float stepDur;
	private float stepTimer;
	private bool stepL;
	private enum surfaceTypes {GRAVEL, STAIRS, INDOORS}
	private surfaceTypes surface;
	private string stepClipName;

	//Tinnitus fade stuff
	public float tinnitusDelay;
	public float tinnitusFadeInDur;
	public float tinnitusFadeOutDur;
	private float tinnitusTimer;

	//volume adjusters
	private float generalVolumeAdj = 1;
	private float stepVolumeAdj = 1;
	private float windVolumeAdj = 1;

	// Use this for initialization
	void Start () {
		playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
		charMot = GameObject.Find("Player").GetComponent<CharacterMotor>();

		//wind setup
		windSource = gameObject.AddComponent<AudioSource>();
		windSource.loop = true;
		windSource.volume = windVolume;
		windSource.clip = wind;
		windSource.Play();

		//tinnitus setup
		tinnitusSource = gameObject.AddComponent<AudioSource>();
		tinnitusSource.loop = true;
		tinnitusSource.volume = 0;
		tinnitusSource.clip = tinnitus;

		//footsteps setup
		stepSourceL = gameObject.AddComponent<AudioSource>();
		stepSourceL.loop = false;
		stepSourceL.volume = stepVolume;
		stepSourceL.clip = Resources.Load("Audio/Step_Gravel_1") as AudioClip;

		stepSourceR = gameObject.AddComponent<AudioSource>();
		stepSourceR.loop = false;
		stepSourceR.volume = stepVolume;
		stepSourceR.clip = Resources.Load("Audio/Step_Gravel_2") as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		DetectSurface();
		AdjustWind();
		FootSteps();
		Tinnitus();
		if (!charMot.canMove) tinnitusTimer = Time.time;

		UpdateVolume();

		//manage timers
		if (playerInput.MovDir != Vector3.zero) tinnitusTimer = Time.time;
	}

	void UpdateVolume () {
		windSource.volume = windVolume *windVolumeAdj *generalVolumeAdj;
		stepSourceL.volume = stepVolume *stepVolumeAdj *generalVolumeAdj;
		stepSourceR.volume = stepVolume *stepVolumeAdj *generalVolumeAdj;
	}

	void DetectSurface () {
		Ray Charles = new Ray(GameObject.Find("Player").transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(Charles, out hit, 2)) {
			if (hit.collider.gameObject.name == "Stairs" || hit.collider.gameObject.name == "Platform") surface = surfaceTypes.STAIRS; 
			else if (hit.collider.gameObject.name == "Ground") surface = surfaceTypes.GRAVEL;
			else if (hit.collider.gameObject.name == "Floor") surface = surfaceTypes.INDOORS;
		}
	}

	void AdjustWind() {
		if (surface == surfaceTypes.INDOORS) {
			if (windVolumeAdj > 0.5f) windVolumeAdj -= Time.deltaTime;
		} else {
			if (windVolumeAdj < 1) windVolumeAdj += Time.deltaTime;
		}
	}

	void FootSteps() {
		if (charMot.canMove) {
			//set the current path depending on surface
			if (surface == surfaceTypes.GRAVEL) stepClipName = "Audio/Step_Gravel_";
			else if (surface == surfaceTypes.STAIRS) stepClipName = "Audio/Step_MetalStair_";
			else if (surface == surfaceTypes.INDOORS) stepClipName = "Audio/Step_Indoors_";

			stepDur = Mathf.Lerp(0.3f, 1f, 1 -playerInput.MovDir.magnitude);

			if (playerInput.MovDir != Vector3.zero) {
				//if the player is moving fade in step sounds
				if (stepSourceL.volume < stepVolume) {
					stepVolumeAdj += Time.deltaTime *4;
				}
				if (Time.time > stepTimer +stepDur) {
					if (stepL) {
						//play step R sound
						if (!stepSourceR.isPlaying) StepPlay(stepSourceR);
					} else {
						//play step L sound
						if (!stepSourceL.isPlaying) StepPlay(stepSourceL);
					}
				}

			} else {
				//if the player is not moving fade out step sounds
				if (stepSourceL.volume > 0) {
					stepVolumeAdj -= Time.deltaTime *4;
				}
				stepTimer = Time.time;
			}
		}
	}

	void StepPlay (AudioSource source) {
		source.clip = Resources.Load(stepClipName +Random.Range(1,4).ToString()) as AudioClip;
		source.Play();
		stepTimer = Time.time;
		stepL = !stepL;
	}

	void Tinnitus () {
		if (Time.time > tinnitusTimer +tinnitusDelay) {
			//start playing tinnitus
			if (!tinnitusSource.isPlaying) tinnitusSource.Play();

			//crossfade with all other sounds
			if (generalVolumeAdj > 0) {
				generalVolumeAdj -= Time.deltaTime /tinnitusFadeInDur;
				tinnitusSource.volume = tinnitusVolume *(1 -generalVolumeAdj);
			}

		} else {
			//crossfade out with all other sounds
			if (generalVolumeAdj < 1) {
				generalVolumeAdj += Time.deltaTime /tinnitusFadeOutDur;
				tinnitusSource.volume = tinnitusVolume *(1 -generalVolumeAdj);
			}

			//stop playing tinnitus
			if (tinnitusSource.volume < 0 && tinnitusSource.isPlaying) tinnitusSource.Stop();
		}
	}
}
