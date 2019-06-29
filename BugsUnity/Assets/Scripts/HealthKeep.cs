using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class HealthKeep :MonoBehaviour {
	// Defines the starting health for a new game
	[SerializeField] private int m_StartingHealth = 50;
	// The current health
	public int Health = 50;
	// The health ui display
	private TextMeshProUGUI m_HealthDisplay;
	// Event tracker for death event
	[SerializeField] public AnalyticsEventTracker m_ET_Death;

	// Use this for initialization
	void Start() {
		// Set the current health to the starting health value
		Health = m_StartingHealth;
		m_HealthDisplay = GetComponent<TextMeshProUGUI>();
	}

	void UpdateHealth() {
		// Update the health display
		m_HealthDisplay.SetText(Health.ToString());
	}

	// Update is called once per frame
	void Update() {
		// If theres no health left
		if(Health <= 0) {
			// TODO: Create Game Over Screen
			// Trigger the death event
			m_ET_Death.TriggerEvent();
			// Load the title screen
			SceneManager.LoadScene("Title");
		}
	}
	public void TakeDamage(int dmg) {
		Health -= dmg;
		UpdateHealth();
	}
}
