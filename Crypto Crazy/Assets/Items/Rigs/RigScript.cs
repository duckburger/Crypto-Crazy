using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigScript : MonoBehaviour {


    public Rig me;

    //public int myID;
    public Sprite myIcon;
    public SpriteRenderer iconHolder;

	// Use this for initialization
	void Start () {
        myIcon = me.icon;
        iconHolder = GetComponent<SpriteRenderer>();
	}


	public void RefreshIcon()
    {
        myIcon = me.icon;
        iconHolder.sprite = myIcon;
    }
}
