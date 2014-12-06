using UnityEngine;
using System.Collections;

public class PlayerSegment : MonoBehaviour {

	public PlayerSegment previousSegm;
	public PlayerSegment nextSegm;

	public Transform nextLink;
	public Transform prevLink;

	private float _angle = 0f;
	
	public float angle {
		get {
			return _angle;
		}

		set {
			_angle = value;
			transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

		}
	}

	public bool followsLead = true;

	// Use this for initialization
	void Start () {
		_angle = Quaternion.Dot(transform.rotation, new Quaternion(0f, 0f, 1f, 0f));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!followsLead || !previousSegm) 
			return;

//		float targetAngle = previousSegm.angle;
//
//		if (nextSegm) {
//			targetAngle += nextSegm.angle;
//		}else {
//			targetAngle += _angle;
//		}
//
//		angle = Mathf.LerpAngle(angle, targetAngle / 2f, 0.25f);

		transform.position += (previousSegm.nextLink.position - prevLink.position) / 1.25f;

	}
}
