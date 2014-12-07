using UnityEngine;
using System.Collections;

public class LevelCoordinator : MonoBehaviour {

	public int startingEnemies = 1;
	public GameObject[] enemyPrefabs;

	public FlyTally[] flyTallies;

	// Use this for initialization
	void Start () {
		flyTallies = GameObject.FindObjectsOfType<FlyTally>();
		for (int i=0; i<startingEnemies; i++)
			SpawnEnemy();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RegisterKill(int team, GameObject enemy) {
		Debug.Log(team);
		enemy.GetComponent<EnemyFly>().Kill(team);
		flyTallies[team].CatchFly();
		if (flyTallies[team].completed)
			LevelWon(team);
		else
			SpawnEnemy();
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
