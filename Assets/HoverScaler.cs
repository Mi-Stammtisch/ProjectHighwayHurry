using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


[RequireComponent(typeof(EventTrigger))]
public class HoverScaler : MonoBehaviour
{
    [Range(1, 2)]
    public float ScaleAmount = 1.1f;
    public float ScaleUpSpeed = 0.1f;
    public float ScaleDownSpeed = 0.1f;
    private Vector3 originalScale;
    [SerializeField] UnityEvent SelectEvent;
    private GameObject objToScale;
  
    private void Start()
    {
        objToScale = transform.GetChild(0).gameObject;
        originalScale = objToScale.transform.localScale;
        

        EventTrigger trigger = this.gameObject.AddComponent<EventTrigger>();
        //set on pointer enter event to hover button
        //set on pointer exit event to hover button off

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { HoverButton(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { HoverButtonOff(); });
        trigger.triggers.Add(entry2);

        
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerClick;
        entry3.callback.AddListener((data) => { OnSelect(); });
        trigger.triggers.Add(entry3);



    }
    public void HoverButton()
    {
        LeanTween.scale(objToScale, new Vector3(ScaleAmount, ScaleAmount, ScaleAmount), ScaleUpSpeed);
       // Debug.Log("Scale Up");
    }

    public void HoverButtonOff()
    {
        LeanTween.cancel(objToScale);
        LeanTween.scale(objToScale, originalScale, ScaleDownSpeed);
    }

    public void OnSelect()
    {
        Debug.Log("Selected");
        LeanTween.cancel(objToScale);
        LeanTween.scale(objToScale, originalScale, ScaleDownSpeed);

        //invoke the select event
        SelectEvent.Invoke();



    }
}   
