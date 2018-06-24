using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RackGroup : MonoBehaviour {

    public bool isEnabled;
    public bool fullyUpgraded;
    public Image darkOverlay;

    public RigController rigController;
    public MapController currentMapController;
    public MiningController miningController;
    public UIController mainUIController;

    
    public int racksControlled;


    // All the text objects we will need to adjust every upgrade 
    public Text title;
    public Text description;
  
    // The 2 buttons on this UI
    public Button unfoldButton;
    public Button upgradeAllButton;

    // For changing the text/image of the buttons
    public Text upgradeAllButtonText;

    // For controlling the upgrade level bar
    public Image upgradeLvlUI;

    public int orderNumber;
   

    // Use this for initialization
    void Start () {

        rigController = FindObjectOfType<RigController>();
        miningController = FindObjectOfType<MiningController>();
       
        mainUIController = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIController>();

        // I am sending the ID of the rackslot I am referring to
        unfoldButton.onClick.AddListener(() => mainUIController.ShowRacksSideMenu(orderNumber));

        upgradeAllButton.onClick.AddListener(() => UpgradeAllRacksInThisGroup());


    }

    private int UpgradeAllRacksInThisGroup()
    {
        // TODO implement
        throw new NotImplementedException();
    }

    private void OnEnable()
    {

        currentMapController = FindObjectOfType<MapController>();

        if (currentMapController.racksBuilt <= 0)
        {
            upgradeAllButton.interactable = false;
            unfoldButton.interactable = false;

            upgradeAllButton.GetComponent<Image>().color = Color.black;
            unfoldButton.GetComponent<Image>().color = Color.black;


        }
        else
        {
            upgradeAllButton.interactable = true;
            unfoldButton.interactable = true;

            upgradeAllButton.GetComponent<Image>().color = Color.green;
            unfoldButton.GetComponent<Image>().color = Color.green;
        }
    }

    // Update is called once per frame
    void Update () {
        


	}
}
