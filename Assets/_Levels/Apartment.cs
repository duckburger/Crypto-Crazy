using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crypto Crazy/Apartment")]
public class Apartment : ScriptableObject {

    public string myTitle;
    public Sprite myIcon;
    public int myPrice;
    [TextArea(3,10)]
    public string myDescText;
    public int rackSlots;
    

    public GameObject myPrefab;
}
