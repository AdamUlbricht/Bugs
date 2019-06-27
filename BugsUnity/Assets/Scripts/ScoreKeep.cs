using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreKeep :MonoBehaviour {
	//private Text score;
	private TextMeshProUGUI scoreTMP;

	// Use this for initialization
	void Start() {
		//score = GetComponent<Text> ();
		scoreTMP = gameObject.GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update() {
		scoreTMP.SetText(Spawner.m_Score.ToString());
	}
}
