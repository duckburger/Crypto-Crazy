using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RigUI : MonoBehaviour {

    public bool isEnabled;
    public bool fullyUpgraded;
    public Image darkOverlay;

    public RigController rigController;
    public MapController currentMapController;
    public MiningController miningController;


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



        rigController = FindObjectOfType<RigController>();
        miningController = FindObjectOfType<MiningController>();
        currentMapController = FindObjectOfType<MapController>();

        

        if (controllingRack)
        {
            upgradeButton.onClick.AddListener(() => rigController.UpgradeARack(GetComponent<RackID>().myControlID));
            myRackID = GetComponent<RackID>();
        } else
        {
            upgradeButton.onClick.AddListener(() => rigController.UpgradeARig(GetComponent<RigID>().myControlID));
            myRigID = GetComponent<RigID>();
            myRig = currentMapController.itemDatabase.rigTypes[0];
        }

        if (activateButton)
            activateButton.onClick.AddListener(() => ActivateThisUI());

        if (!controllingRack)
            currentMapController.upgradedRigActions += UpdateMyRigUI;
        else
            currentMapController.upgradedRackActions += UpdateMyRackUI;

        

        InitizalizeTheUI();

        
    }

    void ActivateThisUI()
    {
        if (miningController.myMiningController.currencyMined > 200)
        {
            miningController.myMiningController.currencyMined -= 200;
            isEnabled = true;
            InitizalizeTheUI();
            if (!controllingRack)
                currentMapController.SpawnAnItem(myRig, myRigID.myControlID);
        }
        else
        {
            Debug.Log("Not enough money to buy a rig!");
        }
       
        
            
    }
  

   

    void UpdateMyRigUI(int idOfUpdatedRig, Rig currentRigInfo)
    {

        
        if (idOfUpdatedRig == myRigID.myControlID)
        {
          
            // FIRST check whether this is the last upgade
            if (currentRigInfo.buildingID  >= 9)
            {
                title.text = currentRigInfo.title;
                description.text = currentRigInfo.myDescription;
                miscText.text = "MAX RIG LVL REACHED!";
                rigIcon.sprite = currentRigInfo.icon;
                buttonText.text = "MAX UPGRADE REACHED";

                tempEffectPercentage += currentRigInfo.myEffectOnMining;

                currentEffectText.text = "+ " + tempEffectPercentage.ToString() + "%";
                //rigsControlled.text = "x1";
                upgradeLvlUI.fillAmount = currentRigInfo.buildingID / currentMapController.gameObject.GetComponent<ItemDatabase>().rigTypes.Count;

                // Set this button as disabled
                upgradeLvlUI.fillAmount = 1;
                upgradeButton.GetComponent<Image>().color = Color.gray;
                upgradeButton.interactable = false;
                fullyUpgraded = true;
                myRig = currentRigInfo;
                return;
            }


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
                 upgradeLvlUI.fillAmount = (float)currentRigInfo.buildingID / currentMapController.gameObject.GetComponent<ItemDatabase>().rigTypes.Count;
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
                upgradeLvlUI.fillAmount = (float)currentRigInfo.buildingID / currentMapController.gameObject.GetComponent<ItemDatabase>().rigTypes.Count;
                myRig = currentRigInfo;

            }
        }
    }


    public void UpdateMyRackUI(int rackSlot, Rack currentRackInfo)
    {

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
                miscText.text = "Next rig effect: \n" + "+" + currentMapController.itemDatabase.rigTypes[0].myEffectOnMining + "% mining speed";
            } else
            {
                title.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.title;
                description.text = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.myDescription;
                buttonText.text = "UPGRADE RACK\n" + currentMapController.rackSlots[myRackID.myControlID].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl;
                miscText.text = "Next rig effect: \n" + "+" + currentMapController.itemDatabase.rigTypes[0].myEffectOnMining + "% mining speed";
            }

            tempEffectPercentage = currentMapController.rigSlots[0].GetComponentInChildren<RigScript>(true).me.myEffectOnMining;
            Debug.Log(tempEffectPercentage);
            currentEffectText.text = "+ " + tempEffectPercentage + "%";
            upgradeButton.interactable = true;
            darkOverlay.gameObject.SetActive(false);
            upgradeLvlUI.fillAmount = 0;

            

        }
    }

    


	
	// Update is called once per frame
	void Update () {

        if (!fullyUpgraded)
        {
            if (isEnabled)
            {
                if (myRig.priceOfNextUpgradeLvl < miningController.myMiningController.currencyMined)
                {


                    if (!controllingRack)
                    {
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
                        if (currentMapController.rackSlots[myRackID.myControlID - 4].GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl < miningController.myMiningController.currencyMined
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
