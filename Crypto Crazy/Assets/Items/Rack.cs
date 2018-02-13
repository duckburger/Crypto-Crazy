using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rack : MonoBehaviour {

    public List<Rig> myRigs;

    public int myNumber;

    public int CountMyRigs()
    {

        int curAmtOfRigs = 0;
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                curAmtOfRigs++;
        }

        return curAmtOfRigs;
    }


    public void SpawnARig(Rig rigToSpawn)
    {

    }

	// Use this for initialization
	void Start () {

        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
