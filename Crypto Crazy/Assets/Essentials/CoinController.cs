using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public Animator coinAnimator;
    public float spinSpeed;
    public float effectOnMiningSpeed;
    public RectTransform touchRect;

    [SerializeField] Vector2 touchStartPos;
    [SerializeField] Vector2 touchEndPos;
    [SerializeField] bool isCoinSpinning;
    [SerializeField] bool hadEffectOnMineSpeed;
    [SerializeField] MapController currentLevel;

    public MiningControllerTemplate myMiningController;


    [SerializeField] bool holdToSpin;
    [SerializeField] bool isHeldDown;

    // Use this for initialization
    void Start() {

        spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
    }

    public void RefreshMapRef()
    {
        currentLevel = FindObjectOfType<MapController>();
    }
    
    public void TurnOnHoldToSpin()
    {
        holdToSpin = true;
    }

    public void TurnOffHoldToSpin()
    {
        holdToSpin = false;
    }

    // Update is called once per frame
    void Update() {

        // Handles the scrubbing rotation during the touch/drag phase
        if (holdToSpin)
        {
            SetUpHoldToSpin();
        }
            
        HandlePreSpin();

            

        // Handles the launch of the rotation
        if (Input.touchCount > 0 && RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).deltaPosition) 
            || RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
        {

            Debug.Log("Handling spin");
            if (holdToSpin && isHeldDown)
            {
                CheckForHoldToSpin();
            }
            HandleSpin();

            
        }
            

        effectOnMiningSpeed = spinSpeed;

        // This turns blinker on/off - power consumption mechanic?
        //if (spinSpeed >= 8)
        //{
        //    currentLevel.lightsBlinker.BlinkOn();
        //    SimpleNotificationSystem.Instance.QueueNotification("Drawing too much power! Aaaaaaaa...");
        //}
        //else
        //{
        //    currentLevel.lightsBlinker.BlinkOff();
        //    SimpleNotificationSystem.Instance.CloseCurrentNotification();
        //}
        

        if (spinSpeed <= 0 && isCoinSpinning)
        {
            isCoinSpinning = false;

            // Handles the slowdown of the coins per sec meter as the coin stops spinning
            if (hadEffectOnMineSpeed)
            {
                if ((myMiningController.coinsPerSec / 2) > myMiningController.minCoinsPerSec)
                {
                    myMiningController.coinsPerSec -= myMiningController.coinsPerSec / 1.5f;
                }
                //myMiningController.decreaseSpeed *= 3;
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
            {
                coinAnimator.Play("Spinning", -1, Input.GetTouch(0).position.x);

            }

        }
        // Handle coin pre-spin animation through mouse clicks
        else if (Input.GetMouseButton(0))
        {
            Debug.Log("Registered a drag for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
            {
                coinAnimator.Play("Spinning", -1, Input.mousePosition.x / 10);

            }

        }
    }

    void HandleSpin()
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

    void SetUpHoldToSpin()
    {

        // Handle coin pre-spin animation through touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            //Debug.Log("Registered a touch for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
            {
                isHeldDown = true;
            }
            
        }
        // Handle coin pre-spin animation through mouse clicks
        else if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Registered a drag for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
            {
                isHeldDown = true;
            }
            
        }

    }

    void CheckForHoldToSpin()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            touchStartPos = Vector2.zero;
            isHeldDown = true;
            CalculateMiningSpeedIncrease();
            //Debug.Log("Registered a hold for the coin");

        }
        else if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Vector2.zero;
            isHeldDown = true;
            CalculateMiningSpeedIncrease();
            //Debug.Log("Registered a hold for the coin");
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isHeldDown = false;
            CalculateMiningSpeedIncrease();
            return;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHeldDown = false;
            CalculateMiningSpeedIncrease();
            return;
        }

        //if (!RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
        //{
        //    isHeldDown = false;
        //}
        //else if (!RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
        //{
        //    isHeldDown = false;
        //}

        


    }

   

    private void CalculateMiningSpeedIncrease()
    {
        float amountMoved = (touchEndPos - touchStartPos).magnitude;
        if (!holdToSpin)
        {
            spinSpeed += amountMoved * 2 * Time.deltaTime;
        }
        else if (holdToSpin && isHeldDown)
        {
            spinSpeed += myMiningController.miningSpeedIncreaseWhenHeld * Time.deltaTime;
        }

        Debug.Log("Spinspeed is " + spinSpeed);
        spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
        coinAnimator.SetFloat("spinSpeed", spinSpeed);

        


        //An increase calculator

        if (myMiningController.coinsPerSec < myMiningController.maximumCoinsPerSec - 2)
        {
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
            isCoinSpinning = true;
        }
        else
        {
            myMiningController.coinsPerSec = myMiningController.maximumCoinsPerSec;
            // Pause the mining amount at the top for a time
        }

        effectOnMiningSpeed = spinSpeed;

        if (!hadEffectOnMineSpeed)
        {
            hadEffectOnMineSpeed = true;
        }
    }


    // Handles the gradual slow down of the rotation
    private void LateUpdate()
    {
        if (spinSpeed > 0)
        {
            isCoinSpinning = true;
        }
        else
        {
            isCoinSpinning = false;
        }
        SlowCoinDown();
    }

    private void SlowCoinDown()
    {
        if (isCoinSpinning)
        {
            spinSpeed -= Time.deltaTime * 1.5f;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
            coinAnimator.SetFloat("spinSpeed", spinSpeed);
        }
    }
}
