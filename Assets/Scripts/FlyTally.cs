using UnityEngine;
using System.Collections;

public class FlyTally : MonoBehaviour {

	public GameObject[] flies;
	private float[] flyTimes;

	public Color aliveFly;
	public Color deadFly;

	private int caugthFlies = 0;

	private float lastKill;

	public bool completed {
		get {
			return caugthFlies == flies.Length;
		}
	}

	// Use this for initialization
	void Start () {
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CatchFly () {
		if (caugthFlies  >= flies.Length)
			return;

		flyTimes[caugthFlies] = Time.timeSinceLevelLoad - lastKill;
		SpriteRenderer r = (SpriteRenderer) flies[caugthFlies].renderer;
		r.color = deadFly;
		caugthFlies ++;
		lastKill = Time.timeSinceLevelLoad;
	}

	void Reset() {
		foreach (GameObject g in flies) {
			SpriteRenderer r = (SpriteRenderer) g.renderer;
			r.color = aliveFly;
		}

		flyTimes = new float[flies.Length];
		lastKill = Time.timeSinceLevelLoad;
	}
}
