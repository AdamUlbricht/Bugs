using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[SerializeField] private GameObject m_ObjectPrefab;
	public static ObjectPooler Instance { get; private set; }
	private Queue<GameObject> m_Objects = new Queue<GameObject>();

	private void Awake() {
		Instance = this;
	}
	// Allows the spawner to retrieve a objects from the queue
	public GameObject Get() {
		// If there are no objects in the queue
		if(m_Objects.Count == 0) {
			// Add a new object to the pool
			AddObjectToPool(1);
		}
		return m_Objects.Dequeue();
	}

	// Returns a bug to the pool
	public void ReturnObjectToPool(GameObject objectToReturn) {
		// Deactivate the object
		objectToReturn.SetActive(false);
		// Put the object back into the queue
		m_Objects.Enqueue(objectToReturn);
	}

	// Adds a bug to the queue
	private void AddObjectToPool(int count) {
		// Create a new object from prefab
		var newObject = GameObject.Instantiate(m_ObjectPrefab);
		// Set object to active
		newObject.SetActive(true);
		// Add object to the queue
		m_Objects.Enqueue(newObject);
		// 
		newObject.GetComponent<IGameObjectPooled>().Pool = this;
	}

}
