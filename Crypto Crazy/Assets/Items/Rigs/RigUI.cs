using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RigUI : MonoBehaviour {

    private ItemDatabase itemDatabase;

    public bool isEnabled;
    public bool fullyUpgraded;
    public Image darkOverlay;

    public RigController rigController;
    public MapController currentMapController;
    public MiningController miningController;
    public UIController mainUIController;
    public MapDelegateHolder mapDelegateHolder;


    // Determines whether I control the rig or the rack
    public bool controllingRack;

    public int racksControlled;


    // All the text objects we will need to adjust every upgrade 
    public Text title;
    public Text description;
    public Text miscText;
    public Text rigsControlled;

    // We need this to update the rig icon on the left
    public Image rigIcon;

    // Needs this to adjust the button properties every click
    public Button upgradeButton;

    // This button activates the rig panel and buy the basic rig
    public Button activateButton;

    // For changing the text of the button
    public Text buttonText;

    // For controlling the upgrade level bar
    public Image upgradeLvlUI;

    // Show the player what is the current effect of this rig
    public Text currentEffectText;

    public RigID myRigID;
    public RackID myRackID;

    public Rig myRig;
    public Rack myRack;
    


    [SerializeField]
    private float tempEffectPercentage;

    // Use this for initialization
    void Start () {

        currentMapController = FindObjectOfType<MapController>();
        itemDatabase = FindObjectOfType<ItemDatabase>();
        rigController = FindObjectOfType<RigController>();
        miningController = FindObjectOfType<MiningController>();
        mapDelegateHolder = FindObjectOfType<MapDelegateHolder>();




        if (controllingRack)
        {   // If this UI element controls a rack, then make the button call upgrade a rack function from the rig controller


            //Debug.Log("Signing up for the rack upgrades!!! I am a button with and ID " + GetComponent<RackID>().myControlID);
            upgradeButton.onClick.AddListener(() => rigController.UpgradeARack(GetComponent<RackID>().myControlID));
            
            myRackID = GetComponent<RackID>();
            
        } else
        {
            upgradeButton.onClick.AddListener(() => rigController.UpgradeARig(GetComponent<RigID>().myControlID));
            myRigID = GetComponent<RigID>();
            
            myRig = FindObjectOfType<ItemDatabase>().rigTypes[0];
        }

        // If the activate button is ON then give it a function of buying a brand new rig
        if (activateButton && !controllingRack)
            activateButton.onClick.AddListener(() => BuyARigFromScratch());
        else
            activateButton.gameObject.SetActive(false);

        if (!controllingRack)
            mapDelegateHolder.upgradedRigActions += UpdateMyRigUI;
        else
            mapDelegateHolder.upgradedRackActions += UpdateMyRackUI;

        InitizalizeTheUI();
   
    }

    


    public void ResetDataForNewApartment()
    {
        Debug.Log("Refreshing data for the new apartment");
        
        // Find the mape controller for the new apartment
        currentMapController = FindObjectOfType<MapController>();
        mapDelegateHolder = FindObjectOfType<MapDelegateHolder>();

        // Need to assigne functions to the new delegates
        if (!controllingRack)
            mapDelegateHolder.upgradedRigActions += UpdateMyRigUI;
        else
            mapDelegateHolder.upgradedRackActions += UpdateMyRackUI;
    }

    public void InitizalizeTheUI()
    {
        if (!isEnabled)
        {

            title.text = "???";
            description.text = "???";
            miscText.text = "???";

            buttonText.text = "???";
            rigsControlled.text = "???";
            upgradeLvlUI.fillAmount = 0;
            upgradeButton.GetComponent<Image>().color = Color.gray;
            upgradeButton.interactable = false;
            darkOverlay.gameObject.SetActive(true);

        }
        else
        {
            upgradeButton.GetComponent<Image>().color = Color.green;
            activateButton.gameObject.SetActive(false);

            if (!controllingRack)
            {
                title.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.title;
                description.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.myDescription;
                //Debug.Log(currentMapController.rigSlots[myRigID.myControlID].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl);
                buttonText.text = "UPGRADE RIG\n" + currentMapController.rigSlots[myRigID.myControlID].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl;
                //Debug.Log(currentMapController.itemDatabase);
                rigsControlled.text = "x1";
                miscText.text = "Next rig effect: \n" + "+" + itemDatabase.rigTypes[0].myEffectOnMining + "% mining speed";
            }
            else
            {
                title.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.title;
                description.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.myDescription;

                // TODO has to send the INDEX not the NUMBER
                buttonText.text = "UPGRADE RACK\n" + currentMapController.rackSlots[myRackID.myControlID].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl;

                // Multiplying by 3 because this controls a rack
                miscText.text = "Next rack effect: \n" + "+" + itemDatabase.rigTypes[0].myEffectOnMining * 3 + "% mining speed";

                activateButton.gameObject.SetActive(false);
            }

            tempEffectPercentage = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.myEffectOnMining;

            currentEffectText.text = "+ " + tempEffectPercentage + "%";
            upgradeButton.interactable = true;
            darkOverlay.gameObject.SetActive(false);
            upgradeLvlUI.fillAmount = 0;



        }
    }

    #region RIG SPECIFIC FUNCTIONS

    void BuyARigFromScratch()
    {
        // Check if we have enough money for the rig
        if (miningController.myMiningController.currencyMined > 200)
        {
            miningController.myMiningController.currencyMined -= 200;
            isEnabled = true;
            InitizalizeTheUI();
            if (!controllingRack)
                // Passing in my building type and my order number to the map controller
                currentMapController.SpawnAnItem(myRig, myRigID.myControlID);
        }
        else
        {
            Debug.Log("Not enough money to buy a rig!");
        }
       
        
            
    }
    

    // This one is signed up to the UPGRADED A RIG delegate in the MapController
    void UpdateMyRigUI(int idOfUpdatedRig, Rig currentRigInfo)
    {
        //Debug.Log("I've been passed in a " + currentRigInfo.priceOfNextUpgradeLvl);
        
        if (idOfUpdatedRig == myRigID.myControlID)
        {
          
            // FIRST check whether this is the last upgade
            if (currentRigInfo.id  >= 9)
            {
                title.text = currentRigInfo.title;
                description.text = currentRigInfo.myDescription;
                miscText.text = "MAX RIG LVL REACHED!";
                rigIcon.sprite = currentRigInfo.icon;
                buttonText.text = "MAX UPGRADE REACHED";

                tempEffectPercentage += currentRigInfo.myEffectOnMining;

                currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";

               
                upgradeLvlUI.fillAmount = currentRigInfo.buildingID / currentMapController.itemDatabase.rigTypes.Count;

                // Set this button as disabled
                upgradeLvlUI.fillAmount = 1;
                upgradeButton.GetComponent<Image>().color = Color.gray;
                upgradeButton.interactable = false;
                fullyUpgraded = true;
                myRig = currentRigInfo;
                return;
            }

            // If not controlling a rack
            if (!controllingRack)
            {

                 
                 title.text = currentRigInfo.title;
                 description.text = currentRigInfo.myDescription;
                 miscText.text = "Next rig effect: \n" + "+" + currentRigInfo.myEffectOnMining + "% mining speed";
                 rigIcon.sprite = currentRigInfo.icon;

                 tempEffectPercentage += currentRigInfo.myEffectOnMining;

                 currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
                
                 buttonText.text = "BUY BETTER RIG\n" + currentRigInfo.priceOfNextUpgradeLvl;
                 //rigsControlled.text = "x1";
                 upgradeLvlUI.fillAmount = (float)currentRigInfo.id / itemDatabase.rigTypes.Count;
                 myRig = currentRigInfo;


            }
            else
            {
                
                title.text = currentRigInfo.title;
                description.text = currentRigInfo.myDescription;
                miscText.text = "Next rig effect: \n" + "+" + currentRigInfo.myEffectOnMining + "% mining speed";
                rigIcon.sprite = currentRigInfo.icon;

                tempEffectPercentage += currentRigInfo.myEffectOnMining;

                currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
                buttonText.text = "BUY BETTER RIG\n" + currentRigInfo.priceOfNextUpgradeLvl;
                //rigsControlled.text = "x1";
                upgradeLvlUI.fillAmount = (float)currentRigInfo.buildingID / itemDatabase.rigTypes.Count;
                myRig = currentRigInfo;

            }
        }
    }
    #endregion

    #region RACK SPECIFIC FUNCTIONS




 

    // This one is signed up to the UPGRADED A RACK delegate in the MapController
    public void UpdateMyRackUI(Rig currentRigInThisRack, int racksPerGroup, int rackSlot = -1)
    {

        
        // If the rack ID that was sent in matches my RACK ID...
        if (rackSlot == myRackID.myControlID)
        {
            // FIRST check whether this is the last upgade (uses a magic number - currently max rig level)
            if (currentRigInThisRack.id >= 9)
            {
                isEnabled = false;
                title.text = currentRigInThisRack.title + " (Rack)";
                description.text = currentRigInThisRack.myDescription;
                miscText.text = "MAX RIG LVL REACHED!";
                rigIcon.sprite = currentRigInThisRack.icon;
                buttonText.text = "MAX UPGRADE REACHED";

                tempEffectPercentage += currentRigInThisRack.myEffectOnMining;

                currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
                rigsControlled.text = "x" + (racksPerGroup * 3).ToString();
                // Shows the upgrade level visually by dividing by the size of the size of the rig database
                upgradeLvlUI.fillAmount = currentRigInThisRack.buildingID / itemDatabase.rigTypes.Count;
                upgradeLvlUI.fillAmount = 1;
                // Set this button as disabled and change its color to gray
                upgradeButton.GetComponent<Image>().color = Color.gray;
                upgradeButton.interactable = false;
                fullyUpgraded = true;
                myRig = currentRigInThisRack;
                return;
            }
                if (currentRigInThisRack.id == 0)
                    isEnabled = true;
                    darkOverlay.gameObject.SetActive(false);
                    upgradeButton.interactable = true;

                title.text = currentRigInThisRack.title + " (Rack)";
                description.text = currentRigInThisRack.myDescription;
                miscText.text = "Next rig effect: \n" + "+" + currentRigInThisRack.myEffectOnMining + "% mining speed";
                rigIcon.sprite = currentRigInThisRack.icon;

                // Multiplying 3 because there are 3 rigs in the rack
                tempEffectPercentage += currentRigInThisRack.myEffectOnMining * 3;

                

                currentEffectText.text = "+ " + (int)tempEffectPercentage + "%";
                
                // Multiplying 3 because there are 3 rigs in the rack
                buttonText.text = "UPGRADE THE RACK\n" + currentRigInThisRack.priceOfNextUpgradeLvl * 3;
                rigsControlled.text = "x3";
                upgradeLvlUI.fillAmount = (float)currentRigInThisRack.id / itemDatabase.rigTypes.Count;
                myRig = currentRigInThisRack;
            
        }
    }





    #endregion

   

    


	
	// Using this mainly to update the UI on the buttons
	void Update () {

        if (!isEnabled && !controllingRack)
        {
            if (miningController.myMiningController.currencyMined > 200)
            {
                activateButton.GetComponent<Image>().color = Color.green;
            }
            else if (miningController.myMiningController.currencyMined < 200)
            {
                activateButton.GetComponent<Image>().color = Color.grey;
            }
        }
        


        if (!fullyUpgraded)
        {
            if (isEnabled)
            {
                if (myRig.priceOfNextUpgradeLvl < miningController.myMiningController.currencyMined)
                {

                    // This only works because we are controlling the racks 1 by 1
                    if (!controllingRack)
                    {
                        Debug.Log(currentMapController.rigSlots[myRigID.myControlID]);
                        if (currentMapController.rigSlots[myRigID.myControlID].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl < miningController.myMiningController.currencyMined)
                        {
                            upgradeButton.GetComponent<Image>().color = Color.green;

                        }
                        else
                        {
                            upgradeButton.GetComponent<Image>().color = Color.gray;
                        }

                    }
                    else
                    {
                        // Multiplying by 3 because there are 3 rigs in the rack
                       
                        if (currentMapController.rackSlots[myRackID.myControlID - 4].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl * 3 < miningController.myMiningController.currencyMined
                            && myRig.priceOfNextUpgradeLvl < miningController.myMiningController.currencyMined)
                        {
                            upgradeButton.GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            upgradeButton.GetComponent<Image>().color = Color.gray;
                        }
                    }

                }
                else
                {
                    upgradeButton.GetComponent<Image>().color = Color.gray;
                }


            }
        }
        

    }
}
