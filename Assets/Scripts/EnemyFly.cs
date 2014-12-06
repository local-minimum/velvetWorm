using UnityEngine;
using System.Collections;

public class EnemyFly : MonoBehaviour {

	private Vector2 direction;

	[Range(0,10)]
	public float baseSpeed;

	public int scr_width;
	public int scr_height;


	private float speedPerlinX = 0f;
	private float speedPerlinY = 0f;
	
	private float altitudePerlinX = 0f;
	private float altitudePerlinY = 0f;

	private float leftrightPerlinX = 0f;
	private float leftrightPerlinY = 0f;

	// Use this for initialization
	public void Start ()
	{
		direction = Vector2.right;

		speedPerlinX = Random.value * 100;
		altitudePerlinX = Random.value * 100;
		leftrightPerlinX = Random.value * 100;
	}


	
	// Update is called once per frame
	void Update ()
	{
// 		rigidbody2D.velocity = direction;

		Vector3 move = transform.localPosition;

		Vector2 speed = Vector2.right;

		float altitude = transform.localPosition.y + Mathf.PerlinNoise(altitudePerlinX,1);

		direction = rigidbody2D.velocity;

		rigidbody2D.velocity = direction * Mathf.PerlinNoise(altitudePerlinX,altitudePerlinY);

//		transform.localPosition.x += Mathf.PerlinNoise(-1,1);
//		transform.localPosition.y += Mathf.PerlinNoise(-1,1);

//		transform.localPosition = new Vector3(transform.localPosition.x + Mathf.PerlinNoise(-1,1),
//		                                      transform.localPosition.y + Mathf.PerlinNoise(-1,1),
//		                                      transform.localPosition.z);
	}


}
