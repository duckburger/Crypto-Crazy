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
    private LevelUpgrader levelUpgrader;

	// Use this for initialization
	void Start () {
        levelUpgrader = FindObjectOfType<LevelUpgrader>();

    }

    private void OnEnable()
    {
        // Activating the UI elements on this apartment 
        if (active)
        {
            darkOverlay.gameObject.SetActive(false);
            alreadyOwnedText.gameObject.SetActive(false);

            icon.sprite = myApartment.myIcon;
            title.text = myApartment.myTitle;
            descText.text = myApartment.myDescText;


        }
    }

    void AssignFunctionToButton()
    {
        // Here we decide what the BUY APARTMENT button does
        buyButton.onClick.AddListener(() => levelUpgrader.UpgradeToDifferentLvl(myApartment.myPrefab));
    }


    // Update is called once per frame
    void Update () {
		


	}
}
