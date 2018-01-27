using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MiningControllerTemplate : ScriptableObject {

    public string currencyName;
    public int maximumCoinsPerSec;
    public float currencyMined;
    public float coinsPerSec;
    public float decreaseSpeed;

    public int defMaxMinPerSec;
    public float defCurMin;
    public float defCoinsPerSec;
    public float defDecSpeed;

    private void OnEnable()
    {
        maximumCoinsPerSec = defMaxMinPerSec;
        currencyMined = defCurMin;
        coinsPerSec = defCoinsPerSec;
        decreaseSpeed = defDecSpeed;
    }


}
