using UnityEngine;
using System.Collections;

public class IntroScreen : MonoBehaviour {

	private GameManager gameManager;

	private Texture2D bg;
	public GUIStyle introTextStyle;

	private bool fadeOut;
	private float timer;
	private float alpha;
	public float fadeSpeed;

	// Use this for initialization
	void Start () {
		gameManager = GetComponent<GameManager>();

		bg = new Texture2D(1, 1);
		bg.SetPixel(0, 0, Color.black);
		bg.Apply();

		timer = Time.time;
		alpha = 1;
	}

	void OnGUI () {
		GUI.color = new Color(1, 1, 1, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);
		GUI.Label(new Rect(Screen.width/2 -250, Screen.height/2 -250, 500, 500), "Avoid the invisible monster", introTextStyle);
		GUI.color = new Color(1, 1, 1, 1);

		if (Input.anyKeyDown) {
			fadeOut = true;
			timer = Time.time;
		}

		if (fadeOut) {
			if (alpha > 0) alpha -= Time.deltaTime;
			if (alpha < 0) { 
				GUI.Label(new Rect(Screen.width/2 -250, Screen.height/2 -250, 500, 500), "RUN!", introTextStyle);
				gameManager.RunningIntro = false;
			}
			if (Time.time > timer +2) Destroy(this);
		}
	}
}
