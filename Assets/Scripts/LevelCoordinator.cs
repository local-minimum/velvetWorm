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

	// Use this for initialization
	void Start () {

		if (!uiCanvas)
			uiCanvas = GameObject.FindObjectOfType<Canvas>();

//		players.AddRange(GameObject.FindObjectsOfType<PlayerCoordinator>());
		setupTallies();

		for (int i=0; i<startingEnemies; i++)
			SpawnEnemy();


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
				SpawnEnemy();
		}
	}

	private void LevelWon(int team) {
		float t = flyTallies[team].averageTime;

		if (PlayerPrefs.GetFloat(recordTimeKey, 999f) > t) 
			PlayerPrefs.SetFloat(recordTimeKey, t);

//		Application.LoadLevel(Application.loadedLevel);
	}

	private void SpawnEnemy() {
		SpawnEnemy(Random.Range(0, enemyPrefabs.Length - 1));
	}

	private void SpawnEnemy(int idX) {
		Instantiate(enemyPrefabs[idX]);
	}
	
}
