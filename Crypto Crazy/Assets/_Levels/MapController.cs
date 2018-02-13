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

    // This event will send the info up to the UI to unlock a new rack slot, when we install a new rack
    delegate void NewRackInstalled(int newRackID);
    NewRackInstalled RackInstalledActions;




    private void OnEnable()
    {
        // Populating the rack
        foreach (Transform child in rackSlotHolder)
        {
            rackSlots.Add(child);
        }


        foreach (Transform child in rigSlotHolder)
        {
            rigSlots.Add(child);
        }
    }

   
    // TODO: This has to be changed to accept an attribute of type "Building"
    // which will hold an id and a GO
    public void SpawnAnItem (Building thingToSpawn)
    {
        // We're spawning a RACK
        if (thingToSpawn.id == 0)
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
                    if (rackUpgrade.currentUpgradeLvl <= 0)
                    {

                    }

                    GameObject.Instantiate(thingToSpawn.myBuildingPrefab, slot.position, Quaternion.identity, slot);
                    
                    return;
                }

            }
        }
        
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
