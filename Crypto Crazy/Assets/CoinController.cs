using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchStartPos = Input.GetTouch(0).position;
            
        }

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchEndPos = Input.GetTouch(0).deltaPosition;


        }
		
	}
}
