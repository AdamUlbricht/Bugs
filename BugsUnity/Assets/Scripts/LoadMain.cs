using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadMain : MonoBehaviour {

	// Use this for initialization
	public void DoTheLoad () {
		SceneManager.LoadScene("Main",UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

}
