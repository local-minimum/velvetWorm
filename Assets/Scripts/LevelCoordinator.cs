using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelCoordinator : MonoBehaviour {

	public int startingEnemies = 1;
	public GameObject[] enemyPrefabs;

	private Dictionary<int, FlyTally> flyTallies = new Dictionary<int, FlyTally>();
	private List<PlayerCoordinator> players = new List<PlayerCoordinator>();

	public GameObject[] FlyTallyPrefabs;

	// Use this for initialization
	void Start () {

//		foreach (PlayerCoordinator pc in GameObject.FindObjectsOfType<PlayerCoordinator>())
//			players.Add(pc);

		players.AddRange(GameObject.FindObjectsOfType<PlayerCoordinator>());
		setupTallies();

		for (int i=0; i<startingEnemies; i++)
			SpawnEnemy();
	}


	private void setupTallies() {
		List<int> dropKeys = new List<int>();

		foreach (KeyValuePair<int, FlyTally> kvp in flyTallies) {
			if (!players.Where(p => p.playerID == kvp.Key).Any())
				dropKeys.Add(kvp.Key);
		}

		foreach (int k in dropKeys)
			flyTallies.Remove(k);

		foreach (int k in players.Select(p => p.playerID).Where(p => !flyTallies.Keys.Where(k => k == p).Any()))
			flyTallies.Add(k, ((GameObject)
			               Instantiate(FlyTallyPrefabs[
			                            flyTallies.Count < FlyTallyPrefabs.Length - 1 ? 
			                                       	flyTallies.Count : 
			                                       	FlyTallyPrefabs.Length - 1])).GetComponent<FlyTally>());
	}

	public void RegisterKill(int team, GameObject enemyGO) {

		EnemyFly enemy = enemyGO.GetComponent<EnemyFly>();
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
		Application.LoadLevel(Application.loadedLevel);
	}

	private void SpawnEnemy() {
		SpawnEnemy(Random.Range(0, enemyPrefabs.Length - 1));
	}

	private void SpawnEnemy(int idX) {
		Instantiate(enemyPrefabs[idX]);
	}
}
