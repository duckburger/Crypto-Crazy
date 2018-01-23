using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningController : MonoBehaviour {


    public Text currencyText;
    public string curName;

    public float currencyMined;


    private float currencyModifier = 1;

   
    public float CurrencyModifier
    {
        get
        {
            return currencyModifier;
        }

        set
        {
            currencyModifier = value;
        }
    }

    public int currencyMinedPerSecond;


    private void Start()
    {

    }

    // Update is called once per frame
    void Update () {

        currencyMined += CurrencyModifier * Time.deltaTime;

        currencyText.text = Mathf.RoundToInt(currencyMined).ToString("n2");




	}
}
