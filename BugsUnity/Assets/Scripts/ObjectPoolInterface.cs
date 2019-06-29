using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInterface : MonoBehaviour
{
	// 
	private ObjectPooler m_Pool;
	public ObjectPooler Pool {
		get { return m_Pool; }
		set {
			if (m_Pool == null) {
				m_Pool = value;
			}
			else {
				throw new System.Exception("Bad pool use, this should only get set once!");
			}
		}
	}
}

internal interface IGameObjectPooled {
	ObjectPooler Pool { get; set; }
}
