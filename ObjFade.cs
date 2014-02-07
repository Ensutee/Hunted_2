using UnityEngine;
using System.Collections.Generic;

public class ObjFade : MonoBehaviour {

	private bool fade = false;
	private bool hidden = false;
	public bool Fade { set { fade = value; } }
	private float fadeBackTimer =-1;
	public float FadeBackTimer { set { fadeBackTimer = value; } }
	private float fadeBackDelay;
	public float FadeBackDelay { set { fadeBackDelay = value; } }
	private float fadeSpeed;
	public float FadeSpeed { set { fadeSpeed = value; } }
	private float fadeAlpha = 1;
	private Component[] childrenRenderer;

	private Shader diffuse;
	private Shader standartHidden;
	private Shader lightMapHidden;

	// Use this for initialization
	void Start () {
		diffuse = Shader.Find("Diffuse");
		standartHidden = Shader.Find("Custom/Transparent Shadowcaster");
		lightMapHidden = Shader.Find("Custom/Transparent Shadowcaster");

		if (transform.parent == null) childrenRenderer = GetComponentsInChildren(typeof(Renderer));
		else childrenRenderer = transform.parent.GetComponentsInChildren(typeof(Renderer));
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time < fadeBackTimer +fadeBackDelay) fade = true;
		else fade = false;

		Hide();
	}

	void Hide () {
		if (fade) {
			//set material for gameobject and children
			if (!hidden) {
				if (GetComponent<Renderer>() != null) renderer.material.shader = standartHidden;
				foreach (Renderer child in childrenRenderer) {
					if (child.gameObject.layer != LayerMask.NameToLayer("LightMap")) child.material.shader = standartHidden;
					else if (child.gameObject.layer == LayerMask.NameToLayer("LightMap")) child.material.shader = lightMapHidden;
				}
				hidden = true;
			}

			//fade out and set alpha
			if (fadeAlpha > 0) {
				fadeAlpha -= fadeSpeed *Time.deltaTime;
				Color col;

				foreach (Renderer child in childrenRenderer) {
					col = child.material.color;
					child.material.color = new Color(col.r, col.g, col.b, fadeAlpha);
				}
				if (GetComponent<Renderer>() != null) {
					col = renderer.material.color;
					renderer.material.color = new Color(col.r, col.g, col.b, fadeAlpha);
				}
			}

		} else {
			//fade in and apply alpha
			if (fadeAlpha < 1) {
				fadeAlpha += fadeSpeed *Time.deltaTime;
				Color col;
				foreach (Renderer child in childrenRenderer) {
					col = child.material.color;
					child.material.color = new Color(col.r, col.g, col.b, fadeAlpha);
				}
				if (GetComponent<Renderer>() != null) {
					col = renderer.material.color;
					renderer.material.color = new Color(col.r, col.g, col.b, fadeAlpha);
				}

			} else if (hidden) {
				//set material for gameobject and children back
				if (GetComponent<Renderer>() != null) renderer.material.shader = diffuse;
				foreach (Renderer child in childrenRenderer) {
					if (child.gameObject.layer != LayerMask.NameToLayer("LightMap")) child.material.shader = diffuse;
					else if (child.gameObject.layer == LayerMask.NameToLayer("LightMap")) child.material.shader = diffuse;
				}
				hidden = false;
			}
		}
	}
}
