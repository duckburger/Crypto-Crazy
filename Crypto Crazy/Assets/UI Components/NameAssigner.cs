using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAssigner : MonoBehaviour {

    [SerializeField] MiningControllerTemplate miningController;

    public void ChangeCurrencyName(string newName)
    {
        if (newName.Length > 0)
        {
            miningController.currencyName = newName;
        }
    }

    public string GetCurrencyName()
    {
        if (miningController.currencyName.Length > 0)
        {
            return miningController.currencyName;
        }
        //TODO: add a random coin name generator
        // (list of words to add to "coins";
        return "Sweetcoins";
    }

	
}
