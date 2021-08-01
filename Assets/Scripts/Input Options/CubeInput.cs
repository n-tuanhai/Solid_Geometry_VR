using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CubeInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    private GameObject square;

    private void OnEnable()
    {
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
        square = null;
    }

    private void OnDisable()
    {
        controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        GameObject hitObj = pointer.pointerRenderer.GetDestinationHit().collider.gameObject;
        
        if (hitObj.tag.Equals("Polygon"))
        {
            var pol = hitObj.GetComponent<PolygonData>();

            if (pol.polygonType == PolygonData.PolygonType.Regular && pol.pointList.Count == 4)
            {
                pol.PolygonSelectRequestHandler();
                square = pol.isSelected ? hitObj : null;
            }
        }
        
        if (square != null)
        {
            EventBroker.CallCubeData(square);
            square.GetComponent<PolygonData>().PolygonSelectRequestHandler();
            square = null;
        }
    }
}