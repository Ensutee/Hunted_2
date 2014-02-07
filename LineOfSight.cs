using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {

	private RenderTexture view;
	private RenderTexture lightMap;
	private RenderTexture final;
	public Material matLos;
	public enum viewModes {VIEW, NOFOG, LIGHTMAP}
	public viewModes viewMode;

	void Start() {
		view = new RenderTexture(Screen.width, Screen.height, 24);
		lightMap = new RenderTexture(Screen.width, Screen.height, 24);
		final = new RenderTexture(Screen.width, Screen.height, 24);

		matLos.SetTexture("_LightTex", lightMap);
	}

	void Update() {
		camera.targetTexture = lightMap;
		RenderTexture.active = lightMap;
		camera.cullingMask = (1 << LayerMask.NameToLayer("LightMap"));
		camera.Render();
	
		camera.targetTexture = view;
		RenderTexture.active = view;
		camera.cullingMask = -1;
		camera.cullingMask ^= (1 << LayerMask.NameToLayer("LightMap"));
		camera.cullingMask ^= (1 << LayerMask.NameToLayer("Triggers"));
		camera.Render();

		Graphics.Blit(view, final, matLos, -1);
	}

	void OnGUI () {
		if (viewMode == viewModes.VIEW) {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), final);
			GUI.Label(new Rect(0, 0, 150, 25), "Final view (Pro only)");
		} else if (viewMode == viewModes.NOFOG) {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), view);
			GUI.Label(new Rect(0, 0, 150, 25), "No LoS view (Pro only)");
		} else if (viewMode == viewModes.LIGHTMAP) {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), lightMap);
			GUI.Label(new Rect(0, 0, 150, 25), "Light map view (Pro only)");
		}
	}
}