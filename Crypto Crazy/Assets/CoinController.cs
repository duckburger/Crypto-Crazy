using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public Animator coinAnimator;
    public float spinSpeed;

    [SerializeField]
    private Vector2 touchStartPos;

    [SerializeField]
    private Vector2 touchEndPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchStartPos = Input.GetTouch(0).position;
            
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchEndPos = Input.GetTouch(0).deltaPosition;

            float amountMoved = (touchEndPos - touchStartPos).magnitude;
            spinSpeed += amountMoved;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 2000);

            coinAnimator.SetFloat("spinSpeed", spinSpeed);


        }

        if (Input.touchCount < 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            spinSpeed -= (touchEndPos - touchStartPos).magnitude * 5;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 2000);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);
        }
		
	}

    private void LateUpdate()
    {
        spinSpeed -= 5;
        coinAnimator.SetFloat("spinSpeed", spinSpeed);
    }
}
