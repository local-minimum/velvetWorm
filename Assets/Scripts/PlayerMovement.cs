using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 100;
	public float playerNormalGravity = 30;
	public float turnSpeed = 90;
	public float smoothTurn = 10;
	//public float deltaGround = 0.2f;
	public float rayMaxDistance = 2;

	private Vector2 playerNormal;
	private Vector2 surfaceNormal;
	//private float distanceToGround;
	//private bool isGrounded;

	// Use this for initialization
	void Start () {
		playerNormal = transform.up;
		//rigidbody.freezeRotation = true; only 3d?
		//distanceToGround = collider2D.bounds.extents.y - collider2D.bounds.center.y;
	}

	void FixedUpdate() {
		rigidbody2D.AddForce (-playerNormalGravity * rigidbody2D.mass * playerNormal);
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	private void Movement() {
		/*
		//transform.Rotate (0, Input.GetAxis ("Movement") * turnSpeed * Time.deltaTime, 0);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, -playerNormal, rayMaxDistance);
		if (hit.collider != null) {
			//isGrounded = hit.distance <= (distanceToGround + deltaGround);
			Debug.DrawLine(hit.point, hit.normal);
			surfaceNormal = hit.normal;
		} else {
			//isGrounded = false;
			surfaceNormal = Vector2.up;
		}

		playerNormal = Vector2.Lerp(playerNormal, surfaceNormal, smoothTurn * Time.deltaTime);

		Vector2 playerForward = 
*/
		//Quaternion targetRot = Quaternion.LookRotation(playerForward, playerNormal);

		//transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, smoothTurn * Time.deltaTime);

		//transform.Translate(0, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime); 


		float d = Input.GetAxisRaw ("Horizontal");
		if (d < 0) {
			rigidbody2D.velocity = -transform.right * Time.deltaTime * speed;
		} else if (d > 0) {
			rigidbody2D.velocity = transform.right * Time.deltaTime * speed;
		} else {
			rigidbody2D.velocity = Vector2.zero;
		}

	}
}
