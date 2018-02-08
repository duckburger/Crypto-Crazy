using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NotificationThumbnail : MonoBehaviour, IPointerDownHandler {

    public Notification myNotification;
    public NotificationSystem notificationSystem;

    public float turnOffTimer = 15f;

    public bool isBlinking;



	// Use this for initialization
	void Start () {
        notificationSystem = FindObjectOfType<NotificationSystem>();

    }
	
	// Update is called once per frame
	void Update () {

        turnOffTimer -= Time.deltaTime;

        if (turnOffTimer <= 5 && !isBlinking)
        {
            StartCoroutine(BlinkThumbnailIcon());
            isBlinking = true;
        }

        if (turnOffTimer <= 0)
        {
            this.gameObject.SetActive(false);
            isBlinking = false;
            myNotification = null;
        }


	}

    public void ResetMe()
    {
        isBlinking = false;
        turnOffTimer = 15f;
        myNotification = null;
        this.gameObject.SetActive(false);

    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Registered a click on the " + this.name + " thumbnail");

        notificationSystem.DisplayRegularNotification(myNotification, this);
        ResetMe();
    }


    IEnumerator BlinkThumbnailIcon()
    {

        Debug.Log("Blinking the thumbnail icon");
        Color origColor = this.GetComponent<Image>().color;
        int i = 0;
        while (i < 3)
        {
            this.GetComponent<Image>().color = new Color (255f, 255f, 255f, 0);
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<Image>().color = origColor;
            yield return new WaitForSeconds(1f);
            i++;
        }
        this.GetComponent<Image>().color = origColor;
        yield break;
    }

}
