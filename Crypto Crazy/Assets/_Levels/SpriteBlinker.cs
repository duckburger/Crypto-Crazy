using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlinker : MonoBehaviour {

    [SerializeField] SpriteRenderer spriteToBlink;
    [SerializeField] float fadeTime;

    float maxFadeTime;

    [SerializeField] bool isBlinking;

    private void Start()
    {
        maxFadeTime = fadeTime;
    }

    public void BlinkOn()
    {
        if (!isBlinking)
        {
            isBlinking = true;
        }
    }

    public void BlinkOff()
    {
        if (isBlinking)
        {
            isBlinking = false;
            LeanTween.alpha(this.gameObject, 0, 0.5f).setEase(LeanTweenType.easeOutQuart);
        }       
    }


    private void Update()
    {
        if (isBlinking)
        {
            // Blink
            Blink();
        }
    }

    void Blink()
    {
        float alpha = Mathf.SmoothStep(0, 0.5f, fadeTime);
        spriteToBlink.color = new Color(0, 0, 0, alpha);
        fadeTime = Random.Range(0, maxFadeTime);
    }

}
