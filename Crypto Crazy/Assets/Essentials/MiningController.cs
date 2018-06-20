using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningController : MonoBehaviour {


    public MiningControllerTemplate myMiningController;

    

    private void LateUpdate()
    {
        
        if (myMiningController.coinsPerSec > myMiningController.minCoinsPerSec)
        {
            myMiningController.coinsPerSec -= Time.deltaTime * myMiningController.decreaseSpeed;
        }
            

        if (myMiningController.coinsPerSec == 1)
        {
            myMiningController.decreaseSpeed = myMiningController.defDecSpeed;
        }

        //if (myMiningController.coinsPerSec > myMiningController.minCoinsPerSec)
        //{
        //    if (myMiningController.coinsPerSec > 0 && myMiningController.coinsPerSec <= 10)
        //    {
        //        myMiningController.coinsPerSec -= myMiningController.decreaseSpeed;
        //    }
        //    else if (myMiningController.coinsPerSec > 10 && myMiningController.coinsPerSec <= 50)
        //    {
        //        myMiningController.coinsPerSec -= myMiningController.decreaseSpeed * 2;
        //    }
        //    else if (myMiningController.coinsPerSec > 50 && myMiningController.coinsPerSec <= 500)
        //    {
        //        myMiningController.coinsPerSec -= myMiningController.decreaseSpeed * 4;
        //    }
        //    else if (myminingcontroller.coinspersec > 500 && myminingcontroller.coinspersec <= 10000)
        //    {
        //        myminingcontroller.coinspersec -= myminingcontroller.decreasespeed * 16;
        //    }
        //    else if (myminingcontroller.coinspersec > 10000 && myminingcontroller.coinspersec <= 500000)
        //    {
        //        myminingcontroller.coinspersec -= myminingcontroller.decreasespeed * 48;
        //    }
        //}
    }

    public void IncreaseMinMaxMiningSpeed (float addition)
    {
        myMiningController.minCoinsPerSec += addition;
        myMiningController.maximumCoinsPerSec += addition;
    }

    public void AddTimeToDustTimer (int amount)
    {
        myMiningController.dustTimer += amount;
    }


    // Update is called once per frame
    void Update () {

        myMiningController.currentBalance += myMiningController.coinsPerSec * Time.deltaTime;
        myMiningController.coinsPerSec = Mathf.Clamp(myMiningController.coinsPerSec, myMiningController.minCoinsPerSec, myMiningController.maximumCoinsPerSec);

    }
}
