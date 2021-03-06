﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour {

    public UpgradeTemplate myUpgrade;
    public MiningControllerTemplate myMiningController;
    public MiningController minContr;
    public MapController mapController;
    public GameEvent activationEvent;
   
    public TextMeshProUGUI titleField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI miscTextField;

    public Image icon;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public Image upgradeLevelUI;
    public TextMeshProUGUI currentEffectText;

    public float pricePercentageGrowth;

    // Use this for initialization
    void Start()
    {
        minContr = FindObjectOfType<MiningController>();
        ResetDataForNewApartment();       
    }

    public void ResetDataForNewApartment()
    {
        mapController = FindObjectOfType<MapController>();
        RefreshUI();

    }

    float CalculatePriceOfNextBuilding(float initialCost, float costBase, float currentUpgrLvl)
    {
        // cost = first item cost * costBase ^ amount of this upgrade currently owned
        return initialCost * Mathf.Pow(costBase + currentUpgrLvl / 3, currentUpgrLvl + 1);
    }

    private void RefreshUI()
    {
        titleField.text = myUpgrade.title;
        descriptionField.text = myUpgrade.descr;
        if (myUpgrade.icon)
        {
            icon.sprite = myUpgrade.icon;
        }
        else
        {
            icon.sprite = null;
        }

        upgradeButton.interactable = true;
        buttonText.text = "BUY" + "\n" + myUpgrade.priceOfNextUpgradeLvl;

        upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;

        if (myUpgrade.primaryListOfEffects.Count == 0 && myUpgrade.secondaryListOfEffects.Count == 0)
        {
            currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
            miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
        }
        else
        {
            currentEffectText.text = myUpgrade.GenerateCurrentEffectTextAnnouncement();
            miscTextField.text = myUpgrade.GenerateNextUpgradeTextAnnouncement();
        } 
    }

    // TODO: Pull the ui updating methods out of here

    // This is assigned to the button through the inspector!
    public void PurchaseTheUpgrade()
    {
            if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currentBalance && myUpgrade.currentUpgradeLvl < myUpgrade.maxUpgradeLvl)
            {
                // Charging for the upgrade
                myMiningController.currentBalance -= myUpgrade.priceOfNextUpgradeLvl;
                // Calculate the growth of the upgrade price
                myUpgrade.priceOfNextUpgradeLvl = CalculatePriceOfNextBuilding(myUpgrade.defPrOfNxtUpgLvl, myUpgrade.costBase, myUpgrade.currentUpgradeLvl);
                myUpgrade.currentUpgradeLvl++;
                // APPLYING THE UPGRADE EFFECTS HERE
                ApplyMyEffectsToGame(myUpgrade.myApplications, myUpgrade.buildingsISpawn);
                upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;

            // IF THIS UPGRADE ONLY BUILDS STUFF...
            if (myUpgrade.myApplications.Count <= 0)
            {
            // Updating the text for the current and next effect if this is a building only upgrade
                currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
                miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
            }
            else
            {   // ...IF THIS UPGRADE ALSO AFFECTS ATTRIBUTES
                // If this has been the last upgrade then turn the button off and make it gray
                if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
                {
                    miscTextField.text = myUpgrade.GenerateNextUpgradeTextAnnouncement();
                    upgradeButton.interactable = false;
                    buttonText.text = "MAX UPGRADE PURCHASED";
                    //Debug.Log("Turnt off the " + myUpgrade.title + " button");
                    upgradeButton.GetComponent<Image>().color = Color.gray;
                    return;
                }
                currentEffectText.text = myUpgrade.GenerateCurrentEffectTextAnnouncement();
                miscTextField.text = myUpgrade.GenerateNextUpgradeTextAnnouncement();    
            }
            
                // Check if after applying this upgrade we've reached the maximum one
                if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
                {

                // Turn off the button if we reached max upgrade
                    upgradeButton.interactable = false;
                    upgradeButton.GetComponent<Image>().color = Color.gray;
                    return;
                }
                // Update the text on the button to display the price of the next step
                buttonText.text = "BUY" + "\n" + NumberConverter.ConvertNumber(myUpgrade.priceOfNextUpgradeLvl);
            }
            else
            {
                Debug.Log("Not enough money for this upgrade right now!");
                return;
            }
        } 
        
    public void ApplyMyEffectsToGame(List<Attribute> myAttributes, List<Building> myBuildings)
    {
        //Debug.Log("Registered a button press on " + myUpgrade.title);
        if (myAttributes.Count > 0)
        {
            // 0 - Mining speed
            // 1 - Cooling timer
            foreach(Attribute attribute in myAttributes)
            {
                // Mining speed
                if (attribute.id == 0)
                {  
                    minContr.IncreaseMinMaxMiningSpeed(myUpgrade.primaryListOfEffects[myUpgrade.currentUpgradeLvl]);
                }
                // Dust timer
                if (attribute.id == 1)
                {
                    minContr.AddTimeToDustTimer(myUpgrade.secondaryListOfEffects[myUpgrade.currentUpgradeLvl]);
                }
                // Apply a random doubler or a rig/rack
                if (attribute.id == 2)
                {
                    minContr.DoubleARandomRigRack();
                }

                
            }
        }

        if (myBuildings.Count > 0)
        {
            foreach(Building building in myBuildings)
            {
                Debug.Log("I am spawning a building!");
                mapController.SpawnAnItem(building);
                continue;
            }

           
        }
    }
     
	// Update is called once per frame
	void Update () {

        if (myUpgrade.currentUpgradeLvl < myUpgrade.maxUpgradeLvl)
        {
            if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currentBalance)
            {
                upgradeButton.GetComponent<Image>().color = myMiningController.positiveColor;
            }
            else
            {
                upgradeButton.GetComponent<Image>().color = Color.gray;
            }
        }
		
	}
}
