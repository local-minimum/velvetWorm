using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FlyTally : MonoBehaviour {

	public GameObject[] flies;
	private UnityEngine.UI.Text[] flyClocks;

	private float[] flyTimes;

	public Color aliveFly;
	public Color deadFly;

	private int caugthFlies = 0;
	private LevelCoordinator lvlCoord;


	public bool completed {
		get {
			return caugthFlies == flies.Length;
		}
	}

	public float averageTime {
		get {
			return flyTimes.Sum() / (float) flyTimes.Count();
		}
	}

	// Use this for initialization
	void Start () {
		lvlCoord = GameObject.FindObjectOfType<LevelCoordinator>();

		Reset();
	}
	
	void OnGUI() {
		if (caugthFlies >= flies.Length)
			return;

		SetCurrentFlyTime();
	}

	public static string TimeToString(float f) {

		float s = 0f;
		if (f >= 1f)
			s = Mathf.Floor(f);
		
		int mS = Mathf.RoundToInt((f % s) * 100f);
		
		
		if (mS == 100) {
			mS = 0;
			s += 1f;
		}
		if (mS < 0) {
			mS = 0;
		}

		if (s > 999) {
			s = 999f;
			mS = 99;
		}

		return string.Format("{0}:{1:00}", (int) s, mS);
	}

	void SetCurrentFlyTime() {
		flyTimes[caugthFlies] = Time.timeSinceLevelLoad - lvlCoord.enemyClock;
		flyClocks[caugthFlies].text = TimeToString(flyTimes[caugthFlies]);
	}


	public void CatchFly () {
		if (caugthFlies  >= flies.Length)
			return;

		SetCurrentFlyTime();

		UnityEngine.UI.Image im = flies[caugthFlies].GetComponent<UnityEngine.UI.Image>();
		im.color = deadFly;
		caugthFlies ++;
	}

	public void Reset() {
		gameObject.SetActive(true);
//		Debug.Log(flies.Length);
		flyClocks = new UnityEngine.UI.Text[flies.Length];
		flyTimes = new float[flies.Length];
//		Debug.Log("X");
		for (int i=0; i<flies.Length;i++) {
			UnityEngine.UI.Image im = flies[i].GetComponent<UnityEngine.UI.Image>();
			im.color = aliveFly;
//			Debug.Log(i);

			flyClocks[i] = flies[i].GetComponentInChildren<UnityEngine.UI.Text>();
//			Debug.Log("--");
			flyClocks[i].text = "0:00";
		}

	}
}
