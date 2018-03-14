using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIDAssigner : MonoBehaviour {

    // Need this reference to know how many racks we are controlling per section
    public MapController currentMapController;
    public Transform rigMenuHolder;

    public GameObject rackGroupTemplate;
    public GameObject individualRackTemplate;

    private void Awake()
    {

        currentMapController = FindObjectOfType<MapController>();
        PopulateTheRigsMenu();
        AssignIDToRigMenu();
    }

    
   
    // This spawns the UI elements for racks into the rigs menu
    private void PopulateTheRigsMenu()
    {
        
            for (int i = 0; i < currentMapController.maxRacks; i++)
            {
                Instantiate(individualRackTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder);
            }
        
       


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
            } else if (!currentMapController.controlsRacksByOne && child.GetComponent<RackID>() && i < 16)
            {

            }
        }


    }

    
	
}
