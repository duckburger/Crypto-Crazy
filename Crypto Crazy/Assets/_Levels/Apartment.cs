using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Apartment : ScriptableObject {

    public string myTitle;
    public Sprite myIcon;
    [TextArea(3,10)]
    public string myDescText;
    public int rackSlots;
  

    public GameObject myPrefab;
}
