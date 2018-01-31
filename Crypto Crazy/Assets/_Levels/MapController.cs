using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {


    public UpgradeTemplate rackUpgrade;
    public UpgradeTemplate chairUpgrade;
    public UpgradeTemplate deskUpgrade;
    public UpgradeTemplate monitorUpgrade;

    public MiningController miningControllerInstance;
    public MiningControllerTemplate myMiningController;


    public List<Transform> rackSlots = new List<Transform>();

    public Transform rackSlotsInThisApartment;

    public GameObject partner;
    public GameObject livingRoom;

    public GameObject rack;

    private void Start()
    {
        foreach (Transform child in rackSlotsInThisApartment)
        {
            rackSlots.Add(child);
        }
    }

    


    public void InstallRack()
    {
        Debug.Log("Purchased a new rack");
        rackUpgrade.currentUpgradeEffect += 1;

        foreach(Transform slot in rackSlots)
        {
            // This doesn't need a second option, because the Upgrade script already checks whether to let the player press the button
            if (slot.childCount == 0)
            {
                GameObject.Instantiate(rack, slot.position, Quaternion.identity, slot);
                return;
            }
            
        }
    }

    public void InstallChair()
    {
        Debug.Log("Purchased a new chair");

        miningControllerInstance.AddPercentageToMiningSpeed(chairUpgrade.effectsForEachUpgradeLvl[chairUpgrade.currentUpgradeLvl]);
    }

    public void InstallDesk()
    {
        Debug.Log("Purchased a new desk");

        miningControllerInstance.AddPercentageToMiningSpeed(deskUpgrade.effectsForEachUpgradeLvl[deskUpgrade.currentUpgradeLvl]);

    }

    public void InstallMonitor()
    {
        Debug.Log("Purchased a new monitor");

        miningControllerInstance.AddPercentageToMiningSpeed(monitorUpgrade.effectsForEachUpgradeLvl[monitorUpgrade.currentUpgradeLvl]);

    }

   
}
