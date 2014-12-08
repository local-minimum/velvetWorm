using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public bool inputSegment = false;
	public PlayerSegment mySegment;
	public LayerMask layerMask;
	public bool hugSurface = true;
	public bool wantingToMove = true;

	public float speed = 100f;
	public float playerNormalGravity = 30f;
	public float turnSpeed = 90f;
	public float smoothTurn = 10f;
	public float rayMaxDistance = 1f;

	private Vector2 playerNormal;
	private Vector2 _surfaceNormal;
	private int normalMemory;

	private bool originalDirection = true;
	private Vector3 originalScale;
	private Vector3 reflectedScale;
	private Quaternion slimeOriginalRotation;
	private Quaternion slimeReverseRotation;

	public bool playerDirection {
		get {
			return originalDirection;
		}
	}

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
		reflectedScale = originalScale;
		reflectedScale.x *= -1f;
		mySegment = gameObject.GetComponent<PlayerSegment>();

//		slimeOriginalRotation = mySegment.cannon.slimer.transform.localRotation;
//		slimeReverseRotation = Quaternion.Euler(-90f, 90f, 180f) * slimeOriginalRotation;
//		slimerReverseScale.x *= -1;

		playerNormal = transform.up;
	}

	void FixedUpdate() {
		rigidbody2D.AddForce (-playerNormalGravity * rigidbody2D.mass * playerNormal);
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	Vector2 surfaceNormal {
		set {
			_surfaceNormal = value;
			normalMemory = 3;
		}

		get {
			normalMemory--;

			return normalMemory > 0 ? _surfaceNormal : Vector2.up;
		}
	}

	private bool segmentWithoutContact {
		get {
			return normalMemory < 1;
		}
	}

	private void Movement() {
		float d = inputSegment ? 0f : Input.GetAxisRaw ("Horizontal");
		wantingToMove = Input.GetButton("Horizontal");

		if (Input.GetButtonDown("Flip")) {
			originalDirection = !originalDirection;
			transform.localScale = originalDirection ? originalScale : reflectedScale;
//			mySegment.cannon.transform.localScale = originalDirection ? originalScale : reflectedScale;
//			mySegment.cannon.slimer.transform.localRotation = originalDirection ? slimeOriginalRotation : slimeReverseRotation;
		}

		if (hugSurface) {
			Vector2 v = new Vector2 (transform.position.x, transform.position.y);


			// Try to detect a surface normal

			RaycastHit2D hit = Physics2D.Raycast (collider2D.bounds.center, -playerNormal, rayMaxDistance, layerMask);

//			RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, -playerNormal, rayMaxDistance, layerMask);
			if (hit.collider != null) {
				surfaceNormal = hit.normal;
				Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
//				Debug.DrawLine(collider2D.bounds.center, collider2D.bounds.center - (Vector3) playerNormal, Color.red);
			}

			if (segmentWithoutContact)
				return;


			
			playerNormal = Vector2.Lerp (playerNormal, _surfaceNormal, smoothTurn);

			if (mySegment.prevSegment && wantingToMove)
				playerNormal = Vector2.Lerp(playerNormal, mySegment.prevSegment.movement.playerNormal, smoothTurn);

			transform.up = playerNormal;
			
			if (wantingToMove) 
				rigidbody2D.velocity = transform.right * Time.deltaTime * speed * d * (originalDirection ? 1f : -1f);
			
		}
	
	}

	/*
	void TestFlip(float d) {
		if ((Mathf.Sign(d) < 0f) == originalDirection) {
			originalDirection = !originalDirection;
		}

	}*/
}