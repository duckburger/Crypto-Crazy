using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {


    public UpgradeTemplate rackUpgrade;


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
            if (slot.childCount == 0)
            {
                GameObject.Instantiate(rack, slot.position, Quaternion.identity, slot);
                return;
            }
            
        }
    }


   
}
