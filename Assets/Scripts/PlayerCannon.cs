using UnityEngine;
using System.Collections;

public class PlayerCannon : MonoBehaviour {

	public float minAngle = 0f;
	public float maxAngle = 180f;

	public float shootingWobbling = 5f;

	private Quaternion curQuat = new Quaternion(0f, 0f, 1f, -1.5f);
	// Use this for initialization
	void Start () {
		transform.localRotation = curQuat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
