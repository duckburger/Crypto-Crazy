using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// This attached to an object that is going to react to an event passed from another object

public class GameEventListener : MonoBehaviour {


    public GameEvent Event;
    public UnityEvent Response;

    public void OnEnable()
    {
        Event.RegisterAListener(this);
    }

    public void OnDisable()
    {
        Event.UnregisterAListener(this);
    }


    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
