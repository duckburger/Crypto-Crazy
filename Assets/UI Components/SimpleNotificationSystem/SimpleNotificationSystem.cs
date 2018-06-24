using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleNotificationSystem : MonoBehaviour {

    public static SimpleNotificationSystem Instance;
    [SerializeField] TextMeshProUGUI notificationText;
    [SerializeField] float timeBetweenNotifications;
    [SerializeField] float animTime;
    [SerializeField] LeanTweenType easeOnAnimIn;
    [SerializeField] LeanTweenType easeOnAnimOut;

    Queue<string> notifications = new Queue<string>();
    bool isShowing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void CloseCurrentNotification()
    {
        if (notifications.Count > 0)
        {
            Hide();
        }
    }

    public void QueueNotification(string notification)
    {
        if (!notifications.Contains(notification))
        {
            notifications.Enqueue(notification);
            DisplayNextNotification();
        }
        else
        {
            Debug.Log("Notification already in the queue, won't add it again.");
        }
    }

    void DisplayNextNotification()
    {
        if (!isShowing)
        {
            notificationText.text = notifications.Peek();
            Display();
        }
        else
        {
            return;
        }
    }
	
    IEnumerator WaitThenDisplayNext()
    {
        yield return new WaitForSeconds(timeBetweenNotifications);
        Hide();
        yield return new WaitForSeconds(animTime);
        if (notifications.Count > 0)
        {
            notificationText.text = notifications.Peek();
            Display();
        }
       
    }


    void Display()
    {
        isShowing = true;
        notificationText.gameObject.SetActive(true);
        LeanTween.alphaCanvas(notificationText.GetComponent<CanvasGroup>(), 1, animTime).setEase(easeOnAnimIn);
        StartCoroutine(WaitThenDisplayNext());
    }

    void Hide()
    {
        if (notifications.Count > 0)
        {
            notifications.Dequeue();
            LeanTween.alphaCanvas(notificationText.GetComponent<CanvasGroup>(), 0, animTime).setEase(easeOnAnimIn)
                .setOnComplete(() =>
                {
                    notificationText.gameObject.SetActive(false);
                    isShowing = false;
                });
        }
    }

}
