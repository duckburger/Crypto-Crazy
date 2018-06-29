using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Canvas visualUpgradesMenu;
    public Canvas realEstateMenu;
    public Canvas nonVisualUpgradesMenu;
    public Canvas rigsMenu;
    public Canvas Menu5;
    public Canvas Menu6;
    public NotificationSystem notificationSystem;
    public List<Canvas> menus = new List<Canvas>();
    public bool isMenuOpen;

    [SerializeField] ScrollRect upgradeMenuScrollRect;
    [SerializeField] ScrollRect realEstateMenuScrollRect;
    [SerializeField] ScrollRect rigsMenuScrollRect;

    void Start()
    {
        menus.Add(visualUpgradesMenu);
        menus.Add(realEstateMenu);
        menus.Add(nonVisualUpgradesMenu);
        menus.Add(rigsMenu);
        menus.Add(Menu5);
        menus.Add(Menu6);
        CloseAllMenus();
    }

    // This will close all menus if nothing is passed in
    public void CloseAllOtherMenus(Canvas currentMenu = null)
    {
        if (currentMenu != null)
        {
            foreach (Canvas menu in menus)
            {
                if (menu != currentMenu)
                {
                    menu.enabled = false;
                }
            }
        } else
        {
            foreach (Canvas menu in menus)
            {
                menu.enabled = false;
            }
        }
        isMenuOpen = true;

        return;
       
    }

    public void SetMenuStatusToClose()
    {
        isMenuOpen = false;
    }

    

    public void CloseAllMenus()
    {
        foreach (Canvas menu in menus)
        {
            menu.enabled = false;
        }
        isMenuOpen = false;
    }

    public void UpgradesMenuToggle()
    {
        if (visualUpgradesMenu.enabled)
        {
            upgradeMenuScrollRect.verticalNormalizedPosition = 1;
            visualUpgradesMenu.enabled = false;
            isMenuOpen = false;
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(visualUpgradesMenu);
            upgradeMenuScrollRect.verticalNormalizedPosition = 1;
            visualUpgradesMenu.enabled = true;
            isMenuOpen = true;
        }
    }

    public void RealEstateMenuToggle()
    {
        if (realEstateMenu.enabled)
        {
            realEstateMenuScrollRect.verticalNormalizedPosition = 1;
            realEstateMenu.enabled = false;
            isMenuOpen = false;
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(realEstateMenu);
            realEstateMenuScrollRect.verticalNormalizedPosition = 1;
            realEstateMenu.enabled = true;
            isMenuOpen = true;
        }
    }

    public void MiscMenuToggle()
    {
        if (nonVisualUpgradesMenu.enabled)
        {
            nonVisualUpgradesMenu.enabled = false;
            isMenuOpen = false;

        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(nonVisualUpgradesMenu);
            nonVisualUpgradesMenu.enabled = true;
            isMenuOpen = true;
        }
    }

    public void RigsMenuToggle()
    {
        if (rigsMenu.enabled)
        {
            rigsMenuScrollRect.verticalNormalizedPosition = 1;
            rigsMenu.enabled = true;
            isMenuOpen = false;

        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            rigsMenuScrollRect.verticalNormalizedPosition = 1;
            rigsMenu.enabled = true;
            isMenuOpen = true;
        }
    }

    public void Menu5Control()
    {
        if (Menu5.enabled)
        {
            Menu5.enabled = false;
            isMenuOpen = false;

        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            Menu5.enabled = true;
            isMenuOpen = true;
        }
    }

    public void Menu6Control()
    {
        if (Menu6.enabled)
        {
            Menu6.enabled = false;
            isMenuOpen = false;
        }
        else if (!notificationSystem.noteIsShowing)
        {
            CloseAllOtherMenus(rigsMenu);
            Menu6.enabled = true;
            isMenuOpen = true;
        }
    }
}
