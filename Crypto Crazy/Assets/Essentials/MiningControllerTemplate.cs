using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MiningControllerTemplate : ScriptableObject {

    public string currencyName;
    public float maximumCoinsPerSec;
    public float currencyMined;
    public float coinsPerSec;

    //This number determined how low the mining speed can go
    // so it should be modified by upgrades
    public float minCoinsPerSec;

    public float decreaseSpeed;

    public int defMaxMinPerSec;
    public float defCurMin;
    public float defCoinsPerSec;
    public float defDecSpeed;

    public float currentCurrencyExchangeRate;
    

    private void OnEnable()
    {
        maximumCoinsPerSec = defMaxMinPerSec;
        currencyMined = defCurMin;
        coinsPerSec = defCoinsPerSec;
        decreaseSpeed = defDecSpeed;
    }


}
