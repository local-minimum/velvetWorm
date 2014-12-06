using UnityEngine;
using System.Collections.Generic;

public class PlayerCannon : MonoBehaviour {

	[Range(0f, 0.5f)]
	public float flexAngle = 0.5f;

	[Range(0f, 0.1f)]
	public float shootingWobbling = 0.05f;

	[Range(0f, 10f)]
	public float shootingWobblingF = 1f;

	private float _rndWobblingX;
	private float _rndWobblingY = 0f;

	[Range(0f, 0.5f)]
	public float rotationSpeed = 0.1f;

	private Quaternion curQuat = new Quaternion(0f, 0f, 1f, -1f);

	public PlayerMovement headMovement;

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

	public PlayerSegment cannonArm = null;

	public float rotationArmActivated = 2.2f;
	private float rotationArmDeactivated = Mathf.PI;
	public float armActivationTime = 0.5f;

	public Rigidbody2D headRB = null;

	public float stillSpeedSq = 0.05f;

	private bool still = false;
	private bool stillTransition = false;

	public ParticleSystem slimeEmitter;
	private bool _shooting = false;
	private float perlinMean = 0.4652489f;

	public bool ready {
		get {
			return still;
		}
	}

	// Use this for initialization
	void Start () {
		_rndWobblingX = 100f * Random.value;
	}
	
	// Update is called once per frame
	void Update () {
		if (headRB.velocity.sqrMagnitude < stillSpeedSq && !headMovement.wantingToMove) {
			if (still) {
				curQuat.w = Mathf.Clamp(curQuat.w + Input.GetAxis("Vertical") * rotationSpeed * _flipYVal, -6.5f - flexAngle, -6.5f + flexAngle);
				if (_shooting) {
					_rndWobblingY += shootingWobblingF * Time.deltaTime;
					curQuat.w += shootingWobbling * (Mathf.PerlinNoise(_rndWobblingX, _rndWobblingY) - perlinMean);
				}
				transform.localRotation = curQuat;
			} else {
				if (!stillTransition)
					StartCoroutine(Energize());
			}
		} else {
			if (!stillTransition && still)
				StartCoroutine(Deenergize());
		}

	}

	void FixedUpdate() {
//		Debug.Log(string.Format("{0} {1} {2}", still, Input.GetButton("Fire1"), slimeEmitter.isPaused));
		if (still && Input.GetButton("Fire1") && !_shooting) {
		    slimeEmitter.Play();
			_shooting = true;
		} else if ((!still || !Input.GetButton("Fire1")) && _shooting) {
			slimeEmitter.Stop();
			_shooting = false;
		}
	}

	IEnumerator<WaitForSeconds> Energize() {
		stillTransition = true;
		headMovement.hugSurface = false;
		rotationArmDeactivated = cannonArm.angle;
		float rotTarget = rotationArmDeactivated + rotationArmActivated;
		float startT = Time.timeSinceLevelLoad;
		float curT = startT;
		while (curT - startT < armActivationTime) {
			curT = Time.timeSinceLevelLoad;
			cannonArm.angle = Mathf.Lerp(cannonArm.angle, rotTarget, (curT - startT) / armActivationTime);
			yield return new WaitForSeconds(0.05f);
		}
		still = true;
		stillTransition = false;

	}

	IEnumerator<WaitForSeconds> Deenergize() {
		stillTransition = true;
		float rotTarget = rotationArmDeactivated;
		float startT = Time.timeSinceLevelLoad;
		float curT = startT;
		while (curT - startT < armActivationTime) {
			curT = Time.timeSinceLevelLoad;
			cannonArm.angle = Mathf.Lerp(cannonArm.angle, rotTarget, (curT - startT) / armActivationTime);
			yield return new WaitForSeconds(0.05f);
		}
		headMovement.hugSurface = true;
		still = false;
		stillTransition = false;
		
	}

}
