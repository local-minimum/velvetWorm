using UnityEngine;
using System.Collections.Generic;

public class PlayerCannon : MonoBehaviour {
	

	[Range(0f, 45f)]
	public float flexAngle = 0.5f;

	[Range(0f, 0.1f)]
	public float shootingWobbling = 0.05f;

	[Range(0f, 10f)]
	public float shootingWobblingF = 1f;

	private float _rndWobblingX;
	private float _rndWobblingY = 0f;

	[Range(0f, 0.5f)]
	public float rotationSpeed = 0.1f;

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

	private PlayerCoordinator playerCoord;

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
	
	private bool _shooting = false;
	private float perlinMean = 0.4652489f;
	private float baseW;

	[Range(0f, 10f)]
	public float maxSlimingTime = 2f;

	[Range(0f, 10f)]
	public float slimeBetweenTime = 0.5f;

	private float lastSlime;

	public bool ready {
		get {
			return still;
		}
	}

	public float angle {
		get {
			return transform.localEulerAngles.z ;
		}
		
		set {

			transform.localRotation = Quaternion.AngleAxis(WrapAngle(value), Vector3.forward);
			
		}
	}

	// Use this for initialization
	void Start () {

		playerCoord = gameObject.GetComponentInParent<PlayerCoordinator>();
		baseW = angle;
		_rndWobblingX = 100f * Random.value;
	}
	
	// Update is called once per frame
	void Update () {
		if (stillTransition)
			return;

		if  (still && (headMovement.wantingToMove || headRB.velocity.sqrMagnitude > stillSpeedSq)) {
			StartCoroutine(Deenergize());
		} else if (_shooting) {
			_rndWobblingY += shootingWobblingF * Time.deltaTime;
			
			angle = ClampRotation(angle + shootingWobbling * (Mathf.PerlinNoise(_rndWobblingX, _rndWobblingY) - perlinMean),
			                      -flexAngle, flexAngle, baseW); 

		} else if (still) {

				angle = ClampRotation(angle + Input.GetAxis("Vertical") * rotationSpeed * _flipYVal, 
				                      -flexAngle, flexAngle, baseW); 

		} else {

					StartCoroutine(Energize());
		}

	}

	float ClampRotation(float angle, float minAngle, float maxAngle, float clampAroundAngle = 0)
	{


		angle = WrapAngle(angle) - WrapAngle(clampAroundAngle);
		

		//Clamp to desired range
		angle = Mathf.Clamp(angle, minAngle, maxAngle);


		//Set the angle back to the transform and rotate it back to right side up
		return clampAroundAngle + angle;
	}


	//Make sure angle is within 0,360 range
	float WrapAngle(float angle)
	{
		//If its negative rotate until its positive
		while (angle < 0)
			angle += 360;
		
		//If its to positive rotate until within range
		return Mathf.Repeat(angle, 360);
	}

	void FixedUpdate() {
//		Debug.Log(string.Format("{0} {1} {2}", still, Input.GetButton("Fire1"), slimeEmitter.isPaused));
		if (still && Input.GetButton("Fire1") && !_shooting &&  (Time.timeSinceLevelLoad - lastSlime > slimeBetweenTime)) {
//		    particleSystem.Play();
			_shooting = true;

		} else if (_shooting && (Time.timeSinceLevelLoad - lastSlime < maxSlimingTime ) && Input.GetButton("Fire1")  ) {

		
		} else if (_shooting) {
//			particleSystem.Stop();
			_shooting = false;
			lastSlime = Time.timeSinceLevelLoad;
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
			cannonArm.angle = Mathf.LerpAngle(cannonArm.angle, rotTarget, (curT - startT) / armActivationTime);
			yield return new WaitForSeconds(0.05f);
		}
		still = true;
		stillTransition = false;
		lastSlime = Time.timeSinceLevelLoad;

	}

	IEnumerator<WaitForSeconds> Deenergize() {
		stillTransition = true;
		float rotTarget = rotationArmDeactivated;
		float startT = Time.timeSinceLevelLoad;
		float curT = startT;
		while (curT - startT < armActivationTime) {
			curT = Time.timeSinceLevelLoad;
			cannonArm.angle = Mathf.LerpAngle(cannonArm.angle, rotTarget, (curT - startT) / armActivationTime);
			yield return new WaitForSeconds(0.05f);
		}
		headMovement.hugSurface = true;
		still = false;
		stillTransition = false;
	}

}
