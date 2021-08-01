using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PointOnSurfaceInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;

    
    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        if (hitObj.tag.Equals("Base Floor") || hitObj.tag.Equals("Polygon") || hitObj.tag.Equals("Line"))
        {
            var hitPos = pointer.pointerRenderer.GetDestinationHit().point;
            var pos = hitObj.tag.Equals("Base Floor") ? new Vector3(hitPos.x, hitPos.y + 0.1f, hitPos.z) : hitPos;
            EventBroker.CallPointOnSurfaceData(pos, true);
        }
    }
}
