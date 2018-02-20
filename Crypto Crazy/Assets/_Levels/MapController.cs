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

    // This will get called whenever we finish updating a rig (sends a message to UI, for example)
    public delegate void OnRigUpgraded(int rigSlot, Rig newlyInstalledRig);
    public OnRigUpgraded upgradedRigActions;


    public delegate void OnRackUpgraded(int rackSlot, Rack newlyInstalledRack);
    public OnRackUpgraded upgradedRackActions;

    public float pricePercentageGrowth;



    private void Start()
    {
        rigController = FindObjectOfType<RigController>();
        itemDatabase = GetComponent<ItemDatabase>();
        miningControllerInstance = FindObjectOfType<MiningController>();
    }


    private void OnEnable()
    {

        rigController = FindObjectOfType<RigController>();

        // Populating the rack slots list
        int i = 0;
        foreach (Transform child in rackSlotHolder)
        {
            rackSlots.Add(child);
            child.GetComponent<RackSlot>().myOrderNumber = i;
            i++;

        }

        // Populating the rig slot list
        int j = 0;
        foreach (Transform child in rigSlotHolder)
        {
            rigSlots.Add(child);
            child.GetComponent<Rigslot>().myOrderNumber = j;
            j++;
        }

        rigController.rigSpawnedActions += UpgradeARig;
        rigController.rigSpawnedActions += UpgradeARackOfRigs;

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

            foreach (Transform slot in rackSlots)
            {
                // This doesn't need a second option, because the Upgrade script already checks whether to let the player press the button
                if (slot.childCount == 0)
                {
                   
                    GameObject.Instantiate(thingToSpawn.myBuildingPrefab, slot.position, Quaternion.identity, slot);
                    
                    return;
                }

            }
        } else if (thingToSpawn.buildingID == 1)
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
                    rigscript.me = itemDatabase.rigTypes[rigscript.me.buildingID + 1];
                    rigscript.RefreshIcon();


                    // Here we calculate the price of the next ugprade after the one we've just installed
                    rigscript.me.priceOfNextUpgradeLvl = (thisUpgradePrice * pricePercentageGrowth / 100);
                    //pricePercentageGrowth -= (pricePercentageGrowth * 20 / 100);
                    Rig currentRig = rigscript.me;

                    // TODO: MAKE THE NEW RIG ACTUALLY APPLY AN EFFECT TO THE MINING CONTROLLER
                    miningControllerInstance.AddPercentageToMiningSpeed(currentRig.myEffectOnMining);
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

    }

    public void AskAboutPartner()
    {
        NotificationSystem.Instance.DisplayAChoiceNotification(firstRackInstallation, null, () => { SpawnAnItem(rackBuildingObject); KickOutThePartner(); });
        partnerKickedOut = true;
    }

    public void KickOutThePartner()
    {
        Destroy(partner.gameObject);
    }

    
    


   
   
}
