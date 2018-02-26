using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {


    public Text balanceText;
    public Text perSecondText;
    public Text currencyName;
    public MiningControllerTemplate myMiningController;

    public NumberConverter numberConverter;

    public Animator sideMenuAnimator;
    public Animator rigsMenuAnimator;
    public bool sideMenuShown;

	// Use this for initializations
	void Start () {
        currencyName.text = myMiningController.currencyName;

	}


    public void ShowRacksSideMenu()
    {
        if (!sideMenuShown)
        {
            sideMenuShown = true;
            rigsMenuAnimator.SetTrigger("SlideOff");
            sideMenuAnimator.SetTrigger("ShowMyRacks");
            
            
            return;
        }

        HideRackSideMenu();
    }

    public void HideRackSideMenu()
    {
        sideMenuShown = false;
        rigsMenuAnimator.SetTrigger("SlideOn");
        sideMenuAnimator.SetTrigger("HideMyRacks");
        
        
    }
	
	// Update is called once per frame
	void Update () {


        balanceText.text = numberConverter.ConvertNumber(myMiningController.currencyMined);
    
        perSecondText.text = numberConverter.ConvertNumber(myMiningController.coinsPerSec) + "/s";
        
        
	}
}
