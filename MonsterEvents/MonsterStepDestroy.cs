using UnityEngine;
using System.Collections;

public class MonsterStepDestroy : MonoBehaviour {

	private float timer;
	public float life;
	private float alpha;
	public float fadeDur;
	private GameObject foot;
	private Color footCol;

	// Use this for initialization
	void Start () {
		timer = Time.time;
		foot = transform.FindChild("Quad").gameObject;
		footCol = foot.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timer +life) Destroy(gameObject);
		if (Time.time > timer +life -fadeDur) footCol.a -= Time.deltaTime/fadeDur;
		foot.renderer.material.color = footCol;
	}
}
