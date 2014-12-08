using UnityEngine;
using System.Collections;

public class PlayerSegment : MonoBehaviour {

	public PlayerCannon cannon;
	public PlayerSegment prevSegment = null;
	private PlayerMovement playerMovement;

	private bool _shooting = false;
	float _walkstart = 0f;
	public float walkDelay = 0.1f;
	private Animator anim;

	public float angle {
		get {
			return transform.eulerAngles.z ;
		}

		set {
			while (Mathf.Abs(value) > 360)
				value -= Mathf.Sign(value) * 360;

			transform.rotation = Quaternion.AngleAxis(value, Vector3.forward);

		}
	}

	public bool startWalkNext {
		get {
			return Time.timeSinceLevelLoad - _walkstart > walkDelay;
		}
	}

	public PlayerMovement movement {

		get {
			return playerMovement;
		}
	}

	public bool followsLead = true;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!anim)
			return;

		if (cannon.ready) {
			if (!_shooting) {
				_shooting = true;
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
