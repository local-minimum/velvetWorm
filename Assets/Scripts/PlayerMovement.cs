using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	private static int lastPlayer = -1;

	private int _playerID;

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
	private Vector2 surfaceNormal;

	public int playerID {

		get {
			return _playerID;
		}
	}

	// Use this for initialization
	void Start () {
		lastPlayer++;
		_playerID = lastPlayer;

		mySegment = gameObject.GetComponent<PlayerSegment>();
		playerNormal = transform.up;
	}

	void FixedUpdate() {
		rigidbody2D.AddForce (-playerNormalGravity * rigidbody2D.mass * playerNormal);
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	private void Movement() {
		float d = inputSegment ? 0f : Input.GetAxisRaw ("Horizontal");
		wantingToMove = Input.GetButton("Horizontal");

			if (hugSurface) {
				Vector2 v = new Vector2 (transform.position.x, transform.position.y);
				if (wantingToMove) 
					TestFlip(d);
				// Try to detect a surface normal, otherwise up
				RaycastHit2D hit = Physics2D.Raycast (transform.position, -playerNormal, rayMaxDistance, layerMask);
				if (hit.collider != null) {
					surfaceNormal = hit.normal;
				} else {
					surfaceNormal = Vector2.up;
				}
				
				Debug.DrawLine (v, v + surfaceNormal, Color.green);
				
				playerNormal = Vector2.Lerp (playerNormal, surfaceNormal, smoothTurn * Time.deltaTime);
				transform.up = playerNormal;
				
				if (wantingToMove) 
					rigidbody2D.velocity = transform.right * Time.deltaTime * speed * d;
				
			}

		
	}

	void TestFlip(float d) {
		if (Mathf.Sign(d) != Mathf.Sign(transform.localScale.x)) {
			Vector3 s = transform.localScale;
			s.x *= -1f;
			transform.localScale = s;
		}
	}
}