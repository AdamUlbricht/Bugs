using UnityEngine;
using System.Collections;

public class Vegie :MonoBehaviour {

	public bool AmIHurt;
	private int SwapCounter;
	public Material dmg_Mat;
	public Material reg_Mat;
	private MeshRenderer my_renderer;
	private float materialTimer;
	public float materialTime;
	public GameObject[] ChangeMe;
	private bool regular;
	public GameObject HealthKeeper;

	// Use this for initialization
	void Start() {
		AmIHurt = false;
		SwapCounter = 0;
		regular = true;
	}

	// Update is called once per frame
	void Update() {
		if(AmIHurt == true) {
			if(materialTimer <= 0) {
				//swap the shader
				//Debug.Log("Swap the material");
				materialTimer = materialTime;
				++SwapCounter;
				for(int i = 0; i < ChangeMe.Length; i++) {
					my_renderer = ChangeMe[i].GetComponent<MeshRenderer>();
					if(regular == true) {
						my_renderer.material = dmg_Mat;
					}
					else {
						my_renderer.material = reg_Mat;
					}
				}
				regular = !regular;
			}
			materialTimer -= Time.deltaTime;

			if(SwapCounter == 6) {
				AmIHurt = false;
				SwapCounter = 0;
			}
		}
	}

	public void Damage(int dmgValue) {
		if(AmIHurt == false) {
			HealthKeeper.GetComponent<HealthKeep>().TakeDamage(dmgValue);
			AmIHurt = true;
		}
	}

}
