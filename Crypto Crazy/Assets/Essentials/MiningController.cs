using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningController : MonoBehaviour {


    public MiningControllerTemplate myMiningController;

    private void Start()
    {
    }


    private void LateUpdate()
    {
        
        if (myMiningController.coinsPerSec > myMiningController.minCoinsPerSec)
            myMiningController.coinsPerSec -= Time.deltaTime * myMiningController.decreaseSpeed;

        if (myMiningController.coinsPerSec == 1)
            myMiningController.decreaseSpeed = myMiningController.defDecSpeed;

        

    }

    public void IncreaseMinimumMininSpeed(float amount)
    {
        myMiningController.minCoinsPerSec += amount;
        
    }

    public void DecreaseMinimumMiningSpeed(float amount)
    {
        myMiningController.minCoinsPerSec -= amount;
    }

    public void IncreaseMaximumMiningSpeed(float amount)
    {
        myMiningController.maximumCoinsPerSec += amount;
    }

    public void DecreaseMaximumMiningSpeed(float amount)
    {
        myMiningController.maximumCoinsPerSec -= amount;
    }


    // Update is called once per frame
    void Update () {

        

        myMiningController.currencyMined += myMiningController.coinsPerSec * Time.deltaTime;
        myMiningController.coinsPerSec = Mathf.Clamp(myMiningController.coinsPerSec, myMiningController.minCoinsPerSec, myMiningController.maximumCoinsPerSec);






    }
}
