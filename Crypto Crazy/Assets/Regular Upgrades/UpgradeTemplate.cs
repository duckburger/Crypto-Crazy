using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Crypto Crazy/Upgrade")] 
public class UpgradeTemplate : ScriptableObject {

    public Sprite icon;

    public string title;
   

    [TextArea(3, 10)]
    public string descr;

    [TextArea(3, 10)]
    public string miscText;
    [TextArea(3, 10)]
    public string defMiscText;

    public int currentUpgradeLvl;
    public int defCurUpgLvl;

    public float priceOfNextUpgradeLvl;
    public float defPrOfNxtUpgLvl;

    public float defCurUpgEff;

    public float maxUpgradeLvl;

    public List<Attribute> attributesIAffect;
    public List<Building> buildingsISpawn;
    
    [Tooltip("If this list has 0 elements, than the upgrade does not affect the mining speed.")]
    public List<int> primaryListOfEffects;


    [Tooltip("This is a secondary list in case the upgrade affects more than 1 component.")]
    public List<int> secondaryListOfEffects;

    private void OnEnable()
    {
        currentUpgradeLvl = defCurUpgLvl;
        priceOfNextUpgradeLvl = defPrOfNxtUpgLvl;

        if (primaryListOfEffects.Count > 0)
        {
            maxUpgradeLvl = primaryListOfEffects.Count - 1;
        }
    }


  


}
