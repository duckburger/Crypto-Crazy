using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIDAssigner : MonoBehaviour {

    // Need this reference to know how many racks we are controlling per section
    public MapController currentMapController;
    public Transform rigMenuHolder;

    public GameObject individualRigTemplate;
    public GameObject individualRackTemplate;

    [SerializeField] private int rackUIElementsSpawned;
    [SerializeField] private int rigUIElementsSpawned;

    private void Awake()
    {
        currentMapController = FindObjectOfType<MapController>();
        PopulateRigsMenu();
        PopulateRacksMenu();
        AssignIDToRigMenu();
    }

    
   
    // This spawns the UI elements for racks into the rigs menu
    private void PopulateRacksMenu()
    {       
            for (int i = 0; i < currentMapController.maxRacks; i++)
            {
                Instantiate(individualRackTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder);
                rackUIElementsSpawned++;
            }
    }

    private void PopulateRigsMenu()
    {
        for (int i = 0; i < currentMapController.maxRigs; i++)
        {
            RigUI spawnedRigInfo = Instantiate(individualRigTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder).GetComponent<RigUI>();
            if (i == 0)
            {
                spawnedRigInfo.isEnabled = true;
            }
            rigUIElementsSpawned++;
        }
    }
    
    public void RefreshMenuIDs()
    {
        currentMapController = FindObjectOfType<MapController>();
        
        for (int i = rackUIElementsSpawned; i < currentMapController.maxRacks; i++)
        {
            Instantiate(individualRackTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder);
            rackUIElementsSpawned++;
        }

        for (int i = rigUIElementsSpawned; i < currentMapController.maxRigs; i++)
        {
            Instantiate(individualRigTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder);
            rigUIElementsSpawned++;
        }

        AssignIDToRigMenu();


        //int j = 4;
        //foreach (Transform child in rigMenuHolder)
        //{
        //    if (child.GetComponent<RackID>() && j < currentMapController.maxRacks + 4)
        //    {
        //        child.GetComponent<RackID>().myControlID = j;
        //        j++;
        //    }
        //}
    }

    

    public void AssignIDToRigMenu()
    {
        int i = 0;
        

        foreach (Transform child in rigMenuHolder)
        {
            if (child.GetComponent<RigID>())
            {
                child.GetComponent<RigID>().myControlID = i;
                i++;
            }

            // Here we use "+4" because every single apartment will have 4 rigs available.
            else if (child.GetComponent<RackID>() && i < currentMapController.maxRacks + 4)
            {
                child.GetComponent<RackID>().myControlID = i;
                i++;
            }
        }


    }

    
	
}
