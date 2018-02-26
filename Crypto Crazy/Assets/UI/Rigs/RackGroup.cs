using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RackGroup : MonoBehaviour {

    public bool isEnabled;
    public bool fullyUpgraded;
    public Image darkOverlay;

    public RigController rigController;
    public MapController currentMapController;
    public MiningController miningController;
    public UIController mainUIController;

    
    public int racksControlled;


    // All the text objects we will need to adjust every upgrade 
    public Text title;
    public Text description;
  
    // The 2 buttons on this UI
    public Button unfoldButton;
    public Button upgradeAllButton;

    // For changing the text/image of the buttons
    public Text upgradeAllButtonText;

    // For controlling the upgrade level bar
    public Image upgradeLvlUI;

   

    // Use this for initialization
    void Start () {

        rigController = FindObjectOfType<RigController>();
        miningController = FindObjectOfType<MiningController>();
        currentMapController = FindObjectOfType<MapController>();
        mainUIController = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIController>();

        unfoldButton.onClick.AddListener(() => mainUIController.ShowRacksSideMenu());


    }
	
	// Update is called once per frame
	void Update () {
        


	}
}
