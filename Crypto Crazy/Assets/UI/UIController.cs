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

	// Use this for initialization
	void Start () {
        currencyName.text = myMiningController.currencyName;
	}
	
	// Update is called once per frame
	void Update () {


        balanceText.text = numberConverter.ConvertNumber(myMiningController.currencyMined);
    
        perSecondText.text = numberConverter.ConvertNumber(myMiningController.coinsPerSec) + "/s";
        
        
	}
}
