using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigController : MonoBehaviour {


    public int rigsOwned;
    public Rig thisRig;
    

    public MapController currentMap;



    // This is the percentage value that the rig price goes up by after the next upgrade
    public float pricePercentageGrowth;


    // This will get called whenever we spawn a new rig or upgrade an old one
    delegate void OnRigSpawned (int rigId, int rigSlot);
    OnRigSpawned rigSpawnedActions;


    private void OnEnable()
    {
        
    }

    public void UpgradeARig(int rigIDInHierarchy)
    {
        // TODO: Make this send an upgrade message to the map controller, and make this collect money from the MiningController
    }

    public void UpgradeARack()
    {
        // TODO: Make this send an upgrade message to the map controller, and make this collect money from the MiningController
    }

    public float CalculateCostOfNextUpgrade(float priceOfNextUpgradeLvl, float pricePercentageGrowth)
    {
        return (priceOfNextUpgradeLvl * pricePercentageGrowth / 100);
    }


    

   

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
