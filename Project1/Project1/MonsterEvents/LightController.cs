using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	public bool lightOn;
	public bool flicker;
	public float flickerFreq;
	public float flickerRange;
	public float flickDurMax;
	private float flickerDelay;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (flicker && lightOn) {
			if (Time.time > flickerDelay) {
				light.enabled = !light.enabled;
				if (light.enabled) {
					flickerDelay += flickerFreq +Random.Range(-flickerRange, flickerRange);
				} else {
					flickerDelay += Random.Range(0, flickDurMax);
				}
			}

		} else {
			if (lightOn) light.enabled = true;
			else light.enabled = false;
			flickerDelay = Time.time;
		}
	}
}
