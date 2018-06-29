using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    
    public Animator coinAnimator;
    public Animator windFXAnimator;
    public float spinSpeed;
    public float effectOnMiningSpeed;
    public RectTransform touchRect;
    [SerializeField] Vector2 touchStartPos;
    [SerializeField] Vector2 touchEndPos;
    [SerializeField] bool isCoinSpinning;
    [SerializeField] bool hadEffectOnMineSpeed;
    public bool isHoldingTopSpinSpeed;
    [SerializeField] MapController currentLevel;
    [SerializeField] bool isHeldDown;
    public MiningControllerTemplate myMiningController;

    [Header("Available Upgrades")]
    [SerializeField] bool holdToSpin;
    [SerializeField] float timeTopSpinSpeedHeld;
    [SerializeField] float miningSpeedDecrease;


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
        else
        {
            HandlePreSpin();
        }

        // Handles the launch of the rotation
        if (Input.touchCount > 0 && RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).deltaPosition) 
            || RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
        {
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
        if (isCoinSpinning && !isHoldingTopSpinSpeed && spinSpeed < myMiningController.maximumCoinsPerSec && myMiningController.coinsPerSec < myMiningController.maximumCoinsPerSec - 0.2f)
        {
            spinSpeed -= Time.deltaTime;
            spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
            PassSpinSpeedOver();
        }
        else if (isCoinSpinning && timeTopSpinSpeedHeld > 0 && !isHoldingTopSpinSpeed && myMiningController.coinsPerSec > myMiningController.maximumCoinsPerSec - 0.2f)
        {
            StartCoroutine(WaitAtTheTop());
        }

        if (isHoldingTopSpinSpeed)
        {
            spinSpeed = 10;
        }
    }

    IEnumerator WaitAtTheTop()
    {
        isHoldingTopSpinSpeed = true;
        Debug.Log("Starting the timer for the top spin hold");
        yield return new WaitForSeconds(timeTopSpinSpeedHeld);
        isHoldingTopSpinSpeed = false;
        spinSpeed -= 0.3f;
        myMiningController.coinsPerSec -= 0.3f;
        PassSpinSpeedOver();
    }

    public void ToggleCoinsPerSecTextFX()
    {

    }

    void PassSpinSpeedOver()
    {
        coinAnimator.SetFloat("spinSpeed", spinSpeed);
        windFXAnimator.SetFloat("spinSpeed", spinSpeed);
    }


    void HandlePreSpin()
    {
        // Handle coin pre-spin animation through touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Debug.Log("Registered a touch for the coin");

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
            {
                coinAnimator.Play("Spinning", -1, Input.GetTouch(0).position.x / 10);
                windFXAnimator.SetFloat("spinSpeed", 0);
            }

        }
        // Handle coin pre-spin animation through mouse clicks
        else if (Input.GetMouseButton(0))
        {

            if (RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
            {
                coinAnimator.Play("Spinning", -1, Input.mousePosition.x / 10);
                windFXAnimator.SetFloat("spinSpeed", 0);
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

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
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
        else if (Input.GetMouseButton(0))
        {
            touchStartPos = Vector2.zero;
            isHeldDown = true;
            CalculateMiningSpeedIncrease();
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

        if (!RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.GetTouch(0).position))
        {
            isHeldDown = false;
        }
        else if (!RectTransformUtility.RectangleContainsScreenPoint(touchRect, Input.mousePosition))
        {
            isHeldDown = false;
        }

    }


    private void CalculateMiningSpeedIncrease()
    {
        float amountMoved = (touchEndPos - touchStartPos).magnitude;

        if (holdToSpin && isHeldDown && !isHoldingTopSpinSpeed)
        {
            spinSpeed += myMiningController.miningSpeedIncreaseWhenHeld * Time.deltaTime;
        }
        else if (!isHeldDown)
        {
            spinSpeed += amountMoved * Time.deltaTime;
        }

        spinSpeed = Mathf.Clamp(spinSpeed, 0, 10);
        PassSpinSpeedOver();

        effectOnMiningSpeed = spinSpeed;

        //An increase calculator

        if (myMiningController.coinsPerSec < myMiningController.maximumCoinsPerSec - 2)
        {
            if (myMiningController.coinsPerSec > 0 && myMiningController.coinsPerSec <= 150)
            {
                myMiningController.coinsPerSec += effectOnMiningSpeed;
            }
            else if (myMiningController.coinsPerSec > 150 && myMiningController.coinsPerSec <= 500)
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


        if (!hadEffectOnMineSpeed)
        {
            hadEffectOnMineSpeed = true;
        }
    }

  
}
