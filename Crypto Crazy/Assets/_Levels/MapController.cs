using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    [Header("Global data about all upgrades")]
    public UpgradeTemplate rackUpgrade;
    public UpgradeTemplate chairUpgrade;
    public UpgradeTemplate deskUpgrade;
    public UpgradeTemplate monitorUpgrade;
    public UpgradeTemplate coolingUpgrade;
    public UpgradeTemplate hamsterUpgrade;
    
    [Header("Object references")]
    public MiningController miningControllerInstance;
    public MiningControllerTemplate myMiningController;
    public RigController rigController;
    public ItemDatabase itemDatabase;
    public MapDelegateHolder mapDelegateHolder;
    public List<Transform> rackSlots = new List<Transform>();
    public List<Transform> rigSlots = new List<Transform>();
    public Transform rackSlotHolder;
    public Transform rigSlotHolder;
    public SpriteBlinker lightsBlinker;

    [Header("Camera variables")]
    public float leftmostPanValue;
    public float rightmostPanValue;
    public float leftPanValZoomed;
    public float rightPanValZoomed;

    [Header("Objects in scene")]
    public GameObject partner;
    public GameObject livingRoom;
    public GameObject cooling;
    public Transform chairSlot;
    public Transform deskSlot;
    public Transform monitorSlot;
    public GameObject hamster;

    public Building rackPrefab;

    [Header("Notifications")]
    public Notification firstRackInstallation;
    public bool partnerKickedOut;
    public Notification fifthRackInstallation;
    public bool furnitureSold;

    [SerializeField] float pricePercentageGrowth;

    // This factors in what we are sending back to the UI of the racks
  
    public int racksPerGroup;
    public int maxRacks;
    public int racksBuilt;
    public int rigsBuilt;
    public int maxRigs;


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

    private void OnDestroy()
    {
        rigController.rigUpgradedActions -= UpgradeARig;
        rigController.rackUpgradedActions -= UpgradeARackOfRigs;
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

            if (racksBuilt == 3 && !furnitureSold)
            {
                // Run the notification first to ask whether the player wants to remove his furniture
                //FOR THIS TO WORK: Notification system must be enabled!
                AskAboutFurniture();
                return;
            }
            // Find an empty rackSlot to spawn into 
                foreach (Transform slot in rackSlots)
                {
                    // This needs to diversify between when the map supports multiple racks inside aslot, and when it doesn't

                    // This doesn't need a second option, because the Upgrade script already checks whether to let the player press the button
                    if (slot.childCount == 0)
                    {

                        Rack newRack = GameObject.Instantiate(thingToSpawn.myBuildingPrefab, slot.position, Quaternion.identity, slot).GetComponent<Rack>();
                        racksBuilt++;
                        Debug.Log("Racks build is now " + racksBuilt);
                        slot.GetComponent<RackSlot>().racksInThisGroup++;
                        slot.GetComponent<RackSlot>().myRacks.Add(newRack);

                    // Passing in the slot number as well as the most basic rig, because this is a brand new rack
                        
                        mapDelegateHolder.upgradedRackActions(itemDatabase.rigTypes[0], slot.GetComponent<RackSlot>().myOrderNumber);

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

                    
                    // This line sets up the pricing for consequtive rigs
                    itemDatabase.rigTypes[1].priceOfNextUpgradeLvl = (slot.GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);

                    //Debug.Log(slot.GetComponentInChildren<RigScript>(true).me);
                    
                    mapDelegateHolder.upgradedRigActions(slot.GetComponent<Rigslot>().myOrderNumber, slot.GetComponentInChildren<RigScript>(true).me);
                    

                    rigsBuilt++;
                    return;
                }
            }
        }
        // We're upgrading a CHAIR
        else if (thingToSpawn.buildingID == 2)
        {
            // Find the current upgr lvl of chair and see if we need to upgrade the sprite. 
            // Even if we don't, do some effects on the chair?
            if (chairUpgrade.currentUpgradeLvl <= chairUpgrade.maxUpgradeLvl)
            {
                int myUpgradeLvl = chairUpgrade.currentUpgradeLvl;
                chairSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.chairs[myUpgradeLvl];
                // Spawn an fx object on the newlsy changed sprite
                Instantiate(itemDatabase.accentFX, Vector2.zero, Quaternion.identity, chairSlot.transform);
            }
            
        }
        // We're upgrading a MONITOR
        else if (thingToSpawn.buildingID == 3)
        {
            if (monitorUpgrade.currentUpgradeLvl <= monitorUpgrade.maxUpgradeLvl)
            {
                int myUpgradeLvl = monitorUpgrade.currentUpgradeLvl;
                monitorSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.monitors[myUpgradeLvl];
                // Spawn an fx object on the newlsy changed sprite
                Instantiate(itemDatabase.accentFX, Vector2.zero, Quaternion.identity, monitorSlot.transform);
            }
        }
    }

    public void SpawnUpgradedRig(Rig rigToSpawn, int uiSlot, bool spawnInThisSlot)
    {
        foreach (Transform slot in rigSlots)
        {
                if (spawnInThisSlot && slot.GetComponent<Rigslot>().myOrderNumber == uiSlot)
                {
                // This is how we "spawn" a rig by just making it active in the scene
                slot.GetChild(0).gameObject.SetActive(true);
                
                // Give this rig the data and change its icon to the correct one
                slot.GetComponentInChildren<RigScript>(true).me = rigToSpawn;
                slot.GetComponentInChildren<RigScript>(true).RefreshIcon();

                // This line sets up the pricing for consequtive rigs
                itemDatabase.rigTypes[1].priceOfNextUpgradeLvl = (slot.GetComponentInChildren<RigScript>(true).me.priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
                mapDelegateHolder.upgradedRigActions(uiSlot, slot.GetComponentInChildren<RigScript>(true).me);
                return;
                }
                // If this slot is not spawned in then just disable this rig and breakout
                else if (!spawnInThisSlot && slot.GetComponent<Rigslot>().myOrderNumber == uiSlot)
                {
                slot.GetChild(0).gameObject.SetActive(false);
                return;
                }
        }
        
    }
    public void UpgradeRackDirectly(int rigUpgradeLvl, int slotToUpgrade)
    {
        // Upgrades a rack directly by it's number in the slot list
        for(int i = 0; i < rackSlots.Count; i++) 
        {      
            if (rackSlots[i].childCount != 0 && i == slotToUpgrade)
            {
                // Collecting all the rigs in this rack into an array
                RigScript[] rigsInThisRack = rackSlots[i].GetComponentsInChildren<RigScript>();

                // Upgrading all the rigs in this rack one by one
                foreach(RigScript rigController in rigsInThisRack)
                {
                    rigController.me = itemDatabase.rigTypes[rigUpgradeLvl];
                    rigController.RefreshIcon();
                }
                Debug.Log("Uograding to " + itemDatabase.rigTypes[rigUpgradeLvl]);
                // Note the +4 on the slot upgrade because this is where the first rack slot starts at
                mapDelegateHolder.upgradedRackActions(itemDatabase.rigTypes[rigUpgradeLvl], slotToUpgrade + 4);
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
               
                if (rigscript.me.priceOfNextUpgradeLvl < miningControllerInstance.myMiningController.currentBalance)
                {

                    miningControllerInstance.myMiningController.currentBalance -= rigscript.me.priceOfNextUpgradeLvl;
                    float thisUpgradePrice = rigscript.me.priceOfNextUpgradeLvl;
                    //Debug.Log(thisUpgradePrice);
                    // TODO: this might break when getting to the last item in the list
                    rigscript.me = itemDatabase.rigTypes[rigscript.me.id + 1];
                    rigscript.RefreshIcon();


                    // Here we calculate the price of the next ugprade after the one we've just installed
                    rigscript.me.priceOfNextUpgradeLvl = (thisUpgradePrice * pricePercentageGrowth / 100);
                    //pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);
                    Rig currentRig = rigscript.me;

                    // MAKING THE NEW RIG ACTUALLY APPLY AN EFFECT TO THE MINING CONTROLLER
                    miningControllerInstance.IncreaseMinMaxMiningSpeed(currentRig.myEffectOnMining);

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
        // TODO: figure out whether this actually does anything
        if (!rackToUpgrade)
        {
            foreach (Transform slot in rackSlots)
            {
                //Checking my rack ID to find the one we want to upgrade
                if (slot.GetComponent<RackSlot>().myOrderNumber == rackSlot)
                {
                    //Debug.Log("About to upgrade this rack!");
                    RigScript[] rigslotsInThisRackGroup;
                    // Collecting all the RIGSCRIPTS inside this RACKSLOT to the update them
                    rigslotsInThisRackGroup = slot.GetComponentsInChildren<RigScript>(true);
                    //Debug.Log(rigslotsInThisRackGroup.Length);

                    // Find out if we have enough money to upgrade all the rigs in this group
                    if (rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length < miningControllerInstance.myMiningController.currentBalance)
                    {
                        // Charge me the money for the upgrade of the 3 rigs in the group
                        miningControllerInstance.myMiningController.currentBalance -= rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length;
                        //Debug.Log("Charged you " + rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length + " for the upgrades on the rack");

                        // The actual rig upgrade process
                        foreach (RigScript rig in rigslotsInThisRackGroup)
                        {
                            //Debug.Log(rig.me);
                            //Debug.Log("My rig id is now " + rig.me.id);
                            rig.me = itemDatabase.rigTypes[rig.me.id + 1];
                            //Debug.Log("My rig id is now " + rig.me.id);
                            rig.RefreshIcon();

                            

                        }
                        // Getting info from one of the rigs in the bunch
                        RigScript rigSample = rigslotsInThisRackGroup[0];
                        // APPLYING THE UPGRADE'S EFFECTS (this will be multiplied by 3, because there are 3 rigs in each rack
                        miningControllerInstance.IncreaseMinMaxMiningSpeed(rigSample.me.myEffectOnMining * 3);

                        //Send in the info to the UI so it can be udpated with the newly updated rig info for this rag
                        Debug.Log("Spawning a " + rigslotsInThisRackGroup[0].me);
                        mapDelegateHolder.upgradedRackActions(rigslotsInThisRackGroup[0].me, rackSlot);

                       
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

            if (rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length < miningControllerInstance.myMiningController.currentBalance)
            {
                // Charge me the money for the upgrade
                miningControllerInstance.myMiningController.currentBalance -= rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length;
                //Debug.Log("Charged you " + rigslotsInThisRackGroup[0].me.priceOfNextUpgradeLvl * rigslotsInThisRackGroup.Length + " for the upgrades on the rack");

                foreach (RigScript rig in rigslotsInThisRackGroup)
                {
                    Debug.Log(rig.me);
                    //Debug.Log("My rig id is now " + rig.me.id);
                    rig.me = itemDatabase.rigTypes[rig.me.id + 1];
                    //Debug.Log("My rig id is now " + rig.me.id);
                    rig.RefreshIcon();

                    miningControllerInstance.IncreaseMinMaxMiningSpeed(rig.me.myEffectOnMining);

                }
                    Debug.Log("Spawning a " + rigslotsInThisRackGroup[0].me);
                    mapDelegateHolder.upgradedRackActions(rigslotsInThisRackGroup[0].me, rackSlot);

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
        NotificationSystem.Instance.DisplayAChoiceNotification(firstRackInstallation, null, () => { SpawnAnItem(rackPrefab); KickOutThePartner(); });
        partnerKickedOut = true;
    }

    public void KickOutThePartner()
    {
        Destroy(partner.gameObject);
        
    }

    public void AskAboutFurniture()
    {
        NotificationSystem.Instance.DisplayAChoiceNotification(fifthRackInstallation, null, () => { SpawnAnItem(rackPrefab); GetRidOfFurniture(); });
        furnitureSold = true;
    }

    private void GetRidOfFurniture()
    {
        Destroy(livingRoom.gameObject);
        
    }

    #endregion  
}
