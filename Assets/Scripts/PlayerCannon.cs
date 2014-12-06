using UnityEngine;
using System.Collections.Generic;

public class PlayerCannon : MonoBehaviour {

	[Range(0f, 0.5f)]
	public float flexAngle = 0.5f;

	[Range(0f, 0.1f)]
	public float shootingWobbling = 0.05f;

	[Range(0f, 0.5f)]
	public float rotationSpeed = 0.1f;

	private Quaternion curQuat = new Quaternion(0f, 0f, 1f, -1f);

	[SerializeField]
	[HideInInspector]
	private bool _flipY = false;

	/// <summary>
	/// The _flip Y value, way to rotate
	/// -1 for up rotating local counter clockwise (_flipY = false)
	/// 1 for up rotating local clockwise (_flipY = true)
	/// </summary>
	[SerializeField]
	[HideInInspector]
	private float _flipYVal = -1f;
	
	public bool flipY {
		get {
			return _flipY;
		}

		set {
			_flipY = value;
			_flipYVal = _flipY ? -1f : 1f;
		}
	}

	public Transform cannonArm = null;

	public float rotationArmActivated = 2.2f;
	public float rotationArmDeactivated = Mathf.PI;
	private Quaternion localRotationArm = new Quaternion(0f, 0f, 1f, Mathf.PI);
	public float armActivationTime = 0.5f;

	public Rigidbody2D headRB = null;

	public float stillSpeedSq = 0.05f;

	private bool still = false;
	private bool stillTransition = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (headRB.velocity.sqrMagnitude < stillSpeedSq) {
			if (still) {
				curQuat.w = Mathf.Clamp(curQuat.w + Input.GetAxis("Vertical") * rotationSpeed * _flipYVal, -1f - flexAngle, -1f + flexAngle);
				transform.localRotation = curQuat;
			} else {
				if (!stillTransition)
					StartCoroutine(Energize());
			}
		} else {
			if (!stillTransition)
				StartCoroutine(Deenergize());
		}

	}

	IEnumerator<WaitForSeconds> Energize() {
		stillTransition = true;
		float startT = Time.timeSinceLevelLoad;
		float curT = startT;
		while (curT - startT < armActivationTime) {
			curT = Time.timeSinceLevelLoad;
			localRotationArm.w = Mathf.Lerp(localRotationArm.w, rotationArmActivated, (curT - startT) / armActivationTime);
			cannonArm.localRotation = localRotationArm;
			yield return new WaitForSeconds(0.05f);
		}
		still = true;
		stillTransition = false;

	}

	IEnumerator<WaitForSeconds> Deenergize() {
		stillTransition = true;
		float startT = Time.timeSinceLevelLoad;
		float curT = startT;
		while (curT - startT < armActivationTime) {
			curT = Time.timeSinceLevelLoad;
			localRotationArm.w = Mathf.Lerp(localRotationArm.w, rotationArmDeactivated, (curT - startT) / armActivationTime);
			cannonArm.localRotation = localRotationArm;
			yield return new WaitForSeconds(0.05f);
		}
		still = false;
		stillTransition = false;
		
	}

}
