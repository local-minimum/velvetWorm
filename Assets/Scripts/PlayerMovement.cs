using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
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
		float d = Input.GetAxisRaw ("Horizontal");
		wantingToMove = d != 0;

		if (hugSurface) {
			Vector2 v = new Vector2 (transform.position.x, transform.position.y);

			// Try to detect a surface normal, otherwise up
			RaycastHit2D hit = Physics2D.Raycast (transform.position, -playerNormal, rayMaxDistance, layerMask);
			if (hit.collider != null) {
					surfaceNormal = hit.normal;
			} else {
					surfaceNormal = Vector2.up;
			}

			Debug.DrawLine (v, v + surfaceNormal, Color.green);

			if (d < 0) {
				rigidbody2D.velocity = -transform.right * Time.deltaTime * speed;
			} else if (d > 0) {
				rigidbody2D.velocity = transform.right * Time.deltaTime * speed;
			} else {
				rigidbody2D.velocity = Vector2.zero;
			}

		}

	}
}