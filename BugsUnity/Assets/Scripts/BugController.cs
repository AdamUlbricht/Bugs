using Unity;
using UnityEngine;
using UnityScript;
using System.Collections;
using System;
[RequireComponent(typeof(BugMovement))]
public class BugController :MonoBehaviour {
	// TODO: Create remote settings for bug types
	// A reference to the bug manager scriptable object
	// NOTE: use the bug manager to initialise bugs when instantiated or taken from the pool.
	private ScoreManager m_ScoreManager;
	// A reference to the movement script attached to this bug
	private BugMovement BugMovement {
		get {
			return GetComponent<BugMovement>();
		}
	}
	public void Initialize(ScoreManager scoreManager, Spawner spawner, GameObject target) {
		BugMovement.SetTarget(target);
		m_Spawner = spawner;
		this.m_ScoreManager = scoreManager;
	}
	/// Represents whether or not this bug has reached its target destination
	private bool m_ArrivedAtTarget = false;
	/// Represents the time between dealing damage to the target
	[SerializeField] private float m_DamageTime = 1;
	/// Represents the amount of damage this bug deals to the target
	[SerializeField] private int m_DamageValue = 5;
	// Represents the number of points awarded to the player for killing this bug
	[SerializeField] private int m_ScoreValue = 1;
	// The timer for damage interval
	private float m_WaitTimer = 0f;
	// The spawner script
	private Spawner m_Spawner;
	// Used to detect collision with the vegie
	private Vegie vegie;
	// Initialise the bug when removed from the pool
	void OnEnable() {
		m_WaitTimer = 0f;
		m_ArrivedAtTarget = false;
		vegie = null;
	}
	// Returns true if this grub has been clicked on
	public bool CastRay {
		get {
			// Create a ray cast from input position 
			RaycastHit hitInfo = new RaycastHit();
			var hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			// If ray cast intersects this bug
			if(hitInfo.transform.gameObject == this.gameObject) {
				return true;
			}
			else
				return false;
		}
	}
	// When click or touch input is recieved
	public void OnMouseDown() {
		// If click or touch on a bug
		if(CastRay) {
			// Add the score
			m_ScoreManager.AddScore(m_ScoreValue);
			// Kill the bug
			m_Spawner.BugKilled(this.gameObject);
		}
	}
	// When colliding with the veige, reached is true
	void OnCollisionEnter(Collision collision) {
		// If there is a vegie component on the colliding object
		if(collision.gameObject.GetComponent<Vegie>() != null) {
			// Assign the vegie component
			vegie = collision.gameObject.GetComponent<Vegie>();
			// The bug has reached the vegie
			m_ArrivedAtTarget = true;
		}
	}
	// Applies damage to the colliding vegie
	void TargetReached() {
		// If countdown is complete
		if(m_WaitTimer < 0f) {
			// Reset the countdown
			m_WaitTimer = m_DamageTime;
			// If there is a vegie component 
			if(vegie != null) {
				// Tell the vegie to receive damaged
				vegie.Damage(m_DamageValue);
			}
		}
		// Countdown the wait timer
		m_WaitTimer -= Time.deltaTime;
	}
	// Update is called once per frame
	void Update() {
		// If bug has NOT reached the target
		if(!m_ArrivedAtTarget) {
			BugMovement.ApplyTurn();
			BugMovement.ApplyMove();
		}
		else {
			TargetReached();
		}
	}
	internal void SetTransform(Vector3 spawnLocation, Quaternion rotation) {
		transform.position = spawnLocation;
		transform.rotation = rotation;
	}
}
