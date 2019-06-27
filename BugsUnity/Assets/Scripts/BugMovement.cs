using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement :MonoBehaviour {

	// Represents how fast the bug moves
	public float m_Speed;
	// Represents how fast the bug turns
	public float m_RotateSpeed;
	// Represents how far the bug turns
	public float m_TurnAmount;
	// Represents the target of the bug
	public GameObject m_TargetObject;
	// Individual offset for sin wave function
	public float m_SinOffset;

	private void Start() {
		transform.LookAt(m_TargetObject.transform);
		m_SinOffset = Random.Range(-1f, 1f);
	}

	public void ApplyTurn() {
		transform.LookAt(m_TargetObject.transform);
		float y = m_TurnAmount * Mathf.Sin((Time.time + m_SinOffset) * m_RotateSpeed);

		float actualRotation = transform.rotation.eulerAngles.y + y;

		Vector3 v = new Vector3(0, actualRotation, 0);


		transform.rotation = Quaternion.Euler(v);
	}
	public void ApplyMove() {
		transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
	}
	void Update() {
		if(!GetComponent<BugController>().Reached) {
			ApplyTurn();
			ApplyMove();
		}
	}
}
