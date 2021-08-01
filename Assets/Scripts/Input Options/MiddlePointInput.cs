using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MiddlePointInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private List<GameObject> inputLine;
    
    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        inputLine = new List<GameObject>();
        inputLine.Clear();
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (hitObj.tag.Equals("Line"))
        {
            inputLine.Clear();
            inputLine.Add(hitObj);
            EventBroker.CallMiddlePointData(inputLine);
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
                EventBroker.CallMiddlePointData(inputLine);
                foreach (var p in inputLine)
                {
                    p.GetComponent<PointData>().PointSelectRequestHandler();
                }
                inputLine.Clear();
            }
        }
    }
}
