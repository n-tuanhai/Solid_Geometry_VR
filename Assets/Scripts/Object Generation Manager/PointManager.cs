using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Line;
using UnityEngine;
using VRTK;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public List<PointData> pointList;
    public GameObject pointPrefab;
    public GameObject angleInfoPrefab;

    private List<GameObject> selectedPoints;

    private List<GameObject> currentAngle;
    private GameObject angleInfo;
    private GameObject angleToolBox;
    private GameObject lineLengthInfo;
    private float angleSideLength;
    private Vector3 normVec;

    private char nameIncrease;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        nameIncrease = (char) ('A' - 1);
        angleToolBox = null;
    }

    private string SetPointName()
    {
        nameIncrease = nameIncrease > 'Z' ? 'A' : (char) (nameIncrease + 1);
        return nameIncrease.ToString();
    }

    public GameObject SpawnPointAtPosition(Vector3 pos, bool isSelectable)
    {
        var pt = Instantiate(pointPrefab, pos, Quaternion.identity, transform);
        var ptData = pt.GetComponent<PointData>();
        ptData.isSelectable = isSelectable;
        ptData.SetName(SetPointName());
        pointList.Add(ptData);
        return pt;
    }

    public void SpawnMiddlePoint(List<GameObject> inputLine)
    {
        var pos = inputLine.Count == 2
            ? (inputLine[0].transform.position + inputLine[1].transform.position) / 2
            : inputLine[0].GetComponent<LineData>().GetMiddlePoint();
        var pt = SpawnPointAtPosition(pos, false);
        pointList.Add(pt.GetComponent<PointData>());
    }

    public void SpawnSectionPoint(List<GameObject> inputLine, float ratio)
    {
        var pos = inputLine.Count == 2
            ? inputLine[0].transform.position +
              (inputLine[1].transform.position - inputLine[0].transform.position) * ratio
            : inputLine[0].GetComponent<LineData>().GetSectionPoint(ratio);
        var pt = SpawnPointAtPosition(pos, false);
        pointList.Add(pt.GetComponent<PointData>());
    }

    private void SetRelatedObjectUpdate(GameObject point, bool update)
    {
        point.GetComponent<PointData>().isUpdating = true;
        var ln = LineManager.Instance.FindLine(point);
        if (ln.Count != 0)
        {
            foreach (var l in ln)
            {
                l.isUpdating = update;
            }
        }

        var pol = PolygonManager.Instance.FindPolygon(point);
        if (pol.Count != 0)
        {
            foreach (var p in pol)
            {
                p.isUpdating = update;
                foreach (var l in p.lineList)
                {
                    l.isUpdating = update;
                }
            }
        }
    }

    public void SetSelectedPoint(List<GameObject> ptList)
    {
        selectedPoints = new List<GameObject>();
        foreach (var p in ptList)
        {
            selectedPoints.Add(p);
        }

        if (ptList.Count == 1)
        {
            SetRelatedObjectUpdate(ptList[0], true);
        }

        if (ptList.Count == 2)
        {
            SetRelatedObjectUpdate(ptList[0], true);
            SetRelatedObjectUpdate(ptList[1], true);
        }

        if (ptList.Count == 3)
        {
            angleSideLength = Vector3.Magnitude(ptList[2].GetComponent<Transform>().position -
                                                ptList[1].GetComponent<Transform>().position);
            normVec =
                Vector3.Cross(
                    selectedPoints[0].GetComponent<Transform>().position -
                    selectedPoints[1].GetComponent<Transform>().position,
                    selectedPoints[2].GetComponent<Transform>().position -
                    selectedPoints[1].GetComponent<Transform>().position);
            SetRelatedObjectUpdate(ptList[2], true);
            SpawnAngleInfo();
        }
    }

    public void SpawnAngleInfo()
    {
        angleInfo = Instantiate(angleInfoPrefab, new Vector3(-2, 2, 2), Quaternion.identity,
            selectedPoints[1].transform);
        angleInfo.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        angleInfo.GetComponent<VRTK_ObjectTooltip>().UpdateText(Vector3.Angle(
            selectedPoints[0].GetComponent<Transform>().position -
            selectedPoints[1].GetComponent<Transform>().position, selectedPoints[2].GetComponent<Transform>().position -
                                                                  selectedPoints[1].GetComponent<Transform>()
                                                                      .position).ToString(CultureInfo
            .InvariantCulture));
        angleInfo.transform.localPosition = new Vector3(-2, 2, 2);
    }

    // public void SpawnLineLengthInfo()
    // {
    //     lineLengthInfo = Instantiate(angleInfoPrefab, selectedPoints[0] + ) 
    // }

    private void Update()
    {
        if (angleToolBox == null) return;
        angleToolBox.GetComponent<VRTK_ObjectTooltip>().UpdateText(Vector3
            .Angle(currentAngle[0].transform.position - currentAngle[1].transform.position,
                currentAngle[2].transform.position - currentAngle[1].transform.position)
            .ToString(CultureInfo.InvariantCulture));
    }

    public void ShowAngle(List<GameObject> ptList)
    {
        currentAngle = new List<GameObject>();
        foreach (var p in ptList)
        {
            currentAngle.Add(p);
        }

        angleToolBox = Instantiate(angleInfoPrefab, new Vector3(-2, 2, 2), Quaternion.identity,
            currentAngle[1].transform);
        angleToolBox.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        angleToolBox.transform.localPosition = new Vector3(-2, 2, 2);
    }

    public void ClearSelectedPoint(List<GameObject> ptList)
    {
        if (ptList.Count == 3)
        {
            angleSideLength = 0;
            ptList[2].GetComponent<PointData>().isUpdating = false;
            SetRelatedObjectUpdate(ptList[2], false);
            Destroy(angleInfo);
        }

        if (ptList.Count == 2)
        {
            ptList[0].GetComponent<PointData>().isUpdating = false;
            ptList[1].GetComponent<PointData>().isUpdating = false;
            SetRelatedObjectUpdate(ptList[0], false);
            SetRelatedObjectUpdate(ptList[1], false);
        }
        
        if (ptList.Count == 1)
        {
            ptList[0].GetComponent<PointData>().isUpdating = false;
            SetRelatedObjectUpdate(ptList[0], false);
        }

        selectedPoints.Clear();
    }

    public void SetAngle(float angle)
    {
        Quaternion turnAngle = Quaternion.AngleAxis(angle, normVec.normalized);
        selectedPoints[2].transform.position =
            turnAngle * (selectedPoints[2].transform.position - selectedPoints[1].transform.position) +
            selectedPoints[1].transform.position;

        float diff = Vector3.Magnitude(selectedPoints[2].transform.position - selectedPoints[1].transform.position) -
                     angleSideLength;

        if (diff > 0)
        {
            var dir = selectedPoints[2].GetComponent<Transform>().position -
                      selectedPoints[1].GetComponent<Transform>().position;
            selectedPoints[2].transform.position =
                dir.normalized * angleSideLength + selectedPoints[1].transform.position;
        }

        angleInfo.GetComponent<VRTK_ObjectTooltip>().UpdateText(Vector3.Angle(
            selectedPoints[0].GetComponent<Transform>().position -
            selectedPoints[1].GetComponent<Transform>().position, selectedPoints[2].GetComponent<Transform>().position -
                                                                  selectedPoints[1].GetComponent<Transform>()
                                                                      .position).ToString(CultureInfo
            .InvariantCulture));
    }

    public void SetLength(float diff)
    {
        var norm = (selectedPoints[1].transform.position - selectedPoints[0].transform.position).normalized;
        selectedPoints[1].transform.position = selectedPoints[1].transform.position + norm.normalized * diff;
    }

    public void SetPointHeight(float height)
    {
        var pos = selectedPoints[0].transform.position;
        selectedPoints[0].transform.position = new Vector3(pos.x, pos.y + height, pos.z);
    }

    public void SetPointSurface(Vector2 diff)
    {
        var pos = selectedPoints[0].transform.position;
        selectedPoints[0].transform.position = new Vector3(pos.x + diff.x, pos.y, pos.z + diff.y);
    }

    public void DeletePoint(GameObject point)
    {
        var ln = LineManager.Instance.FindLine(point);
        if (ln.Count != 0)
        {
            foreach (var l in ln)
            {
                LineManager.Instance.RemoveLine(l);
                Destroy(l.gameObject);
            }
        }

        var pol = PolygonManager.Instance.FindPolygon(point);
        if (pol.Count != 0)
        {
            foreach (var p in pol)
            {
                PolygonManager.Instance.RemovePolygon(p);
                Destroy(p.gameObject);
            }
        }

        var shape = ShapeManager.Instance.FindShape(point);
        if (shape.Count != 0)
        {
            foreach (var s in shape)
            {
                ShapeManager.Instance.RemoveShape(s);
                Destroy(s.gameObject);
            }
        }

        pointList.Remove(point.GetComponent<PointData>());
        Destroy(point.gameObject);
    }
}