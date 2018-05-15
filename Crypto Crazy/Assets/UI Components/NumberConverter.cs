using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class NumberConverter : MonoBehaviour {

    

    public string[] shortHands = new string[10]
    {
        "Mlns",
        "Blns",
        "Trls",
        "Qdrls",
        "Qntls",
        "Sxtls",
        "Sptls",
        "Octls",
        "Nnls",
        "Dcls"
    };

    public string ConvertNumber(float value)
    {
        int nZeros = (int)(Mathf.Log10(value));
        int prefixIndex = (int)(((nZeros) / 3));
        prefixIndex -= 2; // We delete the Thousand from the function to start with millions


        if (nZeros < 6) // If under the Million, no need to convert
            return value.ToString("0");
        else if (prefixIndex > 19) // Overflow..
            prefixIndex = 19;

        string prefix = shortHands[prefixIndex];
        double number = value / (Mathf.Pow(10, ((prefixIndex + 2) * 3)));
        string returnvalue = number.ToString("0.00");
        returnvalue += " ";
        returnvalue += prefix;
        return returnvalue;
    }

}
