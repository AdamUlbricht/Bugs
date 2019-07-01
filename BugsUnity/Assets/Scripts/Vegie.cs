using UnityEngine;
using System.Collections;

public class Vegie :MonoBehaviour {
	[SerializeField] private HealthManager m_HealthManager;
	// Number of times to swap the material when receiving damage
	[SerializeField] private int m_TimesToSwap = 3;
	// The material to show damage
	[SerializeField] private Material m_DamagedMaterial;
	// Defines the delay between material swaps
	[SerializeField] private float m_MaterialSwapDelay;
	// The list of Renderers to change the materials of
	[SerializeField] private MeshRenderer[] m_MeshRendererToChange;
	// Defines whether the vegie is receiving damage
	private bool m_RecievedDamage;
	// Keeps track of how many times materail has been swapped
	private int m_SwapCounter;
	// The regular material of the object
	private Material m_DefaultMaterial;
	// Wait timer for changing material
	private float m_WaitTimer;
	// Keeps track of if the material is regular or not
	private bool m_MaterialIsDefault;

	// Use this for initialization
	void Start() {
		m_RecievedDamage = false;
		m_SwapCounter = 0;
		m_MaterialIsDefault = true;
		m_DefaultMaterial = m_MeshRendererToChange[0].material;
		m_HealthManager.Initialize();
	}

	// Update is called once per frame
	void Update() {
		// If vegie is hurt
		if(m_RecievedDamage == true) {
			DisplayDamage();
		}
	}

	private void SwapMaterials() {
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

	private void DisplayDamage() {
		// If wait timer is finished
		if(m_WaitTimer <= 0) {
			// Reset the wait timer
			m_WaitTimer = m_MaterialSwapDelay;
			SwapMaterials();
		}
		// If material has changed enough times
		if(m_SwapCounter == m_TimesToSwap) {
			// Vegie is no longer hurt
			m_RecievedDamage = false;
			// Reset the swap counter
			m_SwapCounter = 0;
		}
		// Countdown the wait timer
		m_WaitTimer -= Time.deltaTime;
	}


	public void Damage(int dmgValue) {
		if(m_RecievedDamage == false) {

			m_HealthManager.DamageHealth(dmgValue);
			m_RecievedDamage = true;
		}
	}

}
