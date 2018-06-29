using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningController : MonoBehaviour {


    public MiningControllerTemplate myMiningController;
    [SerializeField] CoinController coinController;

    private void LateUpdate()
    {

        if (myMiningController.coinsPerSec > myMiningController.minCoinsPerSec)
        {
            if (!coinController.isHoldingTopSpinSpeed)
            {
                myMiningController.coinsPerSec -= Time.deltaTime * myMiningController.decreaseSpeed;
            }
        }


        if (myMiningController.coinsPerSec == 1)
        {
            myMiningController.decreaseSpeed = myMiningController.defDecSpeed;
        }
        //    }
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

    #region Upgrade functions

    public void IncreaseMinMaxMiningSpeed (float addition)
    {
        myMiningController.minCoinsPerSec += addition / 2.2f;
        myMiningController.maximumCoinsPerSec += addition;
    }

    public void AddTimeToDustTimer (int amount)
    {
        myMiningController.dustTimer += amount;
    }

    public void AddPercentageToDustTimer (float percentage)
    {
        myMiningController.dustTimer += (myMiningController.dustTimer / 100) * percentage;
    }

    public void DoubleARandomRigRack()
    {
        MapController mapController = FindObjectOfType<MapController>();
        int choice = Random.Range(0, 1);
        if (choice == 0 && mapController.racksBuilt > 0)
        {
            // Apply to a random rack
            int rackChoice = Random.Range(0, mapController.racksBuilt - 1);
            RigScript[] rigsInRack = mapController.rackSlots[rackChoice].GetComponentsInChildren<RigScript>();
            int effectOnMining = 0;
            foreach (RigScript script in rigsInRack)
            {
                effectOnMining += script.me.myEffectOnMining;
            }
            Debug.Log("Applying " + effectOnMining + " points to miningPerSec stat through a random doubler");
            IncreaseMinMaxMiningSpeed(effectOnMining);

        } 
        else
        {
            int rigChoice = Random.Range(0, mapController.rigsBuilt - 1);
            RigScript rig = mapController.rigSlots[rigChoice].GetComponentInChildren<RigScript>();
            int effectOnMining = rig.me.myEffectOnMining;
            Debug.Log("Applying " + effectOnMining + " points to miningPerSec stat through a random doubler");
            IncreaseMinMaxMiningSpeed(effectOnMining);
        }

    }

    #endregion


    // Update is called once per frame
    void Update () {

        myMiningController.currentBalance += myMiningController.coinsPerSec * Time.deltaTime;
        myMiningController.coinsPerSec = Mathf.Clamp(myMiningController.coinsPerSec, myMiningController.minCoinsPerSec, myMiningController.maximumCoinsPerSec);

    }
}
