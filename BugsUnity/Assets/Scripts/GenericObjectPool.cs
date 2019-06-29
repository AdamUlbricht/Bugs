using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> :MonoBehaviour where T : Component {
	// The prefab to pool
	[SerializeField] private T m_Prefab;
	// Instance of pool
	public static GenericObjectPool<T> Instance { get; private set; }
	// The queue for pooled objects
	private Queue<T> m_PooledObjects = new Queue<T>();

	private void Awake() {
		Instance = this;
	}

	// Pulls objects out of the pool
	public T Get() {
		// If there are no objects left
		if(m_PooledObjects.Count == 0) {
			// Create a new object
			AddObjectsToPool(1);
		}
		// Return the object from the pool
		return m_PooledObjects.Dequeue();
	}

	// Returns objects to the pool
	public void ReturnObjectsToPool(T objectToReturn) {
		// Deactivate the object
		objectToReturn.gameObject.SetActive(false);
		// Put the object in the pool
		m_PooledObjects.Enqueue(objectToReturn);
	}

	// Adds new objects to the pool
	void AddObjectsToPool(int count) {
		// Check prefab is defined
		if(m_Prefab == null) {
			Debug.LogError("No Prefab defined for GenericObjectPool");
			return;
		}
		var newObject = GameObject.Instantiate(m_Prefab);
		// Deactivate the object
		newObject.gameObject.SetActive(false);
		// Put the object in the queue
		m_PooledObjects.Enqueue(newObject);
	}

}
