using UnityEngine;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour {
	
	public List<Trigger> eventsList;
	public List<int> eventQueue;
	public bool trigger;
	public float intensity;

	// Use this for initialization
	void Start () {
		eventsList = new List<Trigger>();
		eventQueue = new List<int>();
	}
	
	// Update is called once per frame
	void Update () {
		if (trigger) {
			TriggerEvent(intensity);
			trigger = false;
		}

		if (eventQueue.Count > 0) {
			for(int i = eventQueue.Count -1; i >= 0; i--) {
				if (eventsList.Count != 0) {
					for(int j = eventsList.Count -1; i >= 0; i--) {
						if ((int)eventsList[j].trigger == eventQueue[i] && eventsList[j].runable) {
							eventsList[j].Activate();
							eventsList.RemoveAt(j);
							eventQueue.RemoveAt(i);
							break;
						}
					}
				}
			}
		}
	}

	public void TriggerEvent (float intensityLevel) {
		int level = Mathf.RoundToInt(Mathf.Lerp(0, System.Enum.GetValues(typeof(Trigger.triggerType)).Length-1, intensityLevel));
		eventQueue.Add(Mathf.Clamp(level +Random.Range(-1, 2), 0, System.Enum.GetValues(typeof(Trigger.triggerType)).Length-1));
	}
}
