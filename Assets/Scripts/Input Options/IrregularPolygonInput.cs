using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class IrregularPolygonInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private List<GameObject> pointList;

    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
        pointList = new List<GameObject>();
        pointList.Clear();
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (pointList.Count < 3) return;
        EventBroker.CallIrregularPolygonData(pointList);
        foreach (var pt in pointList)
        {
            pt.GetComponent<PointData>().PointSelectRequestHandler();
        }
        pointList.Clear();
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (hitObj.tag.Equals("Point"))
        {
            var pt = hitObj.GetComponent<PointData>();
            pt.PointSelectRequestHandler();
            if (pt.isSelected)
            {
                pointList.Add(hitObj);
            }
            else pointList.Remove(hitObj);
        }
    }
}
