using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	LevelCoordinator levelCoordinator;

	bool isVisible = true;

	public UnityEngine.UI.Button resumeButton;

	// Use this for initialization
	void Start () {
		levelCoordinator = GameObject.FindObjectOfType<LevelCoordinator>();
		Show ();
	}
	
	// Update is called once per frame
	void Update() {
		if (isVisible) {

		} else {
			if (Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel"))
				Show();
		}
	}

	public void ClickPlay() {
//		Debug.Log("Player");
		levelCoordinator.Reset();
//		Debug.Log("Player");
		levelCoordinator.SpawnOneEnemyEach();
//		Debug.Log("Player");
		ClickResume();
//		Debug.Log("Player");
	}

	public void ClickResume() {
		levelCoordinator.paused = false;
		foreach (Transform t in transform)
			t.gameObject.SetActive(false);
		isVisible = false;
	}

	public void ClickQuit() {
		Application.Quit();
	}

	public void ClickAboutVelvetWorms() {
		Application.OpenURL("https://www.youtube.com/watch?v=mrL2A7my1fc");
	}

	public void ClickAboutUs() {

	}

	public void ClickControls() {

	}

	public void Show() {
		levelCoordinator.paused = true;
		resumeButton.interactable = levelCoordinator.started;
		foreach (Transform t in transform)
			t.gameObject.SetActive(true);
		isVisible = true;
	}
}
