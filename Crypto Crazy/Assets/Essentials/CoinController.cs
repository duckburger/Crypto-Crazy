using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public Animator coinAnimator;
    public float spinSpeed;
    public float effectOnMiningSpeed;
    public RectTransform touchRect;

    [SerializeField]
    private Vector2 touchStartPos;

    [SerializeField]
    private Vector2 touchEndPos;

    [SerializeField]
    private bool isSpinning;

    [SerializeField]
    private bool hadEffectOnMineSpeed;

    public MiningControllerTemplate myMiningController;


    // Use this for initialization
    void Start() {

        spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
    }

    // Update is called once per frame
    void Update() {

        // Handles the scrubbing rotation during the touch/drag phase
        HandlePreSpin();

        // Handles the launch of the rotation
        if (Input.touchCount > 0 && RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).deltaPosition) 
            || RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
            HandleTheSpin();

        effectOnMiningSpeed = spinSpeed;

        if (spinSpeed <= 0 && isSpinning)
        {
            isSpinning = false;

            // Handles the slowdown of the coins per sec meter as the coin stops spinning
            if (hadEffectOnMineSpeed)
            {
                if ((myMiningController.coinsPerSec -= myMiningController.coinsPerSec / 2f) > myMiningController.minCoinsPerSec)
                {
                    myMiningController.coinsPerSec -= myMiningController.coinsPerSec / 2f;
                }
                myMiningController.decreaseSpeed *= 3;
                hadEffectOnMineSpeed = false;
            }
            //Debug.Log("Stopped spinning");
        }

    }

    void HandlePreSpin()
    {
        // Handle coin pre-spin animation through touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Debug.Log("Registered a touch for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
                coinAnimator.Play("Spinning", -1, Input.GetTouch(0).position.x);


        }
        // Handle coin pre-spin animation through mouse clicks
        else if (Input.GetMouseButton(0))
        {
            //Debug.Log("Registered a drag for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
                coinAnimator.Play("Spinning", -1, Input.mousePosition.x);
        }
    }

    void HandleTheSpin()
    {

        // TODO: make the character animate as the coin spins

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchStartPos = Input.GetTouch(0).position;

            //Debug.Log("Registered a touch for the coin");

            

        }
        else if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;

            //Debug.Log("Registered a click for the coin");
        }

   

        if (Input.touchCount < 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchEndPos = Input.GetTouch(0).deltaPosition;
            CalculateMiningSpeedIncrease();

        }
        else if (Input.GetMouseButtonUp(0))
        {

            touchEndPos = Input.mousePosition;
            CalculateMiningSpeedIncrease();


        }

    }

    private void CalculateMiningSpeedIncrease()
    {
        float amountMoved = (touchEndPos - touchStartPos).magnitude;
        spinSpeed += amountMoved * Time.deltaTime;

        spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
        coinAnimator.SetFloat("spinSpeed", spinSpeed);

        effectOnMiningSpeed = spinSpeed;

        isSpinning = true;

        //An increaser
        if (myMiningController.coinsPerSec > 0 && myMiningController.coinsPerSec <= 10)
        {
            myMiningController.coinsPerSec += effectOnMiningSpeed;
        }
        else if (myMiningController.coinsPerSec > 10 && myMiningController.coinsPerSec <= 50)
        {
            myMiningController.coinsPerSec += effectOnMiningSpeed * 2;
        }
        else if (myMiningController.coinsPerSec > 50 && myMiningController.coinsPerSec <= 500)
        {
            myMiningController.coinsPerSec += effectOnMiningSpeed * 6;
        }
        else if (myMiningController.coinsPerSec > 500 && myMiningController.coinsPerSec <= 10000)
        {
            myMiningController.coinsPerSec += effectOnMiningSpeed * 18;
        }
        else if (myMiningController.coinsPerSec > 10000 && myMiningController.coinsPerSec <= 500000)
        {
            myMiningController.coinsPerSec += effectOnMiningSpeed * 54;
        }


            if (!hadEffectOnMineSpeed)
        {

            hadEffectOnMineSpeed = true;

        }
    }

    // Handles the gradual slow down of the rotation
    private void LateUpdate()
    {
        

        if (isSpinning)
        {
            spinSpeed -= Time.deltaTime;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);

           // Debug.Log("Slowing the spinning down");
        }
    }
}
