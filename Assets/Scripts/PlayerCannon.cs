using UnityEngine;
using System.Collections;

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
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		curQuat.w = Mathf.Clamp(curQuat.w + Input.GetAxis("Vertical") * rotationSpeed * _flipYVal, -1f - flexAngle, -1f + flexAngle);
		transform.localRotation = curQuat;

	}
}
