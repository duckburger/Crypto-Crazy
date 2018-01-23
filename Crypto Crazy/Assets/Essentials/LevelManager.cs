using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}


    public void LoadALevel (int levelIndex)
    {
        if (levelIndex >= 0)
        {
            SceneManager.LoadScene(levelIndex);
        } else
        {
            Debug.Log("This scene index is too low!");
        }
    }
	
	// Update is called once per frame
	void Update () {

		
	}
}
