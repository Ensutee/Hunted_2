using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EventHandler : MonoBehaviour {
	
	public List<Trigger> eventsList;
	public List<int> eventQueue;
	public bool trigger;
    //public IntenGetter ig;

	// Use this for initialization
	void Start () {

		eventsList = new List<Trigger>();
		eventQueue = new List<int>();
	}


	
	// Update is called once per frame
	void Update () {

		//if (trigger) {
        //    if(ig != null) TriggerEvent(ig());
		//	trigger = false;
		//}

        //Player does something immidiately trigger event
        //Event tags?
        //Player location condition (light/no light)


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

	public void TriggerEvent (float intensityLevel, EventType type) {
		int level = Mathf.RoundToInt(Mathf.Lerp(0, System.Enum.GetValues(typeof(Trigger.triggerType)).Length-1, intensityLevel));
		eventQueue.Add(Mathf.Clamp(level +Random.Range(-1, 2), 0, System.Enum.GetValues(typeof(Trigger.triggerType)).Length-1));
	}

    public void DoEvent(bool monsterTime, bool firstPerson)
    {
        
    }

    const TriggerType[] preMonster = {}
}

//public enum EventType {Sound, Movement, Monster, Any}
//public delegate float IntenGetter();
