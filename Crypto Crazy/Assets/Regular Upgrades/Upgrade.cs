using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    public UpgradeTemplate myUpgrade;
    public MiningControllerTemplate myMiningController;
    public MiningController minContr;
    public MapController mapController;
    public GameEvent activationEvent;
   
    public Text titleField;
    public Text descriptionField;
    public Text miscTextField;


    public Image icon;
    public Button upgradeButton;
    public Text buttonText;
    public Image upgradeLevelUI;
    public Text currentEffectText;

    public float pricePercentageGrowth;

    [SerializeField]
    private float tempEffectPercentage;
    // Use this for initialization
    void Start() {

       

        minContr = FindObjectOfType<MiningController>();
        mapController = FindObjectOfType<MapController>();

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

        buttonText.text = "BUY" + "\n" + myUpgrade.priceOfNextUpgradeLvl;

        upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;

        // TODO: Figure out a better way to make this tell what to display in the MISC TEXT and the UPGRADE LVL
        if (myUpgrade.primaryListOfEffects.Count == 0)
        {
            currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
            miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
        } else
        {
            tempEffectPercentage = myUpgrade.primaryListOfEffects[(int)myUpgrade.currentUpgradeLvl];
            currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
            miscTextField.text = myUpgrade.miscText + " " + myUpgrade.primaryListOfEffects[(int)myUpgrade.currentUpgradeLvl + 1] + "% mining speed";

            // Here we have to check whether the current upgrade is making use of the second property list
            if (myUpgrade.secondaryListOfEffects.Count > 0)
            {
                miscTextField.text += "\n" + "+" + myUpgrade.secondaryListOfEffects[myUpgrade.currentUpgradeLvl] + " secs to dust timer";
            }

        }
    }


    // TODO: Pull the ui updating methods out of here
    public void PurchaseTheUpgrade()
    {
            if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currencyMined && myUpgrade.currentUpgradeLvl < myUpgrade.maxUpgradeLvl)
            {
                myMiningController.currencyMined -= myUpgrade.priceOfNextUpgradeLvl;
               
                // TODO: Try some other math here
                // Control of the growth of the upgrade price
                myUpgrade.priceOfNextUpgradeLvl += (myUpgrade.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
                pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);
                myUpgrade.currentUpgradeLvl++;

                // APPLYING THE UPGRADE EFFECTS HERE
                ApplyMyEffectsToGame(myUpgrade.attributesIAffect, myUpgrade.buildingsISpawn);

                

                upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;

            // IF THIS UPGRADE ONLY BUILDS STUFF...
            if (myUpgrade.attributesIAffect.Count <= 0)
                {
                // Updating the text for the current and next effect if this is a building only upgrade
                    currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
                    miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
                }

                // ...IF THIS UPGRADE ALSO AFFECTS ATTRIBUTES
                else
                {
                    // If this has been the last upgrade then turn the button off and make it gray
                    if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
                    {
                        miscTextField.text = "MAX UPGRADE REACHED!";
                        upgradeButton.interactable = false;

                        //Debug.Log("Turnt off the " + myUpgrade.title + " button");
                        upgradeButton.GetComponent<Image>().color = Color.gray;
                        return;
                    }
                // This calculates the next effect of the upgrade and display it in the UI
                tempEffectPercentage += myUpgrade.primaryListOfEffects[myUpgrade.currentUpgradeLvl];
                currentEffectText.text = "+ " + (tempEffectPercentage).ToString() + "%";
                    
                    
                    if (myUpgrade.currentUpgradeLvl < myUpgrade.primaryListOfEffects.Count)
                    {
                        miscTextField.text = myUpgrade.miscText + " " + myUpgrade.primaryListOfEffects[myUpgrade.currentUpgradeLvl] + "% mining speed";
                        
                        // We have to check whether the current upgrade is making use of the second property list
                        if (myUpgrade.secondaryListOfEffects.Count > 0)
                        {
                        // TODO: make this independent
                        miscTextField.text += "\n" + "+" + myUpgrade.secondaryListOfEffects[myUpgrade.currentUpgradeLvl] + " secs to dust timer";
                        }

                    } else
                    {
                        miscTextField.text = "MAX UPGRADE REACHED!";

                        upgradeButton.interactable = false;

                        //Debug.Log("Turnt off the " + myUpgrade.title + " button");
                        upgradeButton.GetComponent<Image>().color = Color.gray;


                        Debug.Log("You've reached the maximum level for this upgrade right now");
                        return;
                    }

                }
            
                // Check if after applying this upgrade we've reached the maximum one
                if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
                {

                // Turn off the button if we reached max upgrade
                    upgradeButton.interactable = false;

                    //Debug.Log("Turnt off the " + myUpgrade.title + " button");
                    upgradeButton.GetComponent<Image>().color = Color.gray;
                    return;
                }

                // Update the text on the button to display the price of the next step
                buttonText.text = "BUY" + "\n" + myUpgrade.priceOfNextUpgradeLvl;

                

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
            foreach(Attribute attribute in myAttributes)
            {
                if (attribute.id == 0)
                {  
                    minContr.AddPercentageToMiningSpeed(myUpgrade.primaryListOfEffects[myUpgrade.currentUpgradeLvl]);
                }

                if (attribute.id == 1)
                {
                    minContr.AddTimeToDustTimer(myUpgrade.secondaryListOfEffects[myUpgrade.currentUpgradeLvl]);
                }

                
            }
        }

        if (myBuildings.Count > 0)
        {
            foreach(Building building in myBuildings)
            {
                mapController.SpawnAnItem(building);
                continue;
            }

           
        }
    }

        
    
	
	// Update is called once per frame
	void Update () {

        
        if (myUpgrade.currentUpgradeLvl < myUpgrade.maxUpgradeLvl)
        {
            if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currencyMined)
            {
                upgradeButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                upgradeButton.GetComponent<Image>().color = Color.gray;
            }
        } else
        {
            upgradeButton.GetComponent<Image>().color = Color.gray;
            buttonText.text = "Max upgrade" + "\n" +  "reached for this apartment!";
           
        }
		
	}
}
