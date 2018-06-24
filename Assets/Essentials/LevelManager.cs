using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour {

    [SerializeField] MiningControllerTemplate miningController;
    [SerializeField] TMP_InputField currencyNameInputField;

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
            SimpleNotificationSystem.Instance.QueueNotification("This scene index is too low!");
            return;
        }
    }
	
    public void CheckNameAndStartNewGame()
    {
        if (miningController.currencyName.Length <= 2)
        {
            // TODO: show a notification of some sort
            SimpleNotificationSystem.Instance.QueueNotification("The name must be at least 3 characters long.");
            return;
        }

        char[] convertedName = miningController.currencyName.ToCharArray();
        // Check for spaces in the string and if there are any then don't proceed
        for (int i = 0; i < convertedName.Length; i++)
        {
            if (char.IsWhiteSpace(convertedName[i]))
            {
                SimpleNotificationSystem.Instance.QueueNotification("Please enter a currency name.");
                return;
            }
        }

        LoadALevel(1);
    }

	// Update is called once per frame
	void Update () {

		
	}
}
