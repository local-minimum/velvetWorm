using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	LevelCoordinator levelCoordinator;

	bool isVisible = true;

	public UnityEngine.UI.Button resumeButton;
	public UnityEngine.UI.Button playButton;
	public UnityEngine.UI.Button controlsButton;
	public UnityEngine.UI.Button aboutUsButton;
	public UnityEngine.UI.Button aboutWormButton;
	public UnityEngine.UI.Button quitButton;

	public UnityEngine.UI.Image menuBG;
	public UnityEngine.UI.Image menuLogo;
	public UnityEngine.UI.Image controlsDisplay;

	bool showingControls = false;

	// Use this for initialization
	void Start () {
		levelCoordinator = GameObject.FindObjectOfType<LevelCoordinator>();
//		PlayerPrefs.DeleteAll();
		Show ();
//		ClickPlay();
	}
	
	// Update is called once per frame
	void Update() {
		if (isVisible) {
			if (showingControls && Input.anyKeyDown)
				HideControls();

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

		ShowButtons(false);
		menuBG.enabled = false;
		menuLogo.enabled = false;

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
		showingControls = true;
		ShowButtons(false);
		controlsDisplay.enabled = true;
	}

	void HideControls() {
		showingControls = false;
		controlsDisplay.enabled = false;
		ShowButtons(true);
	}

	private void ShowButtons(bool val) {
		playButton.gameObject.SetActive(val);
		resumeButton.gameObject.SetActive(val);
		controlsButton.gameObject.SetActive(val);
		aboutUsButton.gameObject.SetActive(val);
		aboutWormButton.gameObject.SetActive(val);
		quitButton.gameObject.SetActive(val);
	}

	public void Show() {
		levelCoordinator.paused = true;
		resumeButton.interactable = levelCoordinator.started;
		ShowButtons(true);
		menuBG.enabled = true;
		menuLogo.enabled = true;
		isVisible = true;
		controlsDisplay.enabled = false;
	}
}
