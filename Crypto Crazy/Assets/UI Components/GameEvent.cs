using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This attached to an object that is going to produce an event
[CreateAssetMenu(menuName = "Crypto Crazy/Game Event")]
public class GameEvent : ScriptableObject {

	public List<GameEventListener> eventListeners = new List<GameEventListener>();


    public void Raise()
    {
        // We are going backwards in the list because if we want to remove an event from the list
        // we don't want to mess up the iteration
        for(int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }        
    }

    public void RegisterAListener(GameEventListener listener)
    {
        eventListeners.Add(listener);
    }

    public void UnregisterAListener(GameEventListener listener)
    {
        eventListeners.Remove(listener);
    }
}
