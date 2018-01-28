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

        miscTextField.text = myUpgrade.miscText + " " + myUpgrade.maxUpgradeLvl;
        currentEffectText.text = myUpgrade.currentUpgradeEffect.ToString();
    }


    public void PurchaseTheUpgrade()
    {
        if (myUpgrade.priceOfNextUpgradeLvl < myMiningController.currencyMined && myUpgrade.currentUpgradeLvl <= myUpgrade.maxUpgradeLvl)
        {
            myMiningController.currencyMined -= myUpgrade.priceOfNextUpgradeLvl;
            myUpgrade.currentUpgradeLvl += 1;
            
            // Control of the growth of the upgrade price
            myUpgrade.priceOfNextUpgradeLvl += (myUpgrade.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
            pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);

            // This line raises the event from my attached GameEvent
            // which is unique for each upgrade. This even determines the effect of the upgrade
            activationEvent.Raise();

            upgradeLevelUI.fillAmount = myUpgrade.currentUpgradeLvl / myUpgrade.maxUpgradeLvl;
            currentEffectText.text = myUpgrade.currentUpgradeEffect.ToString();

            Debug.Log("Fill amount is now " + upgradeLevelUI.fillAmount);

            buttonText.text = "BUY" + "\n" + myUpgrade.priceOfNextUpgradeLvl;

            if (myUpgrade.currentUpgradeLvl == myUpgrade.maxUpgradeLvl)
            {
                upgradeButton.interactable = false;
                upgradeButton.GetComponent<Image>().color = Color.gray;

            }

        } else
        {
            Debug.Log("Max upgrade level for this apartment reached");
            return;
        }

        return;
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
            buttonText.text = "Max upgrade" + "\n" +  "reached for this apartment!";
           
        }
		
	}
}
