using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool zoomedIn;

    public void SwitchZoomLevels()
    {
        if (zoomedIn)
        {
            Camera.main.orthographicSize = 3.22f;
            zoomedIn = false;
        }
        else
        {
            Camera.main.orthographicSize = 1.08f;
            zoomedIn = true;
        }
    }

	// Use this for initialization
	void Start () {
        zoomedIn = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
