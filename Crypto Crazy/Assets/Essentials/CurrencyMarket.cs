using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyMarket : MonoBehaviour {


    public float exchangeRate;


    public void DropThePriceOfCurrency()
    {
        exchangeRate -= exchangeRate / 2;
    }


    public void SpikeThePriceOfCurrency()
    {
        exchangeRate += exchangeRate / 2;
    }

	
	
	// Update is called once per frame
	void Update () {
		
	}
}
