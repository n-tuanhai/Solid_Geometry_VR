using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PointInSpaceInput : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public GameObject pointIndicator;
    private GameObject pointIndi;
    
    private void OnEnable()
    {
        pointIndi = Instantiate(pointIndicator, transform.position, Quaternion.identity); 
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
    }

    private void Update()
    {
        pointIndi.GetComponent<Transform>().position = transform.position;
    }

    private void OnDisable()
    {
        Destroy(pointIndi);
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }
    
    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        EventBroker.CallPointInSpaceData(transform.position, true);
    }
}
