using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApartmentUpgrade : MonoBehaviour {

    public Image icon;
    public Text title;
    public Text descText;
    public Text miscText;
    public Button buyButton;
    public Image darkOverlay;
    public GameObject alreadyOwnedText;
    public int orderNumber;

    public Apartment myApartment;


    public bool active;
    public bool current;
    private LevelUpgrader levelUpgrader;
    private MiningController miningController;

	// Use this for initialization
	void Start () {
        levelUpgrader = FindObjectOfType<LevelUpgrader>();
        miningController = FindObjectOfType<MiningController>();
        levelUpgrader.upgradedApartment += RefreshApartmentPurchaseUI;
        AssignFunctionToButton();
    }

    private void OnEnable()
    {
        // Activating the UI elements on this apartment 
        if (active & !current)
        {
            darkOverlay.gameObject.SetActive(false);
            alreadyOwnedText.gameObject.SetActive(false);

            icon.sprite = myApartment.myIcon;
            title.text = myApartment.myTitle;
            descText.text = myApartment.myDescText;

            buyButton.GetComponentInChildren<Text>().text = "BUY APARTMENT\n" + myApartment.myPrice;
        }
        else if (current)
        {
            darkOverlay.gameObject.SetActive(true);
            alreadyOwnedText.gameObject.SetActive(true);

            buyButton.GetComponentInChildren<Text>().text = "...";
            buyButton.interactable = false;
            buyButton.GetComponent<Image>().color = Color.gray;
        }
    }

    void AssignFunctionToButton()
    {
        // Here we decide what the BUY APARTMENT button does
        buyButton.onClick.AddListener(() => levelUpgrader.UpgradeToDifferentLvl(myApartment.myPrefab, this));
    }

    void RefreshApartmentPurchaseUI(ApartmentUpgrade UItoUpdate)
    {
        if (this == UItoUpdate)
        {
            active = false;
            current = true;

            darkOverlay.gameObject.SetActive(true);
            alreadyOwnedText.gameObject.SetActive(true);

            buyButton.GetComponentInChildren<Text>().text = "...";
            buyButton.interactable = false;
            buyButton.GetComponent<Image>().color = Color.gray;

        } else
        {
            if (this.orderNumber < UItoUpdate.orderNumber)
            {
                active = false;
                current = false;
                darkOverlay.gameObject.SetActive(true);
                alreadyOwnedText.gameObject.SetActive(false);
                buyButton.GetComponentInChildren<Text>().text = "...";
                buyButton.GetComponent<Image>().color = Color.gray;
            }     
            
            // Checking whether this is a current apartment
            if (current)
            {
                current = false;
                alreadyOwnedText.gameObject.SetActive(false);
            }
            buyButton.GetComponent<Image>().color = Color.gray;
        }  
    }

    // Update is called once per frame
    void Update () {
		
        if (active)
        {
            // TODO replace this logic with a method in Mining Controller
            if (miningController.myMiningController.currentBalance < myApartment.myPrice)
            {
                buyButton.interactable = false;
                buyButton.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                buyButton.interactable = true;
                buyButton.GetComponent<Image>().color = Color.green;
            }
        }
       


	}
}
