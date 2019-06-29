using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BugController))]
public class BugMovement :MonoBehaviour {
	// Represents how fast the bug moves
	[SerializeField] private float m_Speed = 5f;
	// Represents how fast the bug turns
	[SerializeField] private float m_RotateSpeed = 4f;
	// Represents how far the bug turns
	[SerializeField] private float m_TurnAmount = 15f;
	// Represents the target of the bug
	public GameObject m_TargetObject;
	// Individual offset for sin wave function
	[SerializeField] private float m_SinOffset = 0f;

	private void Start() {
		// Assign a random sine wave offset
		m_SinOffset = Random.Range(-1f, 1f);
	}
	// Applies rotation to the bug
	public void ApplyTurn() {

		// TODO: calculate turn amount using difference between current rotation and a debug ray from bug to target

		// Makes the bug point at the target object
		transform.LookAt(m_TargetObject.transform);
		// Calculates the turn amount, incorporates a sine wave
		float y = m_TurnAmount * Mathf.Sin((Time.time + m_SinOffset) * m_RotateSpeed);
		// Adds the turn amount to the current rotation
		float actualRotation = transform.rotation.eulerAngles.y + y;
		// Applies the rotation value to the transform component
		transform.rotation = Quaternion.Euler(new Vector3(0, actualRotation, 0));
	}
	// Applies the movement to the bug
	public void ApplyMove() {
		// Push the bug in the forward direction
		transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
	}
}
