using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public LayerMask layerMask;

	public float speed = 100f;
	public float playerNormalGravity = 10f;
	public float turnSpeed = 180f;
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
		Vector2 v = new Vector2 (transform.position.x, transform.position.y);

		// Try to detect a surface normal, otherwise up
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -playerNormal, rayMaxDistance, layerMask);
		if (hit.collider != null) {
			surfaceNormal = hit.normal;
		} else {
			surfaceNormal = Vector2.up;
		}

		Debug.DrawLine(v, v + surfaceNormal, Color.green);

		playerNormal = Vector2.Lerp(playerNormal, surfaceNormal, smoothTurn * Time.deltaTime);
		transform.up = playerNormal;

		rigidbody2D.velocity = transform.right * Time.deltaTime * speed * Input.GetAxisRaw ("Horizontal");
	}
}
