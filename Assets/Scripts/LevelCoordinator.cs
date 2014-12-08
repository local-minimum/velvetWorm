using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelCoordinator : MonoBehaviour {

	public int startingEnemies = 1;
	public GameObject[] enemyPrefabs;

	private Dictionary<int, FlyTally> flyTallies = new Dictionary<int, FlyTally>();
	public List<PlayerCoordinator> players = new List<PlayerCoordinator>();

	public GameObject[] FlyTallyPrefabs;
	public Canvas uiCanvas;
	public UnityEngine.UI.Text recordTime;

	public string lvlName;
	
	private float lastKill;
	private bool noSpawn = true;
	private bool isPaused = true;
	private bool showUIWhilePlaying;

	public bool started {
		get {
			return !noSpawn;
		}
	}

	public bool paused {
		get {
			return isPaused;
		}

		set {
			Time.timeScale = value ? 0f : 1f;
			isPaused = value;
			SetUIVisibility(!value ? showUIWhilePlaying : false);
		}
	}

	private string recordTimeKey {
		get {
			return string.Format("{0}_{1}_recordTime", lvlName, numberOfPlayers);
		}
	}

	public int numberOfPlayers {
		get {
			return players.Count();
		}
	}

	public float enemyClock {
		get {
			return noSpawn ? Time.timeSinceLevelLoad : lastKill;
		}
	}

	// Use this for initialization
	void Start () {
		showUIWhilePlaying = PlayerPrefs.GetInt("ShowUI", 1) == 1;
		Time.timeScale = 0f;
		if (!uiCanvas)
			uiCanvas = GameObject.FindObjectOfType<Canvas>();

//		players.AddRange(GameObject.FindObjectsOfType<PlayerCoordinator>());
		setupTallies();

		recordTime.text = string.Format("Best Time: {0}", 
		                                FlyTally.TimeToString(PlayerPrefs.GetFloat(recordTimeKey, 999.99f)));
	
	}


	private void setupTallies() {
		List<int> dropKeys = new List<int>();

		foreach (KeyValuePair<int, FlyTally> kvp in flyTallies) {
			if (!players.Where(p => p.playerID == kvp.Key).Any())
				dropKeys.Add(kvp.Key);
		}

		foreach (int k in dropKeys)
			flyTallies.Remove(k);

		foreach (int k in players.Select(p => p.playerID).Where(p => !flyTallies.Keys.Where(k => k == p).Any())) {
			int prefab = flyTallies.Count < FlyTallyPrefabs.Length - 1 ? flyTallies.Count : FlyTallyPrefabs.Length - 1;
			flyTallies.Add(k, ((GameObject)
			               Instantiate(FlyTallyPrefabs[prefab])).GetComponent<FlyTally>());


			flyTallies[k].transform.SetParent(uiCanvas.transform, false);
		}

//		Debug.Log(players[0].playerID);
	}
	private void SetSpawnTime() {
		lastKill = Time.timeSinceLevelLoad;
	}

	public void RegisterKill(int team, EnemyFly enemy) {
		Debug.Log(team);
		if (team < 0) 
			team = players[0].playerID;

		enemy.Slime();
		if (enemy.isAlive) {
			enemy.Kill(team);
			flyTallies[team].CatchFly();
			if (flyTallies[team].completed)
				LevelWon(team);
			else
				StartCoroutine( SpawnEnemy(Random.Range(1f, 3f)));
		}
	}

	private void LevelWon(int team) {
		float t = flyTallies[team].averageTime;

		if (PlayerPrefs.GetFloat(recordTimeKey, 999f) > t) 
			PlayerPrefs.SetFloat(recordTimeKey, t);

//		Application.LoadLevel(Application.loadedLevel);
	}

	IEnumerator<WaitForSeconds> SpawnEnemy(float delay) {
		noSpawn = true;
		yield return new WaitForSeconds(delay);
		Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)]);
		SetSpawnTime();
		noSpawn = false;
	}

	IEnumerator<WaitForSeconds> SpawnEnemy(int idX, float delay) {
		noSpawn = true;
		yield return new WaitForSeconds(delay);
		Instantiate(enemyPrefabs[idX]);
		SetSpawnTime();
		noSpawn = false;
	}


	public void SpawnOneEnemyEach() {
		
		for (int i=0; i<startingEnemies; i++)
			StartCoroutine( SpawnEnemy(Random.Range(1f, 3f)));
	}

	public void Reset() {
		foreach (EnemyFly ef in GameObject.FindObjectsOfType<EnemyFly>())
			Destroy(ef.gameObject);

		foreach (FlyTally ft in flyTallies.Values)
			ft.Reset();

		noSpawn = true;
	}

	private void SetUIVisibility(bool val) {

		foreach (FlyTally ft in flyTallies.Values)
			ft.gameObject.SetActive(val);
	}
}
