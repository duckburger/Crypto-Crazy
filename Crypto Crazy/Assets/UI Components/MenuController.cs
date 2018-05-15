using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject upgradesMenu;
    public GameObject realEstateMenu;
    public GameObject miscMenu;
    public GameObject rigsMenu;
    public GameObject Menu5;
    public GameObject Menu6;
    public NotificationSystem notificationSystem;
    public List<GameObject> menus = new List<GameObject>();

    void Start()
    {
        menus.Add(upgradesMenu);
        menus.Add(realEstateMenu);
        menus.Add(miscMenu);
        menus.Add(rigsMenu);
        menus.Add(Menu5);
        menus.Add(Menu6);
        CloseAllOtherMenus();    
    }

    // This will close all menus if nothing is passed in
    public void CloseAllOtherMenus(GameObject currentMenu = null)
    {
        if (currentMenu != null)
        {
            foreach (GameObject menu in menus)
            {
                if (menu != currentMenu)
                {
                    menu.SetActive(false);
                }
            }
        } else
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
        }

        GetComponent<UIController>().HideJustSideRackMenu();

        return;
       
    }

    public void CloseAllMenus()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);  
        }
    }

    public void UpgradesMenuToggle()
    {
        if (upgradesMenu.gameObject.activeSelf)
        {
            upgradesMenu.SetActive(false);

        } else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(upgradesMenu);
            upgradesMenu.SetActive(true);
        }
    }

    public void RealEstateMenuToggle()
    {
        if (realEstateMenu.gameObject.activeSelf)
        {
            realEstateMenu.SetActive(false);
           
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(realEstateMenu);
            realEstateMenu.SetActive(true);
           
        }
    }

    public void MiscMenuToggle()
    {
        if (miscMenu.gameObject.activeSelf)
        {
            miscMenu.SetActive(false);
          
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(miscMenu);
            miscMenu.SetActive(true);
           
        }
    }

    public void RigsMenuToggle()
    {
        if (rigsMenu.gameObject.activeSelf)
        {
            rigsMenu.SetActive(false);
           
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            rigsMenu.SetActive(true);
            
        }
    }

    public void Menu5Control()
    {
        if (Menu5.gameObject.activeSelf)
        {
            Menu5.SetActive(false);
           
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            Menu5.SetActive(true);
            
        }
    }

    public void Menu6Control()
    {
        if (Menu6.gameObject.activeSelf)
        {
            Menu6.SetActive(false);
           
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            Menu6.SetActive(true);
          
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
