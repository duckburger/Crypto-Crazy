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
    public int currentUpgradeLvl;
    public int maxUpgradeLvl;

    public List<int> effectsForEachUpgradeLvl;


}
