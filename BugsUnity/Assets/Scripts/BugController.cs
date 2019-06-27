using Unity;
using UnityEngine;
using UnityScript;
using System.Collections;
using System;

public class BugController :MonoBehaviour {
	/// <summary>
	/// Represents whether or not this bug has reached its target destination
	/// </summary>
	[SerializeField] public bool Reached;
	/// <summary>
	/// Represents the time between dealing damage to the target
	/// </summary>
	[SerializeField] public float DamageTime;
	/// <summary>
	/// Represents the amount of damage this bug deals to the target
	/// </summary>
	[SerializeField] public int DamageValue;
	[SerializeField] public int ScoreValue;
	public float timer;
	Vegie vegie;
	public GameObject SpawnerObject;
	private Spawner m_Spawner;
	// Use this for initialization
	void Start() {
		m_Spawner = SpawnerObject.GetComponent<Spawner>();
		Reached = false;
		vegie = null;
	}
	/// <summary>
	/// Returns true if this grub has been clicked on
	/// </summary>
	public bool CastRay {
		get {
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if(hitInfo.transform.gameObject == this.gameObject) {
				return true;
			}
			else
				return false;
		}
	}

	public void OnMouseDown() {
		if(CastRay) {
			//SpawnerObject.GetComponent<AudioSource>().Play();
			GetComponent<AudioSource>().Play();
			m_Spawner.m_BugsLeft--;
			Spawner.m_Score += ScoreValue;
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		vegie = collision.gameObject.GetComponent<Vegie>();
		if(vegie) {
			Reached = true;
		}
	}
	
	private void OnCollisionExit(Collision collision) {
		vegie = collision.gameObject.GetComponent<Vegie>();
		if(vegie) {
			Reached = false;
		}
	}

	void ApplyDamage() {
		timer -= Time.deltaTime;
		if(timer < 0f) {
			if(vegie != null)
				vegie.Damage(DamageValue);
			timer = DamageTime;
		}
	}

	// Update is called once per frame
	void Update() {
		if(Reached)
			ApplyDamage();
	}
}
