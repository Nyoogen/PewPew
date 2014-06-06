using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public GameObject pauseOverlay;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float stageEndWait;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText stageText;
	public GUIText upgradeOneText;
	public GUIText upgradeTwoText;
	public GUIText pauseText;
	private int score;
	private int stage;
	private bool gameOver;
	private bool restart;
	private bool isStageEnd;
	private bool isPaused;

	private Object cloneOverlay;
	private bool isUpgraded;

	IEnumerator SpawnWave () 
	{
//		yield return new WaitForSeconds (startWait);

		stageText.text = "Stage " + stage;
		yield return new WaitForSeconds (2.0f);
		stageText.text = "";

		for (int i = 0; i < hazardCount; i++)
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (hazard, spawnPosition, spawnRotation);
			yield return new WaitForSeconds (spawnWait);
		}

		// There should be detection to check whether there are any hazards on screen instead of a dumb timer
		yield return new WaitForSeconds (stageEndWait);
		isStageEnd = true;
		stage++;

	}


	void Start () 
	{
		gameOver = false;
		restart = false;
		isStageEnd = false;
		isUpgraded = false;
		restartText.text = "";
		gameOverText.text = "";
		upgradeOneText.text = "";
		upgradeTwoText.text = "";
		pauseText.text = "";
		score = 0;
		stage = 1;
		UpdateScore ();
		StartCoroutine (SpawnWave ());
	}

	void Update ()
	{
		// Don't want to allow the player to pause during any of these states
		if (!gameOver && !restart && !isStageEnd)
		{
			if (Input.GetKeyDown (KeyCode.P))
			{
				if (!isPaused)
				{
					// So like the video said, it's bad form to have repeated code like this (see below) when you could theoretically call a function, but I'm lazy
					cloneOverlay = Instantiate (pauseOverlay, Vector3.up, Quaternion.Euler (90.0f, 0.0f, 0.0f)); // Instantiate the "pause screen", i.e. the transparent layer
					Time.timeScale = 0.0f;
					pauseText.text = "Paused";
					isPaused = true;
				}
				else
				{
					Destroy (cloneOverlay);
					Time.timeScale = 1.0f;
					pauseText.text = "";
					isPaused = false;
				}
			}
		}


		if (gameOver)
		{
			restartText.text = "Press 'R' for restart";
			restart = true;
		}

		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (isStageEnd)
		{
			// This if statement exists solely because we don't want to keep instantiating the "pause screen"
			// note: I'm using "isPaused" because the game-pausing functionality exists, and I don't need another bool for it
			if (!isPaused)
			{
				cloneOverlay = Instantiate (pauseOverlay, Vector3.up, Quaternion.Euler (90.0f, 0.0f, 0.0f)); // Instantiate the "pause screen", i.e. the transparent layer
				upgradeOneText.text = "1. QQ";
				upgradeTwoText.text = "2. Pew Pew";
				Time.timeScale = 0.0f; // pause the game
				isPaused = true;
			}


			// CHOOSE YOUR WEAPON
			if (Input.GetKey ("1"))
			{
				// Clearly going to change this, but for now, "upgrade 1" is just the basic laser
				isUpgraded = false;
				EndStage ();
			}
			else if (Input.GetKey ("2"))
			{
				// "upgrade 2" on the other hand shoots 3 lasers per click
				isUpgraded = true;
				EndStage ();
			}
		}
	}

	// This is a misnomer; this actually cleans up post-upgrade screen
	void EndStage ()
	{
		isStageEnd = false;
		isPaused = false;
		upgradeOneText.text = "";
		upgradeTwoText.text = "";
		Destroy (cloneOverlay);
		Time.timeScale = 1.0f; // resume the game
		StartCoroutine (SpawnWave ());
	}
		
		
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}


	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}


	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}


	// Getters
	public bool getUpgraded ()
	{
		return isUpgraded;
	}

}
