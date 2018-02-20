using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIDAssigner : MonoBehaviour {

    // Need this reference to know how many racks we are controlling per section
    public MapController currentMapController;
    public Transform rigMenuHolder;


    private void Awake()
    {
        AssignIDToRigMenu();
    }

    private void OnEnable()
    {
        currentMapController = FindObjectOfType<MapController>();

        
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
            } else if (child.GetComponent<RackID>() && i < 16)
            {
                child.GetComponent<RackID>().myControlID = i;
                i++;
            }
        }


    }

    
	
}
