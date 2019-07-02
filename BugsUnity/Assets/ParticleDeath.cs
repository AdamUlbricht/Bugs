using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeath : MonoBehaviour
{
	private ParticleSystem ps;
	private void OnEnable() {
		ps = GetComponent<ParticleSystem>();
	}
	private void Update() {
		if(ps.isStopped) {
			ParticlePool.Instance.ReturnObjectsToPool(ps);
		}
	}
}
