using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System;

public class Spawner :MonoBehaviour {
	// Represents the number of bugs per minute
	[SerializeField] private float m_InitSpawnRate = 50f;
	// Use this to adjust the difference in maxbugs per level
	[SerializeField] private float m_Gradient = 1.5f;
	// The amount of time given to the player to rest between waves of bugs
	[SerializeField] private float m_TimeBetweenLevels = 2f;
	// A list containing all Bug prefabs
	[SerializeField] private GameObject[] m_Bug;
	// These game objects limit the x value of the spawn position
	[SerializeField] private GameObject m_LimitMax;
	[SerializeField] private GameObject m_LimitMin;
	

	private float m_WaitTimer;

	// 
	private float m_TimeLeft;
	private float m_ActualSpawnRate;
	private bool m_LevelSpawned;
	private int m_BugsSpawnedInLevel;

	public GameObject m_Target;
	public ScoreKeep m_ScoreKeeper;
	public int m_BugsLeft;
	public static int m_Score;
	public static int m_Level;
	public int MaxBugs { get; set; }

	// Generates a random position between the defined position limits
	private Vector3 SpawnLocation {
		get {
			if(m_LimitMin != null && m_LimitMax != null) {
				Vector3 vector3 = gameObject.transform.position;
				vector3.x += UnityEngine.Random.Range(m_LimitMin.transform.position.x, m_LimitMax.transform.position.x);
				return vector3;
			}
			else {
				Debug.LogError("Position Limits not set in Spawner");
				return new Vector3(0,0,0);
			}
		}
	}
	/// Updates the maximum number buugs based off the current level
	private void UpdateMaxBugs() {
		// if level is a multiple of 5
		if(m_Level % 5 == 0) {
			MaxBugs = m_Level * 10;
		}
		// if level + 1 is a multiple of 5
		else if((m_Level + 1) % 5 == 0) {
			MaxBugs = MaxBugs + 10;
		}
		// else
		else {
			MaxBugs = MaxBugs + 5;
		}
		Debug.Log("Max Bugs Updated: " + MaxBugs.ToString() + ". Level: " + m_Level);
	}
	// Updates the wait timer between bugs
	private float BugTimer() {
		// Generates a time between each bug, based off of Bugs Per Minute
		m_ActualSpawnRate = m_InitSpawnRate + m_Level * (10 / m_Gradient);
		float t = 1 / (m_ActualSpawnRate / 60);
		return t;
	}
	// Initialise the game
	void Start() {
		m_Level = 0;
		m_Score = 0;
		m_TimeLeft = 1 / (m_InitSpawnRate / 60);
		MaxBugs = 5;
		m_LevelSpawned = false;
		m_ActualSpawnRate = m_InitSpawnRate;
		m_ScoreKeeper.HighScoreUpdate(PlayerPrefs.GetInt("HighScore"));
		m_ScoreKeeper.ScoreUpdate(m_Score);
	}
	// Assigns a target for the given bug
	private void AssignTarget(GameObject Bug) {
		Bug.GetComponent<BugMovement>().m_TargetObject = m_Target;
		Bug.GetComponent<BugController>().SetSpawner(this);

	}
	// Creates a bug at the generated SpawnLocation
	private void SpawnBug() {
		// Get a bug from the pool
		var bug = BugPool.Instance.Get();
		// Set the bug as a child of the spawner
		bug.gameObject.transform.SetParent(this.gameObject.transform);
		// Set the bugs spawner object
		bug.SetSpawner(this);
		// Set the bugs target
		bug.gameObject.GetComponent<BugMovement>().m_TargetObject = m_Target;
		// Activate the bug gameobject
		bug.gameObject.SetActive(true);
		// Set the position to the spawn location
		bug.transform.position = SpawnLocation;
		// Set the Bugs rotation
		bug.transform.rotation = transform.rotation;
		// Increment the number of bugs left to kill
		m_BugsLeft++;
		// increment the number of bugs spawned this level
		m_BugsSpawnedInLevel++;
	}
	// Called every frame
	void Update() {
		// If timer has finished counting down
		if(m_WaitTimer < 0f) {
			// If the level wave has finished spawning
			if(m_LevelSpawned) {
				// Go to the Next Level
				NextLevel();
			}
			// If the level wave has NOT finished spawning
			else {
				// Continue spawning the level wave
				SpawnWave();
			}
		}
		// Countdown the wait timer
		m_WaitTimer -= Time.deltaTime;
	}
	// Resets counters for next level, increments Level
	private void NextLevel() {
		// Incement Level		
		m_Level++;
		// Set wait timer to the level delay
		m_WaitTimer = m_TimeBetweenLevels;
		// Update the number bugs for the next level
		UpdateMaxBugs();
		// Reset the number of bugs spawned in this level
		m_BugsSpawnedInLevel = 0;
		// Wave has not been spawned
		m_LevelSpawned = false;
		// Send analytics for LevelIncrease
		GetComponent<AnalyticsEventTracker>().TriggerEvent();
	}
	// Manages the spawning of bugs during the level wave
	private void SpawnWave() {
		// If all bugs have been spawned for this wave	
		if(m_BugsSpawnedInLevel == MaxBugs) {
			// Wave has been spawned
			m_LevelSpawned = true;
		}
		// If need to spawn more bugs for this wave
		else {
			// Spawn a bug
			SpawnBug();
			// Update the Bug Timer
			m_WaitTimer = BugTimer();
		}
	}

	// Kills the bug
	public void BugKilled(GameObject bug, int scoreValue) {
		// Play the bug death sound
		GetComponent<AudioSource>().Play();
		// Decrease the bugs left on the screen
		m_BugsLeft--;
		// Increase the score according to the bug type killed
		m_Score += scoreValue;
		// TODO: Move score variable to the scorekeeper script
		// Update the scorekeeper
		m_ScoreKeeper.ScoreUpdate(m_Score);
		// If new high score
		if(PlayerPrefs.GetInt("HighScore") < m_Score) {
			// Update high score
			PlayerPrefs.SetInt("HighScore", m_Score);
			// Update the scorekeeper
			m_ScoreKeeper.HighScoreUpdate(m_Score);
		}
		// TODO: Return bug to pool
		BugPool.Instance.ReturnObjectsToPool(bug.GetComponent<BugController>());
	}
}
