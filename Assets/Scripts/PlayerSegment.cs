using UnityEngine;
using System.Collections;

public class PlayerSegment : MonoBehaviour {

	public PlayerCannon cannon;
	public PlayerSegment prevSegment = null;

	private float _angle = 0f;
	private bool _shooting = false;
	float _walkstart = 0f;
	public float walkDelay = 0.1f;
	private Animator anim;

	public float angle {
		get {
			return _angle;
		}

		set {
			_angle = value;
			transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

		}
	}

	public bool startWalkNext {
		get {
			return Time.timeSinceLevelLoad - _walkstart > walkDelay;
		}
	}

	public bool followsLead = true;

	// Use this for initialization
	void Start () {
		_angle = Quaternion.Dot(transform.rotation, new Quaternion(0f, 0f, 1f, 0f));
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!anim)
			return;

		if (cannon.ready) {
			if (!_shooting) {
				_shooting = true;
				Debug.Log("No walk");
				anim.Play("Shooting");
			}
		} else {
			if (_shooting && (prevSegment == null || prevSegment.startWalkNext)) {
				_shooting = false;
				_walkstart = Time.timeSinceLevelLoad;
				anim.Play("Walking");
			}
		}

	}
}
