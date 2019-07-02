using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class MenuController : MonoBehaviour
{
	public static MenuController Instance { get; private set; }

	public void Awake() {
		if(Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}
	[SerializeField] private GameObject m_PauseUI;
	[SerializeField] private GameObject m_GameOverUI;
	[SerializeField] private GameObject m_GameUI;
	[SerializeField] private GameObject m_TiteUI;
	[SerializeField] private TextMeshProUGUI m_HealthText;
	[SerializeField] private TextMeshProUGUI m_ScoreText;
	[SerializeField] private TextMeshProUGUI m_HighScoreText;


	public void Resume() {
		Time.timeScale = 1f;
		m_PauseUI.SetActive(false);
	}

	public void Pause() {
		Time.timeScale = 0f;
		m_PauseUI.SetActive(true);
	}

	public void GameOver() {
		Time.timeScale = 0f;
		m_GameOverUI.SetActive(true);
	}

	public void LoadNewGame() {
		Time.timeScale = 1f;
		m_GameOverUI.SetActive(false);
		m_PauseUI.SetActive(false);
		m_GameUI.SetActive(true);
		m_TiteUI.SetActive(false);
		SceneManager.LoadScene(1);
	}

	public void LoadTitle() {
		m_GameOverUI.SetActive(false);
		m_PauseUI.SetActive(false);
		m_GameUI.SetActive(false);
		m_TiteUI.SetActive(true);
		if(SceneManager.GetActiveScene().buildIndex != 0) {
			SceneManager.LoadScene(0);
		}
	}

	public void UpdateHealthUI(string t) {
		m_HealthText.SetText(t);
	}
	public void UpdateScoreUI(string t) {
		m_ScoreText.SetText(t);
	}
	public void UpdateHighScoreUI(string t) {
		m_HighScoreText.SetText(t);
	}
}
