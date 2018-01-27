using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningController : MonoBehaviour {


    public MiningControllerTemplate myMiningController;

    [SerializeField]
   

    private void LateUpdate()
    {
        
        if (myMiningController.coinsPerSec > 1)
            myMiningController.coinsPerSec -= Time.deltaTime * myMiningController.decreaseSpeed;

        if (myMiningController.coinsPerSec == 1)
            myMiningController.decreaseSpeed = myMiningController.defDecSpeed;

        

    }

    // Update is called once per frame
    void Update () {

        myMiningController.coinsPerSec = Mathf.Clamp(myMiningController.coinsPerSec, 1, myMiningController.maximumCoinsPerSec);

        myMiningController.currencyMined += myMiningController.coinsPerSec * Time.deltaTime;

        

	}
}
