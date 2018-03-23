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

    public Apartment myApartment;

    [SerializeField]
    private bool active;
    [SerializeField]
    private bool current;
    private LevelUpgrader levelUpgrader;
    private MiningController miningController;

	// Use this for initialization
	void Start () {
        levelUpgrader = FindObjectOfType<LevelUpgrader>();
        miningController = FindObjectOfType<MiningController>();

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

            

        } else if (current)
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
        buyButton.onClick.AddListener(() => levelUpgrader.UpgradeToDifferentLvl(myApartment.myPrefab));
    }


    // Update is called once per frame
    void Update () {
		
        if (active)
        {
            if (miningController.myMiningController.currencyMined < myApartment.myPrice)
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
