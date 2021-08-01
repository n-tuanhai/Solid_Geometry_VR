using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SectionPointInput : MonoBehaviour
{
     public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    public GameObject keyboardUI;
    public InputField inputField;
    
    private float ratio;
    private List<GameObject> inputLine;
    
    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
        inputLine = new List<GameObject>();
        inputLine.Clear();
        ratio = 0;
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        float.TryParse(inputField.text, out ratio);
        inputField.text = "";
        keyboardUI.SetActive(false);

        if (inputLine.Count == 2)
        {
            foreach (var p in inputLine)
            {
                p.GetComponent<PointData>().PointSelectRequestHandler();
            }
        }
        EventBroker.CallSectionPointData(inputLine, ratio);
        inputLine.Clear();
    }
    
    
    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (hitObj.tag.Equals("Line"))
        {
            inputLine.Clear();
            inputLine.Add(hitObj);
            keyboardUI.SetActive(true);
            inputLine.Clear();
        }

        else if (hitObj.tag.Equals("Point"))
        {
            var pt = hitObj.GetComponent<PointData>();
            pt.PointSelectRequestHandler();
            if (pt.isSelected)
            {
                inputLine.Add(hitObj);
            }
            else inputLine.Remove(hitObj);
            
            if (inputLine.Count == 2)
            {
                keyboardUI.SetActive(true);
            }
        }
    }
}
