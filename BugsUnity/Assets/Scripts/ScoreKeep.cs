using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreKeep :MonoBehaviour {
	public TextMeshProUGUI m_scoreTMP;
	public TextMeshProUGUI m_HighScoreTMP;
	public int m_Score;
	public int m_HighScore;

	public void ScoreUpdate(int score) {
		m_scoreTMP.SetText(score.ToString());
		m_Score = score;
	}

	public void HighScoreUpdate(int score) {
		m_HighScoreTMP.SetText(score.ToString());
		m_HighScore = score;
	}
}
