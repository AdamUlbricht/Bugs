using UnityEngine;
using System.Collections;

public class Vegie :MonoBehaviour {
	// Defines whether the vegie is receiving damage
	private bool m_RecievedDamage;
	// Number of times to swap the material when receiving damage
	[SerializeField] private int m_TimesToSwap = 3;
	// Keeps track of how many times materail has been swapped
	private int m_SwapCounter;
	// The material to show damage
	public Material m_DamagedMaterial;
	// The regular material of the object
	private Material m_DefaultMaterial;
	// Wait timer for changing material
	private float m_WaitTimer;
	// Defines the delay between material swaps
	public float m_MaterialSwapDelay;
	// The list of Renderers to change the materials of
	public MeshRenderer[] m_MeshRendererToChange;
	// Keeps track of if the material is regular or not
	private bool m_MaterialIsDefault;
	// A reference to the HealthKeeper script
	public HealthKeep m_HealthKeeper;

	// Use this for initialization
	void Start() {
		m_RecievedDamage = false;
		m_SwapCounter = 0;
		m_MaterialIsDefault = true;

		m_DefaultMaterial = m_MeshRendererToChange[0].material;
	}

	// Update is called once per frame
	void Update() {
		// If vegie is hurt
		if(m_RecievedDamage == true) {
			// If wait timer is finished
			if(m_WaitTimer <= 0) {
				// Reset the wait timer
				m_WaitTimer = m_MaterialSwapDelay;
				// For each material to change
				for(int i = 0; i < m_MeshRendererToChange.Length; i++) {
					// If the material of the renderer is normal
					if(m_MaterialIsDefault == true) {
						// Change it to damaged
						m_MeshRendererToChange[i].material = m_DamagedMaterial;
						// Renderer no longer has regular material
						m_MaterialIsDefault = false;
					}
					// If the material is damaged
					else {
						// Change it to normal
						m_MeshRendererToChange[i].material = m_DefaultMaterial;
						// Renderer now has regular material
						m_MaterialIsDefault = true;
						// Increment the swap counter
						++m_SwapCounter;
					}
				}
			}
			// Countdown the wait timer
			m_WaitTimer -= Time.deltaTime;
			// If material has changed enough times
			if(m_SwapCounter == m_TimesToSwap) {
				// Vegie is no longer hurt
				m_RecievedDamage = false;
				// Reset the swap counter
				m_SwapCounter = 0;
			}
		}
	}

	public void Damage(int dmgValue) {
		if(m_RecievedDamage == false) {
			m_HealthKeeper.TakeDamage(dmgValue);
			m_RecievedDamage = true;
		}
	}

}
