using UnityEngine;
using System.Collections;

public class CameraObjFader : MonoBehaviour {

	public Transform player;
	public float fadeDelay;
	public float fadeSpeed;
	public float clearRadiusWide;
	public float clearRadiusNarrow;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.GetComponent<PlayerInput>().IsLooking) {
			RaycastHit[] hitNarrow = Physics.SphereCastAll(transform.position, clearRadiusNarrow, (player.position -transform.position) +Vector3.down *clearRadiusNarrow);
			RaycastHit[] hitWide = Physics.SphereCastAll(transform.position, clearRadiusWide, (player.position -transform.position) +Vector3.up *clearRadiusWide);

			foreach (RaycastHit obj in hitWide) {
				if (obj.transform.GetComponent<ObjFade>() != null) {
					if (obj.point.y > player.position.y+3) {
						obj.transform.GetComponent<ObjFade>().FadeBackTimer = Time.time;
						obj.transform.GetComponent<ObjFade>().FadeBackDelay = fadeDelay;
						obj.transform.GetComponent<ObjFade>().FadeSpeed = fadeSpeed;
					}
				}
			}
			foreach (RaycastHit obj in hitNarrow) {
				if (obj.transform.GetComponent<ObjFade>() != null) {
					if (obj.point.z < player.position.z && obj.point.y > player.position.y) {
						obj.transform.GetComponent<ObjFade>().FadeBackTimer = Time.time;
						obj.transform.GetComponent<ObjFade>().FadeBackDelay = fadeDelay;
						obj.transform.GetComponent<ObjFade>().FadeSpeed = fadeSpeed;
					}
				}
			}
		}
	}
}
