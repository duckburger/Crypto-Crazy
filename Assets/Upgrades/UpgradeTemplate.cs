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

    public float accumulatedPrimUpgrEffect;
    public float accumulatedSecUpgrEffect;

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
        accumulatedPrimUpgrEffect = 0;
        accumulatedSecUpgrEffect = 0;

        if (primaryListOfEffects.Count > 0)
        {
            maxUpgradeLvl = primaryListOfEffects.Count - 1;
        }
    }

    public string GenerateNextUpgradeTextAnnouncement()
    {
        if (currentUpgradeLvl == maxUpgradeLvl)
        {
           return "MAX UPGRADE REACHED!";
        }

        if (primaryListOfEffects.Count > 0 && secondaryListOfEffects.Count > 0)
        {
            return "Next Lvl: " + "+ " + primaryListOfEffects[(int)currentUpgradeLvl + 1] + " mining speed" + ", +" + secondaryListOfEffects[(int)currentUpgradeLvl + 1] + "s to dust timer";
        } 
        else if (primaryListOfEffects.Count > 0 && secondaryListOfEffects.Count <= 0)
        {
            return "Next Lvl: " + "+ " + primaryListOfEffects[(int)currentUpgradeLvl + 1] + " mining speed";
        }
        else if (primaryListOfEffects.Count <= 0 && secondaryListOfEffects.Count > 0)
        {
            return "Next Lvl: " + "+" + secondaryListOfEffects[(int)currentUpgradeLvl + 1] + "s to dust timer";
        }
        return null;
    }
    
    public string GenerateCurrentEffectTextAnnouncement()
    {
        if (primaryListOfEffects.Count > 0 && secondaryListOfEffects.Count > 0)
        {
            accumulatedPrimUpgrEffect += primaryListOfEffects[(int)currentUpgradeLvl];
            accumulatedSecUpgrEffect += secondaryListOfEffects[(int)currentUpgradeLvl];
            return "+" + accumulatedSecUpgrEffect + "," + "\n" + "+" + accumulatedSecUpgrEffect + "s";
        }
        else if (primaryListOfEffects.Count > 0 && secondaryListOfEffects.Count <= 0)
        {
            accumulatedPrimUpgrEffect += primaryListOfEffects[(int)currentUpgradeLvl];
            return "+" + accumulatedPrimUpgrEffect;
        }
        else if (primaryListOfEffects.Count <= 0 && secondaryListOfEffects.Count > 0)
        {
            accumulatedSecUpgrEffect += secondaryListOfEffects[(int)currentUpgradeLvl];
            return "+" + accumulatedSecUpgrEffect + "s";
        }
        return "";
    }
      
}
