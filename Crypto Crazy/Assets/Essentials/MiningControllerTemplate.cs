using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crypto Crazy/Mining Controller")]
public class MiningControllerTemplate : ScriptableObject {

    [Header("Runtime values")]
    public string currencyName;
    public float maximumCoinsPerSec;
    public float currentBalance;
    public float coinsPerSec;
    public float miningSpeedIncreaseWhenHeld;
    public float dustTimer;

    //This number determined how low the mining speed can go
    // so it should be modified by upgrades
    public float minCoinsPerSec;

    public float decreaseSpeed;


    [Header("Default values")]
    public int defMaxCoinsPerSecond;
    public float defaultCurrentBalance;
    public float defCoinsPerSec;
    public float defMiningSpeedIncreaseWhenHeld;
    public float defDecSpeed;
    public float defMinCoinsPerSec;
    public float defaultDustTimer;

    public float currentCurrencyExchangeRate;

    [Header("Colors")]
    public Color bgColor;
    public Color positiveColor;
    public Color negativeColor;
    public Color yellowColor;
    public Color orangeColor;
    

    private void OnEnable()
    {
        maximumCoinsPerSec = defMaxCoinsPerSecond;
        currentBalance = defaultCurrentBalance;
        coinsPerSec = defCoinsPerSec;
        decreaseSpeed = defDecSpeed;
        minCoinsPerSec = defMinCoinsPerSec;
        miningSpeedIncreaseWhenHeld = defMiningSpeedIncreaseWhenHeld;
        dustTimer = defaultDustTimer;
    }

 


}
