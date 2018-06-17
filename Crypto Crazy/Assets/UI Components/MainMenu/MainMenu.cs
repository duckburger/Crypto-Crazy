using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] RectTransform optionsMenu;
    [SerializeField] RectTransform newGameMenu;
    [SerializeField] float menuAnimTime = 0.4f;
    [SerializeField] LeanTweenType animInEase;
    [SerializeField] LeanTweenType animOutEase;


    [SerializeField] RectTransform activeMenu;


    public void HideCurrentlyActiveMenu()
    {
        if (activeMenu != null)
        {
            LeanTween.moveX(activeMenu, Screen.width * 2, menuAnimTime).setEase(animOutEase).setOnComplete( () =>
            {
                activeMenu.gameObject.SetActive(false);
                activeMenu = null;
            });
        }
    }
    

    public void AddMenuAndOpen(RectTransform newMenu)
    {
        if (activeMenu != null)
        {
            HideCurrentlyActiveMenu();
            StartCoroutine(ShowNewMenuWithDelay(newMenu));
            return;
        }
        ShowNewMenuWithoutDelay(newMenu);
    }

    void ShowNewMenuWithoutDelay(RectTransform menu)
    {
        activeMenu = menu;
        activeMenu.gameObject.SetActive(true);
        LeanTween.moveX(activeMenu, 0, menuAnimTime).setEase(animInEase);
        
    }

    IEnumerator ShowNewMenuWithDelay(RectTransform menu)
    {
        yield return new WaitForSeconds(menuAnimTime);
        menu.gameObject.SetActive(true);
        LeanTween.moveX(menu, 0, menuAnimTime).setEase(animInEase);
        activeMenu = menu;
    }



}
