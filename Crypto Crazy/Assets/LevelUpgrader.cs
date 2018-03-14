using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpgrader : MonoBehaviour {

    // Stores the level that is currently active in the game
    public MapController currentLvlData;

    // Stores the new level we will be upgrading to
    public MapController newLvlData;

    private ItemDatabase itemDatabase;

    // Some data from the current level that will be stored
    // to carry over into the newly spawned level
    public int racksAmount;
    public int chairUpgrLvl;
    public int deskUpgrLvl;
    public int monUpgrLvl;
    public int coolUpgrLvl;
    public int hamUpgrLvl;


    public CameraController cameraController;


	// Use this for initialization
	void Start () {
        currentLvlData = FindObjectOfType<MapController>();
        itemDatabase = FindObjectOfType<ItemDatabase>();
    }

    // This method upgrades the current lvl to the prefab passed into it
    public void UpgradeToDifferentLvl(GameObject newLvlPrefab)
    {
        //Collect the numbers from the existing apartment
        CollectData();
        //Spawn the new apartment into the scene in the same position as the old one
        SpawnNewLvl(newLvlPrefab);
        //Assigning the numbers and spawning the same amount of items as in the previous apartment
        AssignDataAndSpawnItems();

        //TODO: add a funciton that will propogate the new apartment into all the classes that use the MapController
    }

    private void AssignDataAndSpawnItems()
    {
        // Giving the new map the data amount the amount of racks
        newLvlData.racksBuilt = racksAmount;
        // Spawning the required amount of racks
        for (int i = 0; i <= racksAmount; i++)
        {
            newLvlData.SpawnAnItem(itemDatabase.basicRackItem);
        }

        int chairUpgrLvl = newLvlData.chairUpgrade.currentUpgradeLvl;
        newLvlData.chairSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.chairs[chairUpgrLvl];

        int deskUpgrLvl = newLvlData.deskUpgrade.currentUpgradeLvl;
        newLvlData.deskSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.desks[deskUpgrLvl];

        int monUpgrLvl = newLvlData.monitorUpgrade.currentUpgradeLvl;
        newLvlData.monitorSlot.GetComponent<SpriteRenderer>().sprite = itemDatabase.desks[monUpgrLvl];

        int coolUpgrLvl = newLvlData.coolingUpgrade.currentUpgradeLvl;
        newLvlData.cooling.GetComponent<SpriteRenderer>().sprite = itemDatabase.coolSystems[coolUpgrLvl];

        int hamUpgrLvl = newLvlData.hamsterUpgrade.currentUpgradeLvl;
        newLvlData.hamster.GetComponent<SpriteRenderer>().sprite = itemDatabase.hamsters[hamUpgrLvl];


    }


    private void SpawnNewLvl(GameObject lvlToSpawn)
    {
        newLvlData = Instantiate(lvlToSpawn, currentLvlData.transform.position, Quaternion.identity).GetComponent<MapController>();
    }

    private void CollectData()
    {
        racksAmount = currentLvlData.racksBuilt;
        chairUpgrLvl = currentLvlData.chairUpgrade.currentUpgradeLvl;
        deskUpgrLvl = currentLvlData.deskUpgrade.currentUpgradeLvl;
        monUpgrLvl = currentLvlData.monitorUpgrade.currentUpgradeLvl;
        coolUpgrLvl = currentLvlData.coolingUpgrade.currentUpgradeLvl;
        hamUpgrLvl = currentLvlData.hamsterUpgrade.currentUpgradeLvl;

        // Note: camera boundries for each lvl are stored on the level itself!
    }
}
