using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public Animator coinAnimator;
    public float spinSpeed;
    public RectTransform touchRect;

    [SerializeField]
    private Vector2 touchStartPos;

    [SerializeField]
    private Vector2 touchEndPos;

    [SerializeField]
    private bool isSpinning;






    // Use this for initialization
    void Start() {
        spinSpeed = Mathf.Clamp(spinSpeed, 0, 2000);
    }

    // Update is called once per frame
    void Update() {

        HandlePreSpin();

        if (Input.touchCount > 0 && RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).deltaPosition) || RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
            HandleTheSpin();

    }

    void HandlePreSpin()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log("Registered a touch for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
                coinAnimator.Play("Spinning", -1, Input.GetTouch(0).position.x);


        }
        else if (Input.GetMouseButton(0))
        {
            Debug.Log("Registered a drag for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
                coinAnimator.Play("Spinning", -1, Input.mousePosition.x);
        }
    }

    void HandleTheSpin()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchStartPos = Input.GetTouch(0).position;

            Debug.Log("Registered a touch for the coin");

            

        }
        else if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;

            Debug.Log("Registered a click for the coin");
        }

       /* if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
           

            
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 5);


            coinAnimator.SetFloat("spinSpeed", spinSpeed);

            Debug.Log("Registered a touch movement for the coin");


        }*/

        if (Input.touchCount < 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchEndPos = Input.GetTouch(0).deltaPosition;

            float amountMoved = (touchEndPos - touchStartPos).magnitude;
            spinSpeed += amountMoved;

            spinSpeed -= (touchEndPos - touchStartPos).magnitude * Time.deltaTime;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 5);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);

            isSpinning = true;

        }
        else if (Input.GetMouseButtonUp(0))
        {

            touchEndPos = Input.mousePosition;


            spinSpeed += (touchEndPos - touchStartPos).magnitude * Time.deltaTime;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 5);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);

            isSpinning = true;

            Debug.Log("Registered mouse button up for the coin");
        }

        if (spinSpeed <= 0)
        {
            isSpinning = false;

            Debug.Log("Stopped spinning");
        }
    }

    private void LateUpdate()
    {
        if (isSpinning)
        {
            spinSpeed -= Time.deltaTime;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 5);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);

            Debug.Log("Slowing the spinning down");
        }
    }
}
