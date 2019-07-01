using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Score Manager")]
public class ScoreManager : ScriptableObject
{
	private int m_Score;

	public void Initialize() {
		m_Score = 0;
		MenuController.Instance.UpdateScoreUI(m_Score.ToString());
		MenuController.Instance.UpdateHighScoreUI(PlayerPrefs.GetInt("HighScore").ToString());
	}

	/// <summary>
	/// Adds to the players score by the specified amount.
	/// </summary>
	/// <param name="amount">The amount to add to the players score.</param>
	public void AddScore(int amount) {
		m_Score += amount;
		MenuController.Instance.UpdateScoreUI(m_Score.ToString());
		if(PlayerPrefs.GetInt("HighScore") < m_Score) {
			PlayerPrefs.SetInt("HighScore", m_Score);
			MenuController.Instance.UpdateHighScoreUI(PlayerPrefs.GetInt("HighScore").ToString());
		}
	}

	// TODO: Update the score UI


}
