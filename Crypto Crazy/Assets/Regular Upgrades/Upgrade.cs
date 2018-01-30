using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    public UpgradeTemplate myUpgrade;
    public MiningControllerTemplate myMiningController;
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
	void Start () {
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
        if (myUpgrade.effectsForEachUpgradeLvl.Count == 0)
        {
            currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
            miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
        } else
        {
            tempEffectPercentage = myUpgrade.effectsForEachUpgradeLvl[(int)myUpgrade.currentUpgradeLvl];
            currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
            miscTextField.text = myUpgrade.miscText + " " + myUpgrade.effectsForEachUpgradeLvl[(int)myUpgrade.currentUpgradeLvl + 1] + "% mining speed";
        }
    }


    // THE UPGRADE COST MATH IS HERE
    public void PurchaseTheUpgrade()
    {
        if (myUpgrade.currentUpgradeLvl < myUpgrade.maxUpgradeLvl)
        {
            if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currencyMined)
            {
                myMiningController.currencyMined -= myUpgrade.priceOfNextUpgradeLvl;
                myUpgrade.currentUpgradeLvl += 1;

                // TODO: Try some other math here
                // Control of the growth of the upgrade price
                myUpgrade.priceOfNextUpgradeLvl += (myUpgrade.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
                pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);



                // This line raises the event from my attached GameEvent
                // which is unique for each upgrade. This even determines the effect of the upgrade
                activationEvent.Raise();

                upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;
                Debug.Log("Fill amount is now " + upgradeLevelUI.fillAmount);


                if (myUpgrade.effectsForEachUpgradeLvl.Count == 0)
                {
                    currentEffectText.text = myUpgrade.currentUpgradeLvl.ToString();
                    miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
                }
                else 
                {
                    tempEffectPercentage += myUpgrade.effectsForEachUpgradeLvl[(int)myUpgrade.currentUpgradeLvl];
                    currentEffectText.text = "+ " + (tempEffectPercentage).ToString() + "%";

                    if ((int)myUpgrade.currentUpgradeLvl + 1 <= myUpgrade.effectsForEachUpgradeLvl.Count - 1)
                        miscTextField.text = myUpgrade.miscText + " " + myUpgrade.effectsForEachUpgradeLvl[(int)myUpgrade.currentUpgradeLvl + 1] + "% mining speed";
                    else
                        miscTextField.text = "MAX UPGRADE REACHED!";
                } 

                buttonText.text = "BUY" + "\n" + myUpgrade.priceOfNextUpgradeLvl;

                if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
                {
                    upgradeButton.interactable = false;

                    Debug.Log("Turnt off the " + myUpgrade.title + " button");
                    upgradeButton.GetComponent<Image>().color = Color.gray;

                }

            }
            else
            {
                Debug.Log("Not enough money for this upgrade right now!");
                return;
            }
        } else
        {
            Debug.Log("You've reached the maximum level for this upgrade right now");
            return;
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
