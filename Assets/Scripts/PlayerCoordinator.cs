using UnityEngine;
using System.Collections.Generic;

public class PlayerCoordinator : MonoBehaviour {

	private static int lastPlayer = -1;
	
	private int _playerID;

	private List<PlayerMovement> playerMovements = new List<PlayerMovement>();
	private PlayerCannon playerCannon;
	private List<PlayerSegment> playerSegments = new List<PlayerSegment>();
	private PlayerMovement _playerMovementActive;

	public int playerID {
		
		get {
			return _playerID;
		}
	}

	public PlayerMovement playerMovementActive {
		get {
			return _playerMovementActive;
		}
	}

	// Use this for initialization
	void Start () {
		lastPlayer++;
		_playerID = lastPlayer;

		playerCannon = gameObject.GetComponentInChildren<PlayerCannon>();
		foreach (Transform child in transform) {
			PlayerMovement pm = child.GetComponent<PlayerMovement>();
			if (pm) {
				playerMovements.Add(pm);
				if (pm.inputSegment)
					_playerMovementActive = pm;
			}
			PlayerSegment ps = child.GetComponent<PlayerSegment>();
			if (ps)
				playerSegments.Add(ps);
		}
	}

}
