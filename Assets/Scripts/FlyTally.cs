using UnityEngine;
using System.Collections;

public class FlyTally : MonoBehaviour {

	public GameObject[] flies;
	private UnityEngine.UI.Text[] flyClocks;

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
	
	void OnGUI() {
		if (caugthFlies >= flies.Length)
			return;

		SetCurrentFlyTime();
	}

	void SetCurrentFlyTime() {
		flyTimes[caugthFlies] = Time.timeSinceLevelLoad - lastKill;
		float s = 0f;
		if (flyTimes[caugthFlies] >= 1f)
 			s = Mathf.Floor(flyTimes[caugthFlies]);

		int mS = Mathf.RoundToInt((flyTimes[caugthFlies] % s) * 100f);


		if (mS == 100) {
			mS = 0;
			s += 1f;
		}
		if (mS < 0) {
			mS = 0;
		}

		flyClocks[caugthFlies].text = string.Format("{0}:{1:00}", (int) s, mS);
	}

	public void CatchFly () {
		if (caugthFlies  >= flies.Length)
			return;

		SetCurrentFlyTime();

		UnityEngine.UI.Image im = flies[caugthFlies].GetComponent<UnityEngine.UI.Image>();
		im.color = deadFly;
		caugthFlies ++;
		lastKill = Time.timeSinceLevelLoad;
	}

	void Reset() {

		flyClocks = new UnityEngine.UI.Text[flies.Length];
		flyTimes = new float[flies.Length];

		for (int i=0; i<flies.Length;i++) {
			UnityEngine.UI.Image im = flies[i].GetComponent<UnityEngine.UI.Image>();
			im.color = aliveFly;
			//add shake?

			flyClocks[i] = flies[i].GetComponentInChildren<UnityEngine.UI.Text>();
			flyClocks[i].text = "0:00";
		}

		lastKill = Time.timeSinceLevelLoad;
	}
}
