using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour {


    public Text noteTitle;
    public Image noteImage;
    public Text noteBody;
    public Button button1;
    public Button button2;
   
    public MenuController menuController;
    public MiningController miningController;

    public NotificationThumbnail currentlyOpenNoteThumbnail;

    public float miscNoteInterval;

    public GameObject notificationPanel;

    public List<Notification> miscNotifications;

    public Notification firstTimeRackNotification;

    public Transform thumbnailHolder;
    public List<GameObject> thumbnailSpaces;

    public Notification currentNoteToshow;

    public bool noteIsShowing;


    // Use this for initialization
    void Start()
    {
        HideAllNotificaitons();

        PopulateTheThumbnailSpaces();
    }


    public void PopulateTheThumbnailSpaces()
    {
        for (int i = 0; i < thumbnailHolder.childCount; i++)
        {
            thumbnailSpaces.Add(thumbnailHolder.GetChild(i).gameObject);
            thumbnailHolder.GetChild(i).gameObject.SetActive(false);
        }

        return;
    }

    public void RemoveReadNotificationFromPanel()
    {

    }

    // Pushes a regular notification to the thumbnail panel for the player to consider opening
    public void PushAMiscNotification(Notification notification)
    {

       foreach(GameObject thumbnailSpace in thumbnailSpaces)
        {
            if (!thumbnailSpace.gameObject.activeSelf)
            {
                thumbnailSpace.SetActive(true);
                
                thumbnailSpace.GetComponent<NotificationThumbnail>().myNotification = notification;
                thumbnailSpace.GetComponent<NotificationThumbnail>().turnOffTimer = 15f;
                thumbnailSpace.GetComponent<Image>().sprite = notification.icon;
                return;
            }
        }

        Debug.Log("Too many thumbnails to show new ones");

    }

    public void DisplayRegularNotification (Notification notification, NotificationThumbnail currentlyOpenThumbnail)
    {

        currentlyOpenNoteThumbnail = currentlyOpenThumbnail;

        // Close all the other menus first
        HideAllNotificaitons();
        menuController.CloseAllOtherMenus();

        currentNoteToshow = notification;
        notificationPanel.SetActive(true);
        

        noteTitle.text = currentNoteToshow.title;
        noteBody.text = currentNoteToshow.body;

        noteIsShowing = true;

        noteImage.gameObject.SetActive(false);
        

        if (notification.image)
        {
            noteImage.gameObject.SetActive(true);
            noteImage.GetComponent<Image>().sprite = notification.image;
        }

        if (notification.button1Label != "")
        {
            button1.gameObject.SetActive(true);
            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener(HideAllNotificaitons);
            button1.onClick.AddListener(() => HideAllNotificaitons());
            button1.GetComponentInChildren<Text>().text = notification.button1Label;
        }

        button2.onClick.RemoveAllListeners();
        button2.gameObject.SetActive(false);

        currentlyOpenNoteThumbnail.gameObject.SetActive(false);
        currentlyOpenNoteThumbnail = null;
 

    }


    // TODO: Write a function for a notification that will affect an attribute in the game

    

    public void HideAllNotificaitons()
    {
        notificationPanel.SetActive(false);
        noteIsShowing = false;
    }

	
	
	// Update is called once per frame
	void Update () {

        miscNoteInterval -= Time.deltaTime;

        if (miscNoteInterval <= 0 && !noteIsShowing)
        {
            PushAMiscNotification(miscNotifications[Random.Range(0, miscNotifications.Count)]);
            miscNoteInterval = 30f;
        }

		
	}
}
