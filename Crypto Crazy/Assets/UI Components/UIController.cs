using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour {


    public Text balanceText;
    public Text perSecondText;
    public TextMeshProUGUI currencyName;
    public MiningControllerTemplate myMiningController;

    public NumberConverter numberConverter;

    public Animator sideMenuAnimator;
    public Animator rigsMenuAnimator;
    public bool sideMenuShown;

    public delegate void OnAuxMenuOpened(int rackGroupID);
    public OnAuxMenuOpened openedAuxMenu;

    public delegate void OnAuxMenuClosed();
    public OnAuxMenuClosed closedAuxMenu;


    private void Awake()
    {
        if (myMiningController.currencyName.Length < 3)
        {
            myMiningController.currencyName = "Sweetcoins";
        } 

    }


    // Use this for initializations
    void Start () {
        currencyName.text = myMiningController.currencyName;

	}
    public void ShowRacksSideMenu(int rackGroupOrderNumber = 0)
    {
        if (!sideMenuShown)
        {
            rigsMenuAnimator.gameObject.SetActive(true);
            sideMenuShown = true;
            rigsMenuAnimator.SetBool("SlideOnMenu", false);
            sideMenuAnimator.SetBool("ShowMenu", true);

            // Should call a delegate to make sure that will trigger the aux menu to show the right amount of racks
            openedAuxMenu(rackGroupOrderNumber);

            return;
        }

        HideRackSideMenu();
    }

    public void HideRackSideMenu()
    {
        sideMenuShown = false;
        rigsMenuAnimator.SetBool("SlideOnMenu", true);
        //sideMenuAnimator.SetBool("ShowMenu", false);

        closedAuxMenu();


        return;

    }

    public void HideJustSideRackMenu()
    {
        sideMenuShown = false;

        //sideMenuAnimator.SetBool("ShowMenu", false);

        


        return;
    }


	
	// Update is called once per frame
	void Update () {


        balanceText.text = numberConverter.ConvertNumber(myMiningController.currentBalance);
    
        perSecondText.text = numberConverter.ConvertNumber(myMiningController.coinsPerSec) + "/s";
        
        
	}
}
