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
                RigUI spawnedRackInfo = Instantiate(individualRackTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder).GetComponent<RigUI>();
                rackUIElementsSpawned++;
                spawnedRackInfo.slotText.text = "Rack Slot # " + (i + 1);
        }
    }

    private void PopulateRigsMenu()
    {
        for (int i = 0; i < currentMapController.maxRigs; i++)
        {
            RigUI spawnedRigUI = Instantiate(individualRigTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder).GetComponent<RigUI>();
            // Enable the very first rig because that one is always present
            if (i == 0)
            {
                spawnedRigUI.isEnabled = true;
            }
            spawnedRigUI.slotText.text = "Rig Slot # " + (i + 1);
            rigUIElementsSpawned++;
        }
    }
    
    public void RefreshMenuIDs()
    {
        currentMapController = FindObjectOfType<MapController>();
        
        for (int i = rackUIElementsSpawned; i < currentMapController.maxRacks; i++)
        {
            RigUI spawnedRackUI = Instantiate(individualRackTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder).GetComponent<RigUI>();
            spawnedRackUI.slotText.text = "Rack Slot # " + (i + 1);
            rackUIElementsSpawned++;
        }

        for (int i = rigUIElementsSpawned; i < currentMapController.maxRigs; i++)
        {
            RigUI spawnedRigUI = Instantiate(individualRigTemplate, Vector3.zero, Quaternion.identity, rigMenuHolder).GetComponent<RigUI>();
            spawnedRigUI.slotText.text = "Rig Slot # " + (i + 1);
            rigUIElementsSpawned++;
        }

        AssignIDToRigMenu();
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
            // Here we use "+4" because every single apartment will have 4 rigs available, 
            // so the count for racks starts at 5
            else if (child.GetComponent<RackID>() && i < currentMapController.maxRacks + 4)
            {
                child.GetComponent<RackID>().myControlID = i;
                i++;
            }
        }


    }

    
	
}
