using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Rig : Building {

    public string myStringName;

    public string title;
    public Sprite icon;

    public int id;

    public Sprite myAppearance;

    [TextArea(3, 10)]
    public string myDescription;
    public int myEffectOnMining;

    public float priceOfNextUpgradeLvl;


}
