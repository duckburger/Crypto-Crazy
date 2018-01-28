using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu] 
public class UpgradeTemplate : ScriptableObject {

    public Sprite icon;

    public string title;
   
    [TextArea(3, 10)]
    public string descr;

    [TextArea(3, 10)]
    public string miscText;
    [TextArea(3, 10)]
    public string defMiscText;

    public float currentUpgradeLvl;
    public float defCurUpgLvl;

    public float priceOfNextUpgradeLvl;
    public float defPrOfNxtUpgLvl;

    public float currentUpgradeEffect;
    public float defCurUpgEff;
    

    public float maxUpgradeLvl;
    
    public List<int> effectsForEachUpgradeLvl;


    private void OnEnable()
    {
        currentUpgradeLvl = defCurUpgLvl;
        priceOfNextUpgradeLvl = defPrOfNxtUpgLvl;
        currentUpgradeEffect = defCurUpgEff;
    }


}
