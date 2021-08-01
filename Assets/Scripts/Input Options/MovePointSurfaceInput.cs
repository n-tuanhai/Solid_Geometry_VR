using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MovePointSurfaceInput : MonoBehaviour
{
    public VRTK_Pointer pointer;
    public VRTK_ControllerEvents controllerEvents;
    public GameObject rightController;
    private List<GameObject> pointList;
    private Vector3 lastPosition;

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

    private void Update()
    {
        if (pointList.Count != 1) return;

        if (!controllerEvents.gripPressed)
        {
            lastPosition = new Vector3(rightController.transform.position.x, 0, rightController.transform.position.z);
        }
        else if (controllerEvents.gripPressed)
        {
            var curPos = rightController.transform.position;
            if (curPos != lastPosition)
            {
                Vector2 diff = new Vector2(curPos.x - lastPosition.x, curPos.z - lastPosition.z);
                EventBroker.CallSetPointSurfaceData(diff);
                lastPosition = curPos;
            }
        }
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (pointList.Count != 1) return;
        EventBroker.CallClearSelectedPointList(pointList);
        foreach (var pt in pointList)
        {
            pt.GetComponent<PointData>().PointSelectRequestHandler();
        }

        EventBroker.CallClearSelectedPointList(pointList);
        pointList.Clear();
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
                    pointList.Add(hitObj);
                }
                else pointList.Remove(hitObj);
            }
        }


        if (pointList.Count == 1)
        {
            EventBroker.CallSetSelectedPointList(pointList);
        }
    }
}