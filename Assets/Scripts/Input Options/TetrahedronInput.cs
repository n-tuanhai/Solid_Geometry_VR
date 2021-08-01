using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TetrahedronInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private List<GameObject> objectList;

    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
        objectList = new List<GameObject>();
        objectList.Clear();
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (objectList.Count != 2) return;
        EventBroker.CallTetrahedronData(objectList);
        objectList[0].GetComponent<PolygonData>().PolygonSelectRequestHandler();
        objectList[1].GetComponent<PointData>().PointSelectRequestHandler();
        objectList.Clear();
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (objectList.Count == 0)
        {
            if (hitObj.tag.Equals("Polygon"))
            {
                var pol = hitObj.GetComponent<PolygonData>();
                if (pol.pointList.Count == 3)
                {
                    pol.PolygonSelectRequestHandler();
                    if (pol.isSelected)
                    {
                        objectList.Add(hitObj);
                    }
                    else objectList.Remove(hitObj);
                }
            }
        }
        else
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
    }
}