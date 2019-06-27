using UnityEngine;
using UnityEngine.Analytics;
using UnityEditor.Analytics;
using System.Collections;
using System;

public class Spawner :MonoBehaviour {
	
	// Inspector 
	/// <summary>
	/// Represents the number of bugs per minute
	/// </summary>
	[SerializeField] private float m_InitSpawnRate;
	[SerializeField] private float m_Gradient;
	[SerializeField] private float m_TimeBetweenLevels;
	[SerializeField] private GameObject m_Bug;
	[SerializeField] private GameObject m_LimitMax;
	[SerializeField] private GameObject m_LimitMin;

	// Private 
	private float m_TimeLeft;
	private float m_BugTimer;
	private float m_LevelTimer;
	private float m_ActualSpawnRate;
	private bool m_LevelSpawned;
	private int m_BugsSpawnedInLevel;


	// Public 
	public GameObject m_Target;
	public int m_BugsLeft;
	public static int m_Score;
	public static int m_Level;
	public int MaxBugs { get; set; }

	/// <summary>
	/// Generates a random position between the defined position limits
	/// </summary>
	private Vector3 SpawnLocation {
		get {
			Vector3 vector3 = gameObject.transform.position;
			vector3.x += UnityEngine.Random.Range(m_LimitMin.transform.position.x, m_LimitMax.transform.position.x);
			return vector3;
		}
	}
	/// <summary>
	/// Updates the maximum number buugs based off the current level
	/// </summary>
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
	/// <summary>
	/// Updates the wait timer between levels
	/// </summary>
	private void UpdateLevelTimer() {
		m_LevelTimer = m_TimeBetweenLevels;
	}
	/// <summary>
	/// Updates the wait timer between bugs
	/// </summary>
	private void UpdateBugTimer() {
		// Generates a time between each bug, based off of Bugs Per Minute
		m_ActualSpawnRate = m_InitSpawnRate + m_Level * (10 / m_Gradient);
		m_BugTimer = 1 / (m_ActualSpawnRate / 60);
	}
	// Initialise the game
	void Start() {
		
		m_Level = 0;
		m_Score = 0;
		m_TimeLeft = 1 / (m_InitSpawnRate / 60);
		MaxBugs = 5;
		m_LevelSpawned = false;
		m_ActualSpawnRate = m_InitSpawnRate;
		m_LevelTimer = m_TimeBetweenLevels;
	}
	/// <summary>
	/// Assigns a target for the given bug
	/// </summary>
	/// <param name="Bug"></param>
	private void AssignTarget(GameObject Bug) {
		Bug.GetComponent<BugMovement>().m_TargetObject = m_Target;
		Bug.GetComponent<BugController>().SpawnerObject = this.gameObject;
	}
	/// <summary>
	/// Creates a bug at the generated SpawnLocation
	/// </summary>
	private void SpawnBug() {
		// TODO: Change from instantiate to pooler
		// Debug.Log("Making a bug appear now!");
		// Instantiate a new bug gameobject
		GameObject NewGrub = Instantiate(m_Bug, SpawnLocation, transform.rotation);
		m_BugsLeft++;
		m_BugsSpawnedInLevel++;
		// Assign the target
		AssignTarget(NewGrub);
	}
	// Called every frame
	void Update() {
		if(!m_LevelSpawned) {
			SpawnWave();
		}
		else if(m_BugsLeft == 0) { // When all bugs have been defeated
			//Debug.Log("All bugs defeated");
			m_LevelTimer = m_LevelTimer - Time.deltaTime; // Count down to next wave
		}
		if(m_LevelTimer < 0f && m_BugsLeft == 0) { // When done counting down to next wave
			Debug.Log("Level countdown complete");
			NextLevel();
		}
		// Debug.Log("level timer: " + m_LevelTimer);
	}

	/// <summary>
	/// Resets counters for next level, increments Level
	/// </summary>
	private void NextLevel() {
		UpdateMaxBugs();
		m_BugsSpawnedInLevel = 0;
		m_LevelSpawned = false; // Wave has not been spawned
		UpdateLevelTimer();
		GetComponent<AnalyticsEventTracker>().TriggerEvent();
	}

	private void SpawnWave() {
		if(m_BugsSpawnedInLevel == MaxBugs) { // If all bugs have been spawned for this wave	
			Debug.Log("All bugs spawned");
			m_LevelSpawned = true; // Wave has been spawned			
			m_Level++; // Incement Level
			
		}
		// If need to spawn more bugs this wave
		else if(m_BugTimer > 0f) { // If Bug Timer is still counting
			m_BugTimer = m_BugTimer - Time.deltaTime;// Count down to next bug
		}
		else { // If bug timer is done counting			   
			Debug.Log("Bug timer done, spawning new bug");
			SpawnBug();// Spawn a bug
			UpdateBugTimer();// Update the Bug Timer
		}
	}
}
