using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {


    public UpgradeTemplate rackUpgrade;
    public UpgradeTemplate chairUpgrade;
    public UpgradeTemplate deskUpgrade;
    public UpgradeTemplate monitorUpgrade;
    public UpgradeTemplate coolingUpgrade;
    public UpgradeTemplate hamsterUpgrade;
    
    public MiningController miningControllerInstance;
    public MiningControllerTemplate myMiningController;
    public RigController rigController;
    public ItemDatabase itemDatabase;
    public MapDelegateHolder mapDelegateHolder;


    public List<Transform> rackSlots = new List<Transform>();
    public List<Transform> rigSlots = new List<Transform>();

    public Transform rackSlotHolder;
    public Transform rigSlotHolder;

    public float leftmostScrollValue;
    public float rightmostScrollValue;

    public GameObject partner;
    public GameObject livingRoom;
    public GameObject cooling;
    public Transform chairSlot;
    public Transform deskSlot;
    public Transform monitorSlot;
    public GameObject hamster;

    public Building rackBuildingObject;


    public Notification firstRackInstallation;
    public bool partnerKickedOut;
    public Notification fifthRackInstallation;
    public bool furnitureSold;

    

    public float pricePercentageGrowth;

    // This factors in what we are sending back to the UI of the racks
  
    public int racksPerGroup;
    public int maxRacks;
    public int racksBuilt;
    public int rigsBuilt;



    private void Start()
    {
        

        
    }


    private void OnEnable()
    {

        rigController = FindObjectOfType<RigController>();
        itemDatabase = FindObjectOfType<ItemDatabase>();
        miningControllerInstance = FindObjectOfType<MiningController>();
        mapDelegateHolder = FindObjectOfType<MapDelegateHolder>();


        rigController.rigUpgradedActions += UpgradeARig;
        rigController.rackUpgradedActions += UpgradeARackOfRigs;

      

        // Populating the rig slot list and assigning IDs
        int i = 0;
        foreach (Transform child in rigSlotHolder)
        {
            rigSlots.Add(child);
            child.GetComponent<Rigslot>().myOrderNumber = i;
            i++;
        }

        // Populating the rack slots list and assigning IDs

        foreach (Transform child in rackSlotHolder)
        {
            rackSlots.Add(child);
            child.GetComponent<RackSlot>().myOrderNumber = i;
            
                child.GetComponent<RackSlot>().racksInThisGroup = 0;
          
            i++;
        }

       

    }

   
    public void SpawnAnItem (Building thingToSpawn, int uiSlot = 999)
    {
        // We're spawning a RACK
        if (thingToSpawn.buildingID == 0)
        {
            if (!partnerKickedOut)
            {
                // Run the notification first to ask whether the player wants to remove his partner
                

                //FOR THIS TO WORK: Notification system must be enabled!
                AskAboutPartner();
                return;
            }

            if (racksBuilt >= (maxRacks / 2) && !furnitureSold)
            {
                // Run the notification first to ask whether the player wants to remove his furniture


                //FOR THIS TO WORK: Notification system must be enabled!
                AskAboutFurniture();
                return;
            }


            // Find an empty rackSlot to spawn into (if the map controls racks 1-by-1)

           
                foreach (Transform slot in rackSlots)
                {
                    // This needs to diversify between when the map supports multiple racks inside aslot, and when it doesn't

                    // This doesn't need a second option, because the Upgrade script already checks whether to let the player press the button
                    if (slot.childCount == 0)
                    {

                        Rack newRack = GameObject.Instantiate(thingToSpawn.myBuildingPrefab, slot.position, Quaternion.identity, slot).GetComponent<Rack>();
                        racksBuilt++;
                        slot.GetComponent<RackSlot>().racksInThisGroup++;
                        slot.GetComponent<RackSlot>().myRacks.Add(newRack);

                        // Passing in the slot number as well as the most basic rig, because this is a brand new rack
                        // This case is for when the map has singular rack control. This is needed because individual racks won't be shown in the 
                        // Main rig menu - they will be hidden in a slide-out menu
                        mapDelegateHolder.upgradedRackActions(itemDatabase.rigTypes[0], racksPerGroup, slot.GetComponent<RackSlot>().myOrderNumber);

                        return;
                    }

                }
            
   
        }
        // We're spawning a RIG (this only works for the 4 RIG slots in each map)
        else if (thingToSpawn.buildingID == 1)
        {
            foreach(Transform slot in rigSlots)
            {
                if (slot.GetComponent<Rigslot>().myOrderNumber == uiSlot)
                {
                    // This is how we "spawn" a rig by just making it active in the scene
                    slot.GetChild(0).gameObject.SetActive(true);

                    

                    itemDatabase.rigTypes[1].priceOfNextUpgradeLvl = (slot.GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);

                    //Debug.Log(slot.GetComponentInChildren<RigScript>(true).me);

                    mapDelegateHolder.upgradedRigActions(slot.GetComponent<Rigslot>().myOrderNumber, slot.GetComponentInChildren<RigScript>(true).me);
                    

                    rigsBuilt++;
                    return;
                }
            }
        }
    }

    #region UPGRADE RELATED FUNCTIONS
    public void UpgradeARig(int rigSlot)
    {
       
        foreach (Transform slot in rigSlots)
        {
            if (slot.GetComponent<Rigslot>().myOrderNumber == rigSlot)
            {
                RigScript rigscript = slot.gameObject.GetComponentInChildren<RigScript>(true);
                // DO THE MONEY CHECK
               
                if (rigscript.me.priceOfNextUpgradeLvl < miningControllerInstance.myMiningController.currencyMined)
                {

                    miningControllerInstance.myMiningController.currencyMined -= rigscript.me.priceOfNextUpgradeLvl;
                    float thisUpgradePrice = rigscript.me.priceOfNextUpgradeLvl;
                    Debug.Log(thisUpgradePrice);
                    // TODO: this might break when getting to the last item in the list
                    rigscript.me = itemDatabase.rigTypes[rigscript.me.id + 1];
                    rigscript.RefreshIcon();


                    // Here we calculate the price of the next ugprade after the one we've just installed
                    rigscript.me.priceOfNextUpgradeLvl = (thisUpgradePrice * pricePercentageGrowth / 100);
                    //pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);
                    Rig currentRig = rigscript.me;

                    // MAKING THE NEW RIG ACTUALLY APPLY AN EFFECT TO THE MINING CONTROLLER
                    miningControllerInstance.AddPercentageToMiningSpeed(currentRig.myEffectOnMining);

                    // Sewnding the info to the UI element responsible for this rig
                    mapDelegateHolder.upgradedRigActions(rigSlot, currentRig);
                    return;

                }
                else
                {
                    Debug.Log("Not enough money for this rig right now!"); 
                }
            }
        }
    }

    public void UpgradeARackOfRigs(int rackSlot, Rack rackToUpgrade = null)
    {
        if (!rackToUpgrade)
        {
            foreach (Transform slot in rackSlots)
            {
                //Checking my rack ID to find the one we want to upgrade
                if (slot.GetComponent<RackSlot>().myOrderNumber == rackSlot)
                {
                    Debug.Log("About to upgrade this rack!");
                    RigScript[] rigslotsInThisRackGroup;
                    // Collecting all the RIGSCRIPTS inside this RACKSLOT to the update them
                    rigslotsInThisRackGroup = slot.GetComponentsInChildren<RigScript>(true);
                    Debug.Log(rigslotsInThisRackGroup.Length);

                    // Find out if we have enough money to upgrade all the rigs in this group
                    if (rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length < miningControllerInstance.myMiningController.currencyMined)
                    {
                        // Charge me the money for the upgrade
                        miningControllerInstance.myMiningController.currencyMined -= rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length;
                        //Debug.Log("Charged you " + rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length + " for the upgrades on the rack");

                        foreach (RigScript rig in rigslotsInThisRackGroup)
                        {
                            Debug.Log(rig.me);
                            //Debug.Log("My rig id is now " + rig.me.id);
                            rig.me = itemDatabase.rigTypes[rig.me.id + 1];
                            //Debug.Log("My rig id is now " + rig.me.id);
                            rig.RefreshIcon();

                            // APPLYING THE UPGRADE'S EFFECTS (this will be done 3 times, because there are 3 rigs in each rack
                            miningControllerInstance.AddPercentageToMiningSpeed(rig.me.myEffectOnMining);

                        }
                        //Send in the info to the UI so it can be udpated with the newly updated rig info for this rag
                        mapDelegateHolder.upgradedRackActions(rigslotsInThisRackGroup[0].me, racksPerGroup, rackSlot);

                       
                        return;

                    }
                    Debug.Log("Not enough money to upgrade all the rigs in this group!");
                    return;
                }

            }
        } else
        {
            Debug.Log("About to upgrade a specific rack!");
            RigScript[] rigslotsInThisRackGroup;
            // Collecting all the RIGSCRIPTS inside this RACKSLOT to the update them
            rigslotsInThisRackGroup = rackToUpgrade.GetComponentsInChildren<RigScript>(true);

            if (rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length < miningControllerInstance.myMiningController.currencyMined)
            {
                // Charge me the money for the upgrade
                miningControllerInstance.myMiningController.currencyMined -= rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length;
                //Debug.Log("Charged you " + rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length + " for the upgrades on the rack");

                foreach (RigScript rig in rigslotsInThisRackGroup)
                {
                    Debug.Log(rig.me);
                    //Debug.Log("My rig id is now " + rig.me.id);
                    rig.me = itemDatabase.rigTypes[rig.me.id + 1];
                    //Debug.Log("My rig id is now " + rig.me.id);
                    rig.RefreshIcon();

                    miningControllerInstance.AddPercentageToMiningSpeed(rig.me.myEffectOnMining);

                }
              
                    mapDelegateHolder.upgradedRackActions(rigslotsInThisRackGroup[0].me, racksPerGroup, rackSlot);

                //Debug.Log("I've upgraded all the rigs in this group!");
                return;

            }
            Debug.Log("Not enough money to upgrade all the rigs in this group!");
            return;
        }
       
    }


    public void UpgradeChair()
    {

    }

    #endregion


    #region STORY RELATED FUNCTIONS
    public void AskAboutPartner()
    {
        NotificationSystem.Instance.DisplayAChoiceNotification(firstRackInstallation, null, () => { SpawnAnItem(rackBuildingObject); KickOutThePartner(); });
        partnerKickedOut = true;
    }

    public void KickOutThePartner()
    {
        Destroy(partner.gameObject);
        
    }

    public void AskAboutFurniture()
    {
        NotificationSystem.Instance.DisplayAChoiceNotification(fifthRackInstallation, null, () => { SpawnAnItem(rackBuildingObject); GetRidOfFurniture(); });
        furnitureSold = true;
    }

    private void GetRidOfFurniture()
    {
        Destroy(livingRoom.gameObject);
        
    }

    #endregion  
}
