using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject Menu1;
    public GameObject Menu2;
    public GameObject Menu3;
    public GameObject Menu4;
    public GameObject Menu5;
    public GameObject Menu6;


    public List<GameObject> menus = new List<GameObject>();

    private bool isAMenuOpen; // TODO: make use of this somehow


    void Start()
    {
        menus.Add(Menu1);
        menus.Add(Menu2);
        menus.Add(Menu3);
        menus.Add(Menu4);
        menus.Add(Menu5);
        menus.Add(Menu6);

        CloseAllOtherMenus();
        
    }

    void CloseAllOtherMenus(GameObject currentMenu = null)
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

        return;
       
    }

    public void Menu1Control()
    {
        if (Menu1.gameObject.activeSelf)
        {
            Menu1.SetActive(false);
            isAMenuOpen = false;

        } else
        {
            CloseAllOtherMenus(Menu1);
            Menu1.SetActive(true);
            isAMenuOpen = true;
        }
    }

    public void Menu2Control()
    {
        if (Menu2.gameObject.activeSelf)
        {
            Menu2.SetActive(false);
            isAMenuOpen = false;
        }
        else
        {
            CloseAllOtherMenus(Menu2);
            Menu2.SetActive(true);
            isAMenuOpen = true;
        }
    }

    public void Menu3Control()
    {
        if (Menu3.gameObject.activeSelf)
        {
            Menu3.SetActive(false);
            isAMenuOpen = false;
        }
        else
        {
            CloseAllOtherMenus(Menu3);
            Menu3.SetActive(true);
            isAMenuOpen = true;
        }
    }

    public void Menu4Control()
    {
        if (Menu4.gameObject.activeSelf)
        {
            Menu4.SetActive(false);
            isAMenuOpen = false;
        }
        else
        {
            CloseAllOtherMenus(Menu4);
            Menu4.SetActive(true);
            isAMenuOpen = true;
        }
    }

    public void Menu5Control()
    {
        if (Menu5.gameObject.activeSelf)
        {
            Menu5.SetActive(false);
            isAMenuOpen = false;
        }
        else
        {
            CloseAllOtherMenus(Menu4);
            Menu5.SetActive(true);
            isAMenuOpen = true;
        }
    }

    public void Menu6Control()
    {
        if (Menu6.gameObject.activeSelf)
        {
            Menu6.SetActive(false);
            isAMenuOpen = false;
        }
        else
        {
            CloseAllOtherMenus(Menu4);
            Menu6.SetActive(true);
            isAMenuOpen = true;
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
