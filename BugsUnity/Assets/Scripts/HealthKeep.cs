using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthKeep : MonoBehaviour {
	public static int Health = 50;
	public GameObject VeggieHealth;
	private TextMeshProUGUI health;

	// Use this for initialization
	void Start () {
		Health = 50;
		health = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		health.SetText(Health.ToString());
		if (Health <= 0){
			SceneManager.LoadScene("Title");
		}
	}
	public void TakeDamage(int dmg)
	{
		Health -= dmg;
	}
}
