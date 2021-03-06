﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyFly : MonoBehaviour {

	[Range(0, 50f)]
	public float deadDropGravity;

	[Range(0, 100f)]
	public float deadTorque;

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

	private bool _isAlive = true;

	private LevelCoordinator levelCoordinator;

	private ParticleSystem selfSlimer;
	public AudioClip dyingSound;
	public AudioClip thudSound;
	public AudioClip splotSound;

	private float soundX = 0f;

	public bool isAlive {
		get {
			return _isAlive;
		}

	}

	// Use this for initialization
	public void Start ()
	{
		levelCoordinator = GameObject.FindObjectOfType<LevelCoordinator>();
		selfSlimer = gameObject.GetComponent<ParticleSystem>();
		points = GameObject.FindGameObjectsWithTag("FlyTag").Select(e => e.transform).ToArray();
		transform.position = points[Random.Range(0, points.Count() - 1)].position;
		StartCoroutine(pointSwitcher());
	}


	
	// Update is called once per frame
	void Update ()
	{
		if (!_isAlive || levelCoordinator.paused)
			return;

		soundX += Random.Range(0.1f, 0.3f);
		audio.volume = Mathf.PerlinNoise(0f, soundX);

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
		_isAlive = false;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.fixedAngle = false;
		rigidbody2D.AddTorque(Random.Range(-deadTorque, deadTorque));
		rigidbody2D.gravityScale = deadDropGravity;
		gameObject.layer = LayerMask.NameToLayer("Food");
		audio.Stop();
		audio.loop = false;
		audio.PlayOneShot(dyingSound);
		Animator anim = GetComponent<Animator>();
		if (anim) {
			anim.Play("Death");
		}
	}

	public void Slime() {
		selfSlimer.Emit(10);
	}

	IEnumerator<WaitForSeconds> pointSwitcher()
	{
		while (_isAlive)
		{
			int index = Random.Range(0,points.Length-1);
			pnt_x = points[index].position.x;
			pnt_y = points[index].position.y;
			float dist = (transform.position - points[index].position).magnitude;


			dist *= (dist * (flyTime / 20));
//			Debug.Log(dist);

			yield return new WaitForSeconds(Random.Range(0.8f*dist, 2.0f+dist));
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (isAlive)
			return;

		audio.PlayOneShot(splotSound, 1f);
		audio.PlayOneShot(thudSound, 1f);
	}
}
