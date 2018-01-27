using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    public UpgradeTemplate myUpgrade;
   
    public Text titleField;
    public Text descriptionField;
    public Text miscTextField;


    public Image icon;
    public Button upgradeButton;
    public Slider upgradeLevelSlider;

	// Use this for initialization
	void Start () {
        titleField.text = myUpgrade.title;
        descriptionField.text = myUpgrade.descr;
        icon.sprite = myUpgrade.icon;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
