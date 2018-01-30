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

    public void AddPercentageToMiningSpeed (float percentageAmount)
    {
        myMiningController.minCoinsPerSec += (myMiningController.minCoinsPerSec / 100 * percentageAmount );
        myMiningController.maximumCoinsPerSec += (myMiningController.maximumCoinsPerSec / 100 * percentageAmount );
    }



    // Update is called once per frame
    void Update () {

        

        myMiningController.currencyMined += myMiningController.coinsPerSec * Time.deltaTime;
        myMiningController.coinsPerSec = Mathf.Clamp(myMiningController.coinsPerSec, myMiningController.minCoinsPerSec, myMiningController.maximumCoinsPerSec);






    }
}
