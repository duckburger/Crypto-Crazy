using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigController : MonoBehaviour {


    public int rigsOwned;
    

    public MapController currentMap;

    public MiningController miningController;


    // This is the percentage value that the rig price goes up by after the next upgrade
    public float pricePercentageGrowth;


    // This will get called whenever we spawn a new rig or upgrade an old one
    public delegate void OnRigSpawned (int rigSlot);
    public OnRigSpawned rigUpgradedActions;


    public delegate void OnRackSpawned(int rackSlot, Rack rackToUpgrade = null);
    public OnRackSpawned rackUpgradedActions;

    public delegate void OnSpecRackUpgraded(Rack rackToUpgrade);
    public OnSpecRackUpgraded specRackUpgradedActions;


    // Use this for initialization
    void Start()
    {
        miningController = FindObjectOfType<MiningController>();
    }


    public void UpgradeARig(int rigOrderNumber)
    {
        // Sends a message to the map controller
       
            rigUpgradedActions(rigOrderNumber);
  
    }

    public void UpgradeARack(int rackOrderNumber, Rack rackToUpgrade = null)
    {
        // TODO: Make this send an upgrade message to the map controller, and make this collect money from the MiningControlle
       
            rackUpgradedActions(rackOrderNumber);
       


    }

    public float CalculateCostOfNextUpgrade(float priceOfNextUpgradeLvl, float pricePercentageGrowth)
    {
        return (priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
    }
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
