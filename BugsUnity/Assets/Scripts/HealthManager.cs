using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Health Manager")]
public class HealthManager : ScriptableObject
{
	[SerializeField] private int m_InitialHealth;
	private int m_Health;

	public void Initialize() {
		InitializeHealth();
		MenuController.Instance.UpdateHealthUI(m_Health.ToString());
	}

	/// <summary>
	/// Reduces the players health by the given amount.
	/// </summary>
	/// <param name="amount">The amount of health to take away from the player.</param>
	public void DamageHealth(int amount) {
		m_Health -= amount;
		MenuController.Instance.UpdateHealthUI(m_Health.ToString());
		if(m_Health <= 0) {
			MenuController.Instance.GameOver();
		}
	}

	/// <summary>
	/// Resets the players health for a new game.
	/// </summary>
	public void InitializeHealth() {
		m_Health = m_InitialHealth;
	}

}
