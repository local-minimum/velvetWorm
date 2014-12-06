using UnityEngine;
using System.Collections;

public class EnemyFly : MonoBehaviour {



	[Range(0,10)]
	public float baseSpeed;

	[Range(0,1)]
	public float dir_change;



	public int pnt_x;
	public int pnt_y;
	public int pnt_x_dist;
	public int pnt_y_dist;


	// Use this for initialization
	public void Start ()
	{

	}


	
	// Update is called once per frame
	void Update ()
	{


		Vector2 newDir = new Vector2(Random.value*2 - 1, Random.value*2 - 1 );


		newDir = newDir*dir_change + rigidbody2D.velocity * (1.0f - dir_change);


		rigidbody2D.velocity = Vector2.ClampMagnitude(newDir * 1.5f, baseSpeed);

//			Vector2.ClampMagnitude(rigidbody2D.velocity, baseSpeed);
//		rigidbody2D.AddForce( newDir, ForceMode2D.Force);

		if (transform.localPosition.x > (pnt_x + pnt_x_dist))
		{
			rigidbody2D.AddForce(-baseSpeed * Vector2.right,ForceMode2D.Force);
			
		}
		if (transform.localPosition.x < (pnt_x - pnt_x_dist))
		{
			rigidbody2D.AddForce(baseSpeed* Vector2.right,ForceMode2D.Force);
			
		}
		if (transform.localPosition.y > (pnt_y + pnt_y_dist))
		{
			rigidbody2D.AddForce(-baseSpeed * Vector2.up,ForceMode2D.Force);
			
		}
		if (transform.localPosition.y < (pnt_y - pnt_y_dist))
		{
			rigidbody2D.AddForce(baseSpeed*Vector2.up,ForceMode2D.Force);
			
		}

//		transform.localPosition.x += Mathf.PerlinNoise(-1,1);
//		transform.localPosition.y += Mathf.PerlinNoise(-1,1);

//		transform.localPosition = new Vector3(transform.localPosition.x + Mathf.PerlinNoise(-1,1),
//		                                      transform.localPosition.y + Mathf.PerlinNoise(-1,1),
//		                                      transform.localPosition.z);
	}


}
