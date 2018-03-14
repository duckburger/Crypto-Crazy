using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Reattach this to its own GO!
public class ItemDatabase : MonoBehaviour {
   

    public List<Rig> rigTypes = new List<Rig>();

    public List<Sprite> chairs = new List<Sprite>();
    public List<Sprite> desks = new List<Sprite>();
    public List<Sprite> monitors = new List<Sprite>();
    public List<Sprite> hamsters = new List<Sprite>();
    public List<Sprite> coolSystems = new List<Sprite>();

    public Building basicRackItem;	
}
