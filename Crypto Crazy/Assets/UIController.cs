using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {


    public Text balanceText;
    public Text perSecondText;
    public MiningControllerTemplate myMiningController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        balanceText.text = myMiningController.currencyMined.ToString("n2");
        perSecondText.text = Mathf.Ceil(myMiningController.coinsPerSec) + "/s";
	}
}
