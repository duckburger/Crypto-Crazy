using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RigUI : MonoBehaviour {

    public bool isEnabled;
    public Image darkOverlay;
    
    // The number of the rig I am monitoring (and sending commands to)
    public int myRigNumber;

    // The rack I am currently monitoring (and sending commands to)
    public int myRackNumber;

    // Determines whether I control the rig or the rack
    public bool controllingRack;

    public int racksControlled;


    // All the text objects we will need to adjust every upgrade 
    public Text title;
    public Text description;
    public Text miscText;
    public Text rigsControlled;

    // We need this to update the rig icon on the left
    public Image icon;

    // Needs this to adjust the button properties every click
    public Button upgradeButton;

    // For changing the text of the button
    public Text buttonText;

    // For controlling the upgrade level bar
    public Image upgradeLvlUI;

    // Show the player what is the current effect of this rig
    public Text currentEffectText;



    // Use this for initialization
    void Start () {
       

    }

    public void OnEnable()
    {
        if (!isEnabled)
        {

            title.text = "???";
            description.text = "???";
            miscText.text = "???";
            icon = null;
            buttonText.text = "???";
            rigsControlled.text = "???";
            upgradeLvlUI.fillAmount = 0;

            darkOverlay.gameObject.SetActive(true);

        } else
        {
            darkOverlay.gameObject.SetActive(false);
        }
    }


    public void InitizalizeTheUI()
    {
      
    }



	
	// Update is called once per frame
	void Update () {
		
	}
}
