using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpgrader : MonoBehaviour {

    // Stores the level that is currently active in the game
    public MapController currentLvlData;

    // Stores the new level we will be upgrading to
    public MapController newLvlData;

    public UIController uiController;
   

    // Some data from the current level that will be stored
    // to carry over into the newly spawned level
    public int racksAmount;
    public int rigsAmount;

    public List<Rig> oldRigs = new List<Rig>();
    [SerializeField] RigUI[] rigUIElements;

    public int chairUpgrLvl;
    public int deskUpgrLvl;
    public int monUpgrLvl;
    public int coolUpgrLvl;
    public int hamUpgrLvl;


    public CameraController cameraController;


    private ItemDatabase itemDatabase;
   
   
    // Use this for initialization
    void Start () {
        currentLvlData = FindObjectOfType<MapController>();
        itemDatabase = FindObjectOfType<ItemDatabase>();
    }

    // This method upgrades the current lvl to the prefab passed into it
    public void UpgradeToDifferentLvl(GameObject newLvlPrefab)
    {
        //Debug.Log("Upgrading to a new lvl");
        //Collect the numbers from the existing apartment
        CollectData();
        //Spawn the new apartment into the scene in the same position as the old one
        SpawnNewLvl(newLvlPrefab);
        //Assigning the numbers and spawning the same amount of items as in the previous apartment
        AssignDataAndSpawnItems();

        PropogateNewMap();

        //Delete the old apartment
        DeleteOldApartment();

        //Setting the new lvl data to the current lvl data
        currentLvlData = newLvlData;
        newLvlData = null;


    }

    private void PropogateNewMap()
    {
        rigUIElements = uiController.rigsMenuAnimator.GetComponentsInChildren<RigUI>(true);

        foreach(RigUI rigUIScript in rigUIElements)
        {
            rigUIScript.ResetDataForNewApartment();
        }   
    }

    private void DeleteOldApartment()
    {
        Destroy(currentLvlData.gameObject);
        currentLvlData = newLvlData;
    }

    private void SpawnNewLvl(GameObject lvlToSpawn)
    {
        newLvlData = Instantiate(lvlToSpawn, currentLvlData.transform.position, Quaternion.identity).GetComponent<MapController>();
    }

    private void AssignDataAndSpawnItems()
    {
        // Giving the new map the data amount the amount of racks and rigs
        newLvlData.racksBuilt = racksAmount;
        newLvlData.rigsBuilt = rigsAmount;

        


        

        // Spawning the required amount of racks
        if (racksAmount > 0)
        {
            for (int i = 0; i <= racksAmount; i++)
            {
                newLvlData.SpawnAnItem(itemDatabase.basicRackItem);
            }
        }

        // Spawning the required amount of rigs
        if (rigsAmount > 0)
        {
            int i = 0;
            // Spawn each rig from the old apartment into the new one, with the correct upgrade level
            foreach(Rig rig in oldRigs)
            {
                if (rig != null)
                {
                    // This needs to be upgraded with a method that would spawn a leveled up rig
                    newLvlData.SpawnUpgradedRig(rig, i, true);
                    i++;
                } else
                {
                    Debug.Log("Spawning in the " + i + " slot");
                    newLvlData.SpawnUpgradedRig(rig, i, false);
                    i++;
                    continue;
                }
               
            }
        }


        int chairUpgrLvl = newLvlData.chairUpgrade.currentUpgradeLvl;
        newLvlData.chairSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.chairs[chairUpgrLvl];

        int deskUpgrLvl = newLvlData.deskUpgrade.currentUpgradeLvl;
        newLvlData.deskSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.desks[deskUpgrLvl];

        int monUpgrLvl = newLvlData.monitorUpgrade.currentUpgradeLvl;
        newLvlData.monitorSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.monitors[monUpgrLvl];

        int coolUpgrLvl = newLvlData.coolingUpgrade.currentUpgradeLvl;
        newLvlData.cooling.GetComponent<SpriteRenderer>().sprite = itemDatabase.coolSystems[coolUpgrLvl];

        int hamUpgrLvl = newLvlData.hamsterUpgrade.currentUpgradeLvl;
        newLvlData.hamster.GetComponent<SpriteRenderer>().sprite = itemDatabase.hamsters[hamUpgrLvl];


    }


    

    private void CollectData()
    {
        racksAmount = currentLvlData.racksBuilt;
        rigsAmount = currentLvlData.rigsBuilt;

        // Fill out the rigs list to carry it over
        foreach(Transform rigSlot in currentLvlData.rigSlots)
        {
           
            if (rigSlot.GetComponentInChildren<RigScript>())
            {
                oldRigs.Add(rigSlot.GetComponentInChildren<RigScript>().me);
                
               
            } else
            {
                oldRigs.Add(null);
            }

        } 

        chairUpgrLvl = currentLvlData.chairUpgrade.currentUpgradeLvl;
        deskUpgrLvl = currentLvlData.deskUpgrade.currentUpgradeLvl;
        monUpgrLvl = currentLvlData.monitorUpgrade.currentUpgradeLvl;
        coolUpgrLvl = currentLvlData.coolingUpgrade.currentUpgradeLvl;
        hamUpgrLvl = currentLvlData.hamsterUpgrade.currentUpgradeLvl;

        // Note: camera boundries for each lvl are stored on the level itself!
    }

    
}
