using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Notification : ScriptableObject {

    public Sprite icon;
    public Sprite image;

    
    public string title;

    [TextArea(3, 10)]
    public string body;

    public string button1Label;
    public string button2Label;


    public bool affectsGameplay;

    public Attribute[] attributesIAffect;

    public float button1EffectOnAttribute;
    public float button2EffectOnAttribute;

    

}



