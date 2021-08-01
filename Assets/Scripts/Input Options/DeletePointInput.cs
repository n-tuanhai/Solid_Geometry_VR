using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DeletePointInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private GameObject point;

    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;

        point = null;
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
        controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (point == null) return;
        EventBroker.CallDeletePointData(point);
        point.GetComponent<PointData>().PointSelectRequestHandler();
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;

        if (hitObj.tag.Equals("Point"))
        {
            var pt = hitObj.GetComponent<PointData>();
            if (pt.isSelectable)
            {
                pt.PointSelectRequestHandler();
                if (pt.isSelected)
                {
                    point = pt.gameObject;
                }
                else point = null;
            }
        }
    }
}