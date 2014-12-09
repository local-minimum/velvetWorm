using UnityEngine;
using System.Collections;

public class EnemySquirtHit : MonoBehaviour {

	private EnemyFly enemyController;
	private LevelCoordinator lvlCoord;

	// Use this for initialization
	void Start () {
		enemyController = gameObject.GetComponentInParent<EnemyFly>();
		lvlCoord = GameObject.FindObjectOfType<LevelCoordinator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other) {
		lvlCoord.RegisterKill(-1, enemyController);
		audio.Play();
	}
}
