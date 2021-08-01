using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SetLengthInput : MonoBehaviour
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
        if (pointList.Count != 2) return;

        if (!controllerEvents.gripPressed)
        {
            lastPosition = new Vector3(rightController.transform.position.x, rightController.transform.position.y,
                rightController.transform.position.z);
        }
        else if (controllerEvents.gripPressed)
        {
            var curPos = rightController.transform.position;
            if (curPos != lastPosition)
            {
                float diff = curPos.x - lastPosition.x;
                EventBroker.CallSetLengthData(diff);
                lastPosition = curPos;
            }
        }
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (pointList.Count != 2) return;
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
            pt.PointSelectRequestHandler();
            if (pt.isSelected)
            {
                pointList.Add(hitObj);
            }
            else pointList.Remove(hitObj);
        }


        if (pointList.Count == 2)
        {
            EventBroker.CallSetSelectedPointList(pointList);
        }
    }
}