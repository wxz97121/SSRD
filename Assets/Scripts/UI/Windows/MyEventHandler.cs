using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;



public class MyEventHandler : MonoBehaviour, ISelectHandler
{
    public UnityEvent onSelect=new UnityEvent();



    public void OnSelect(BaseEventData eventData)
    {

        onSelect.Invoke();
    
    }
}
