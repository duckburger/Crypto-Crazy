using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxMenu : MonoBehaviour {

    public UIController mainUIController;
    public MapController currentMapController;

    public Transform auxMenuItemsHolder;

    public List<Rack> listOfRacksInMyGroup = new List<Rack>();


    // Use this for initialization
    void Start () {
        // We need this because it will help control what we show in the Aux Menu
        mainUIController = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIController>();
        currentMapController = FindObjectOfType<MapController>();
        currentMapController = FindObjectOfType<MapController>();
        
        mainUIController.openedAuxMenu += PopulateAuxMenu;
        mainUIController.closedAuxMenu += DisableAuxMenu;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PopulateAuxMenu(int rackGroupID)
    {
        // Activate the right amount of racks
        
        RigUI[] rigUIElements = auxMenuItemsHolder.GetComponentsInChildren<RigUI>(true);

        foreach(Transform rack in currentMapController.rackSlots[rackGroupID])
        {
            listOfRacksInMyGroup.Add(rack.GetComponent<Rack>());
        }

        for (int i = 0; i < rigUIElements.Length; i++)
        {
            if (i < currentMapController.racksPerGroup)
            {
                rigUIElements[i].gameObject.SetActive(true);
                rigUIElements[i].controllingRack = true;
                rigUIElements[i].isEnabled = false;
                rigUIElements[i].activateButton.gameObject.SetActive(false);
            }
            else
            {
                rigUIElements[i].gameObject.SetActive(false);
            }

        }
       
        int amountOfRacksToActivate = currentMapController.racksPerGroup;

        for (int i = 0; i < amountOfRacksToActivate; i++)
        {

            if (i == rackGroupID)
            {
                // Found the rack slot we want to pull data from, so we tell the UI what amount of racks 
                // we want to activate based on the amount and fill out their info

                rigUIElements[i].isEnabled = true;
                rigUIElements[i].myRig = currentMapController.rackSlots[i].GetComponent<RackSlot>().GetComponentInChildren<RigScript>(true).me;
                rigUIElements[i].GetComponent<RackID>().myControlID = currentMapController.rackSlots[i].GetComponent<RackSlot>().myOrderNumber;

                rigUIElements[i].InitizalizeTheUI();


                rigUIElements[i].UpdateMyRackUI(currentMapController.rackSlots[i].GetComponent<RackSlot>().GetComponentInChildren<RigScript>(true).me, currentMapController.racksPerGroup, i);

                

            }

        }
    }

   

    public void DisableAuxMenu()
    {
        RigUI[] rigUIElements = auxMenuItemsHolder.GetComponentsInChildren<RigUI>();

        for (int i = 0; i < rigUIElements.Length; i++)
        {

            rigUIElements[i].isEnabled = false;
            rigUIElements[i].controllingRack = false;
            rigUIElements[i].gameObject.SetActive(false);


        }
    }
}
