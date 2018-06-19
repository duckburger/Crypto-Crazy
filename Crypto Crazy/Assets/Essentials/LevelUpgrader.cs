using System.Collections.Generic;
using UnityEngine;

public class LevelUpgrader : MonoBehaviour {

    // Stores the level that is currently active in the game
    [SerializeField] MapController currentLvlData;

    // Stores the new level we will be upgrading to
    public MapController newLvlData;

    public UIController uiController;
    public MenuController menuController;
    public MiningController miningController;
    public MenuIDAssigner menuIDAssigner;
    private ItemDatabase itemDatabase;

    // Some data from the current level that will be stored
    // to carry over into the newly spawned level
    public int racksAmount;
    public int rigsAmount;

    public int chairUpgrLvl;
    public int deskUpgrLvl;
    public int monUpgrLvl;
    public int coolUpgrLvl;
    public int hamUpgrLvl;

    public CameraController cameraController;

    private bool partnerKickedOut;
    private bool furnitureSold;

    [SerializeField] List<Rig> oldRigs = new List<Rig>();
    [SerializeField] List<Rig> oldRacks = new List<Rig>();
    
    [SerializeField] RigUI[] rigUIElements;
    [SerializeField] Upgrade[] upgradeButtons;

    [SerializeField] GameEvent levelUpgradedEvent;
   

    public delegate void OnLevelSuccessfullyUpgraded(ApartmentUpgrade UItoUpdate);
    public OnLevelSuccessfullyUpgraded upgradedApartment;


    // Use this for initialization
    void Start () {
        currentLvlData = FindObjectOfType<MapController>();
        itemDatabase = FindObjectOfType<ItemDatabase>();
        itemDatabase.rackUpgrade.maxUpgradeLvl = currentLvlData.maxRacks;
        
    }

    // This method upgrades the current lvl to the prefab passed into it
    public void UpgradeToDifferentLvl(GameObject newLvlPrefab, ApartmentUpgrade UIToUpdate)
    {
        //Check whether we have enough money to upgrade to this apartment
        if (miningController.myMiningController.currentBalance > UIToUpdate.myApartment.myPrice)
        {
            
            //Collect the numbers from the existing apartment
            CollectData();
            //Spawn the new apartment into the scene in the same position as the old one
            SpawnNewApartment(newLvlPrefab);

           
            //Assigning the numbers and spawning the same amount of items as in the previous apartment
            AssignDataAndSpawnItems();


            //Delete the old apartment
            DeleteOldApartment();

            PropogateNewMap();

            //Setting the new lvl data to the current lvl data
            currentLvlData = newLvlData;
            newLvlData = null;

            // Run the upgraded apartment delegate
            upgradedApartment(UIToUpdate);
            levelUpgradedEvent.Raise();
        }
        return;


    }

    private void PropogateNewMap()
    {
        rigUIElements = uiController.rigsMenuAnimator.GetComponentsInChildren<RigUI>(true);
        itemDatabase.rackUpgrade.maxUpgradeLvl = newLvlData.maxRacks;

        menuIDAssigner.RefreshMenuIDs();
        cameraController.RefreshForNewApt();

        // Propogating the new mapcontroller to all the rig buttons
        foreach (RigUI rigUIScript in rigUIElements) 
        {
            rigUIScript.ResetDataForNewApartment();
        }

        // Propogating the new mapcontroller to all the upgrade buttons
        upgradeButtons = menuController.upgradesMenu.GetComponentsInChildren<Upgrade>(true);

        foreach (Upgrade upgradeBtnController in upgradeButtons)
        {
            upgradeBtnController.ResetDataForNewApartment();
        }
    }

    private void DeleteOldApartment()
    {
        Destroy(currentLvlData.gameObject);
        currentLvlData = newLvlData;
    }

    private void SpawnNewApartment(GameObject lvlToSpawn)
    {
        newLvlData = Instantiate(lvlToSpawn, currentLvlData.transform.position, Quaternion.identity).GetComponent<MapController>();
    }

    private void AssignDataAndSpawnItems()
    {
        // Giving the new map the data amount the amount of racks and rigs
        newLvlData.partnerKickedOut = partnerKickedOut;
        newLvlData.furnitureSold = furnitureSold;

        // Spawning the required amount of racks
        if (racksAmount > 0)
        {
            newLvlData.partner.SetActive(false);
            if (racksAmount >= 5)
                newLvlData.livingRoom.SetActive(false);

            for (int i = 0; i < racksAmount; i++)
            {
                newLvlData.SpawnAnItem(itemDatabase.basicRackItem);
                // Upgrading the spawned rack here if it has upgraded rigs in it
                int rackUpgradeLvl = oldRacks[i].id;
                newLvlData.UpgradeRackDirectly(rackUpgradeLvl, i);        
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
                    newLvlData.SpawnUpgradedRig(rig, i, false);
                    i++;
                    continue;
                }
               
            }

          // Setting the correct sprites for all items
          // TODO: implement here
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
            // If the rigscript item is active in the hierarchy, count it as a spawned rig
            if (rigSlot.GetComponentInChildren<RigScript>())
            {
                oldRigs.Add(rigSlot.GetComponentInChildren<RigScript>().me); 
               
            }
            else // Otherwise count it as an empty rig - this works later when we spawn and empty item so the menu appearance is kept
            {
                oldRigs.Add(null);
            }
        }
        // Fill out the racks list to carry it over
        foreach (Transform rackSlot in currentLvlData.rackSlots)
        {
            if (rackSlot.GetComponentInChildren<RigScript>())
            {
                oldRacks.Add(rackSlot.GetComponentInChildren<RigScript>().me);
            }
            else
            {
                oldRigs.Add(null);
            }

        }


        // TODO: make sure that the sprites for all the upgrades below update to the correct level in the new apartment
        // also, make sure that the rack upgrade UI gets synced with the new max racks number
        chairUpgrLvl = currentLvlData.chairUpgrade.currentUpgradeLvl;
        deskUpgrLvl = currentLvlData.deskUpgrade.currentUpgradeLvl;
        monUpgrLvl = currentLvlData.monitorUpgrade.currentUpgradeLvl;
        coolUpgrLvl = currentLvlData.coolingUpgrade.currentUpgradeLvl;
        hamUpgrLvl = currentLvlData.hamsterUpgrade.currentUpgradeLvl;

        partnerKickedOut = currentLvlData.partnerKickedOut;
        furnitureSold = currentLvlData.furnitureSold;

        // Note: camera boundries for each lvl are stored on the level itself!
    }

    
}
