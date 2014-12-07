using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyFly : MonoBehaviour {



	[Range(0,10)]
	public float baseSpeed;

	[Range(0,1)]
	public float dir_change;

	[Range(1,3)]
	public float point_focus;

	[Range(1,10)]
	public float flyTime;

	public Transform[] points;

	public float pnt_x;
	public float pnt_y;
	private int pnt_x_dist;
	private int pnt_y_dist;

	public bool isAlive = true;


	// Use this for initialization
	public void Start ()
	{
		points = GameObject.FindGameObjectsWithTag("FlyTag").Select(e => e.transform).ToArray();
		StartCoroutine(pointSwitcher());
	}


	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawLine(transform.position, new Vector3(pnt_x,pnt_y));

		Vector2 newDir = new Vector2(Random.value*2 - 1, Random.value*2 - 1 );

		newDir = newDir*dir_change + rigidbody2D.velocity * (1.0f - dir_change);

		rigidbody2D.velocity = Vector2.ClampMagnitude(newDir * 1.5f, baseSpeed);

		if (transform.localPosition.x > (pnt_x + pnt_x_dist))
		{
			rigidbody2D.AddForce(-baseSpeed * point_focus * Vector2.right,ForceMode2D.Force);
			
		}
		if (transform.localPosition.x < (pnt_x - pnt_x_dist))
		{
			rigidbody2D.AddForce(baseSpeed * point_focus * Vector2.right,ForceMode2D.Force);
			
		}
		if (transform.localPosition.y > (pnt_y + pnt_y_dist))
		{
			rigidbody2D.AddForce(-baseSpeed * point_focus * Vector2.up,ForceMode2D.Force);
			
		}
		if (transform.localPosition.y < (pnt_y - pnt_y_dist))
		{
			rigidbody2D.AddForce(baseSpeed * point_focus * Vector2.up,ForceMode2D.Force);
			
		}
	}

	public void Kill(int player)
	{
		isAlive = false;

	}

	IEnumerator<WaitForSeconds> pointSwitcher()
	{
		while (isAlive)
		{
			int index = Random.Range(0,points.Length-1);
			pnt_x = points[index].position.x;
			pnt_y = points[index].position.y;
			float dist = (transform.position - points[index].position).magnitude;


			dist *= (dist * (flyTime / 20));
			Debug.Log(dist);

			yield return new WaitForSeconds(Random.Range(0.8f*dist, 2.0f+dist));
		}
	}
}
