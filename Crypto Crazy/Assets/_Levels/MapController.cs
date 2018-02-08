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

    public Transform rackSlotsInThisApartment;

    public GameObject partner;
    public GameObject livingRoom;
    public GameObject cooling;




    private void Start()
    {
        foreach (Transform child in rackSlotsInThisApartment)
        {
            rackSlots.Add(child);
        }
    }

   
    // TODO: This has to be changed to accept an attribute of type "Building"
    // which will hold an id and a GO
    public void SpawnAnItem (Building thingToSpawn)
    {

        // We're spawning a RACK
        if (thingToSpawn.id == 0)
        {
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

    
    


   
   
}
