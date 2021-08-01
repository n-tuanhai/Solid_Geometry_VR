using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PerpendicularLineInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private List<GameObject> objectList;

    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        objectList = new List<GameObject>();
        objectList.Clear();
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }


    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (objectList.Count == 0)
        {
            if (hitObj.tag.Equals("Point"))
            {
                var pt = hitObj.GetComponent<PointData>();
                pt.PointSelectRequestHandler();
                if (pt.isSelected)
                {
                    objectList.Add(hitObj);
                }
                else objectList.Remove(hitObj);
            }
        }
        else
        {
            if (hitObj.tag.Equals("Polygon"))
            {
                var pol = hitObj.GetComponent<PolygonData>();
                pol.PolygonSelectRequestHandler();
                if (pol.isSelected)
                {
                    objectList.Add(hitObj);
                }
                else objectList.Remove(hitObj);
            }

            if (hitObj.tag.Equals("Line"))
            {
                objectList.Add(hitObj);
            }
        }

        if (objectList.Count == 2)
        {
            EventBroker.CallPerpendicularLineData(objectList);
            objectList[0].GetComponent<PointData>().PointSelectRequestHandler();
            if (objectList[1].GetComponent<PolygonData>() != null)
            {
                objectList[1].GetComponent<PolygonData>().PolygonSelectRequestHandler();
            }

            objectList.Clear();
        }
    }
}