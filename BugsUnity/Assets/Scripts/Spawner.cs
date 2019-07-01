using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System;

public class Spawner :MonoBehaviour {
	// A list containing all Bug prefabs
	[SerializeField] private GameObject[] m_Bug;
	// A reference to the BugManager
	[SerializeField] private ScoreManager m_ScoreManager;
	// A reference to the bug death sound effect
	[SerializeField] private AudioSource m_BugDeathSFX;

	#region Spawning Settings
	// Represents the number of bugs per minute
	[SerializeField] private float m_InitSpawnRate = 50f;
	// Represents the initial maximum number of bugs for the first level
	[SerializeField] private int m_InitMaxBugs = 5;
	// Use this to adjust the difference in maxbugs per level
	[SerializeField] private float m_Gradient = 1.5f;
	// The amount of time given to the player to rest between waves of bugs
	[SerializeField] private float m_TimeBetweenLevels = 2f;
	// These game objects limit the x value of the spawn position
	[SerializeField] private GameObject m_LimitMax;
	[SerializeField] private GameObject m_LimitMin;
	// The number of bugs left in the game
	public int m_BugsLeft;
	// The maximum number of bugs for the current level
	public int CurrentMaxBugs { get; set; }
	private float m_WaitTimer;
	private float m_TimeLeft;
	private float m_ActualSpawnRate;
	private bool m_LevelSpawned;
	private int m_BugsSpawnedInLevel;
	#endregion

	// The target object which the bugs will move towards
	public GameObject m_Target;
	// The current score
	public static int m_Score;
	// The current level
	public static int m_Level;

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
				return new Vector3(0, 0, 0);
			}
		}
	}

	/// <summary>
	/// Updates the maximum number buugs based off the current level.
	/// </summary>
	/// <param name="currentMax">The current maximum number of bugs.</param>
	/// <returns>Returns the new maximum number of bugs as an int.</returns>
	private int MaxBugs(int currentMax) {
		int newMax;
		// If current level is a multiple of 5
		if(m_Level % 5 == 0) {
			newMax = m_Level * 10;
		}
		// If current level + 1 is a multiple of 5
		else if((m_Level + 1) % 5 == 0) {
			newMax = currentMax + 10;
		}
		// If any other level
		else {
			newMax = currentMax + 5;
		}
		return newMax;
	}

	/// <summary>
	/// Returns a new wait timer value for the delay between bugs as a float
	/// </summary>
	/// <param name="initial">True returns a new delay based on initial values. False or Null will return a new delay based on previous delay.</param>
	/// <returns>Return new wait timer value</returns>
	private float NewBugDelay(bool initial = false) {
		if(initial) {
			float t = 1 / (m_InitSpawnRate / 60);
			return t;
		}
		else {
			// Generates a time between each bug, based off of Bugs Per Minute
			m_ActualSpawnRate = m_InitSpawnRate + m_Level * (10 / m_Gradient);
			float t = 1 / (m_ActualSpawnRate / 60);
			return t;
		}
	}

	// Initialise the game
	void Start() {
		Initialize();
	}

	private void Initialize() {
		// Reset the level
		m_Level = 0;
		// Reset the score
		m_Score = 0;
		// Initialize the wait timer
		m_WaitTimer = NewBugDelay(true);
		// initialize the max number of bugs
		CurrentMaxBugs = m_InitMaxBugs;
		// Level has not been spawned
		m_LevelSpawned = false;
		// Initialise the spawn rate
		m_ActualSpawnRate = m_InitSpawnRate;
		m_ScoreManager.Initialize();
	}

	// Creates a bug at the generated SpawnLocation
	private void SpawnBug() {
		// Get a bug from the pool
		var bug = BugPool.Instance.Get();
		// Activate the bug gameobject
		bug.gameObject.SetActive(true);
		// Initialise the scoremanager and spawner for the bug
		bug.Initialize(m_ScoreManager, this, m_Target);
		// Set transforms of the bug
		bug.SetTransform(SpawnLocation, transform.rotation);
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
				SpawnLevel();
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
		CurrentMaxBugs = MaxBugs(CurrentMaxBugs);
		// Reset the number of bugs spawned in this level
		m_BugsSpawnedInLevel = 0;
		// Level has not been spawned
		m_LevelSpawned = false;
		// Send analytics for LevelIncrease
		GetComponent<AnalyticsEventTracker>().TriggerEvent();
	}

	// Manages the spawning of bugs during the level 
	private void SpawnLevel() {
		// If all bugs have been spawned for this level	
		if(m_BugsSpawnedInLevel == CurrentMaxBugs) {
			// Level has been spawned
			m_LevelSpawned = true;
		}
		// If need to spawn more bugs for this wave
		else {
			// Spawn a bug
			SpawnBug();
			// Update the Bug Timer
			m_WaitTimer = NewBugDelay();
		}
	}

	// Kills the bug
	public void BugKilled(GameObject bug) {
		// Play the bug death sound
		m_BugDeathSFX.Play();
		// Decrease the bugs left on the screen
		m_BugsLeft--;
		// Return bug to pool
		BugPool.Instance.ReturnObjectsToPool(bug.GetComponent<BugController>());
	}
}
