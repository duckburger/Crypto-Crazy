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

    public MiningController miningControllerInstance;
    public MiningControllerTemplate myMiningController;
    public RigController rigController;
    public ItemDatabase itemDatabase;


    public List<Transform> rackSlots = new List<Transform>();
    public List<Transform> rigSlots = new List<Transform>();

    public Transform rackSlotHolder;
    public Transform rigSlotHolder;


    public float leftmostScrollValue;
    public float rightmostScrollValue;

    public GameObject partner;
    public GameObject livingRoom;
    public GameObject cooling;

    public Building rackBuildingObject;


    public Notification firstRackInstallation;
    public bool partnerKickedOut;
    public Notification fifthRackInstallation;
    public bool furnitureSold;

    // This will get called whenever we finish updating a rig (sends a message to UI, for example)
    public delegate void OnRigUpgraded(int rigSlot, Rig newlyInstalledRig);
    public OnRigUpgraded upgradedRigActions;


    public delegate void OnRackUpgraded(int rackSlot, Rig newlyInstalledRig, int racksInThisGroup);
    public OnRackUpgraded upgradedRackActions;

    public float pricePercentageGrowth;

    // This factors in what we are sending back to the UI of the racks
    public bool controlsRacksByOne;
    public int racksPerGroup;
    public int maxRacks;
    public int racksBuilt;


    private void Start()
    {
        

        rigController = FindObjectOfType<RigController>();
        itemDatabase = GetComponent<ItemDatabase>();
        miningControllerInstance = FindObjectOfType<MiningController>();
    }


    private void OnEnable()
    {

        rigController = FindObjectOfType<RigController>();

       

        // Populating the rig slot list
        int i = 0;
        foreach (Transform child in rigSlotHolder)
        {
            rigSlots.Add(child);
            child.GetComponent<Rigslot>().myOrderNumber = i;
            i++;
        }

        // Populating the rack slots list

        foreach (Transform child in rackSlotHolder)
        {
            rackSlots.Add(child);
            child.GetComponent<RackSlot>().myOrderNumber = i;
            i++;

        }

        rigController.rigSpawnedActions += UpgradeARig;
        rigController.rackSpawnedActions += UpgradeARackOfRigs;

    }

   
    public void SpawnAnItem (Building thingToSpawn, int uiSlot = 999)
    {
        // We're spawning a RACK
        if (thingToSpawn.buildingID == 0)
        {
            if (!partnerKickedOut)
            {
                // Run the notification first to ask whether the player wants to remove his partner
                AskAboutPartner();
                return;
            }

            if (racksBuilt >= maxRacks / 2 && !furnitureSold)
            {
                // Run the notification first to ask whether the player wants to remove his furniture
                AskAboutFurniture();
                return;
            }



            foreach (Transform slot in rackSlots)
            {
                // This doesn't need a second option, because the Upgrade script already checks whether to let the player press the button
                if (slot.childCount == 0)
                {
                   
                    GameObject.Instantiate(thingToSpawn.myBuildingPrefab, slot.position, Quaternion.identity, slot);
                    racksBuilt++;
                    slot.GetComponent<RackSlot>().racksInThisGroup++;

                    // Passing in the slot number as well as the most basic rig, because this is a brand new rack
                    // This case is for when the map has singular rack control
                    if (controlsRacksByOne)
                        upgradedRackActions(slot.GetComponent<RackSlot>().myOrderNumber, itemDatabase.rigTypes[0], racksPerGroup);

                    return;
                }

            }

            // We're spawning a RIG
        }
        else if (thingToSpawn.buildingID == 1)
        {
            foreach(Transform slot in rigSlots)
            {
                if (slot.GetComponent<Rigslot>().myOrderNumber == uiSlot)
                {
                    slot.GetChild(0).gameObject.SetActive(true);
                    return;
                }
            }
        }
    }


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
                    upgradedRigActions(rigSlot, currentRig);
                    return;

                }
                else
                {
                    Debug.Log("Not enough money for this rig right now!"); 
                }
            }
        }
    }

  
    public void UpgradeARackOfRigs(int rackSlot)
    {

        
        foreach (Transform slot in rackSlots)
        {
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

                        miningControllerInstance.AddPercentageToMiningSpeed(rig.me.myEffectOnMining);

                    }
                    //Send in the info to the UI so it can be udpated with the newly updated rig info for this rag
                    if (controlsRacksByOne)
                        upgradedRackActions(rackSlot, rigslotsInThisRackGroup[0].me, racksPerGroup);

                    //Debug.Log("I've upgraded all the rigs in this group!");
                    return;
                   
                }
                Debug.Log("Not enough money to upgrade all the rigs in this group!");
                return;
            }
            
        }
    }


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
