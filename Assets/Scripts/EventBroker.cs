using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker
{
    public static event Action<List<GameObject>> setSelectedPointList;
    public static event Action<List<GameObject>> clearSelectedPointList;
    public static event Action<float> setAngleData;
    public static event Action<float> setPointHeightData;
    public static event Action<Vector2> setPointSurfaceData;
    public static event Action<GameObject> deletePointData;
    public static event Action<float> setLengthData;
    public static event Action<List<GameObject>> showAngleData;

    public static event Action<Vector3, bool> pointInSpaceData;
    public static event Action<Vector3, bool> pointOnSurfaceData;
    public static event Action<List<GameObject>> middlePointData;
    public static event Action<List<GameObject>, float> sectionPointData;

    public static event Action<List<GameObject>> lineData;
    public static event Action<List<GameObject>> perpendicularLineData;

    public static event Action<List<GameObject>> irregularPolygonData;
    public static event Action<List<GameObject>, int> regularPolygonData;

    public static event Action<List<GameObject>> tetrahedronData;
    public static event Action<List<GameObject>> pyramidData;
    public static event Action<List<GameObject>> prismData;
    public static event Action<GameObject> cubeData;

    #region Select

    public static void CallSetSelectedPointList(List<GameObject> ptList)
    {
        if (setSelectedPointList != null)
            setSelectedPointList(ptList);
    }

    public static void CallClearSelectedPointList(List<GameObject> ptList)
    {
        if (clearSelectedPointList != null)
            clearSelectedPointList(ptList);
    }

    public static void CallSetAngleData(float angle)
    {
        if (setAngleData != null)
            setAngleData(angle);
    }

    public static void CallSetPointHeightData(float angle)
    {
        if (setPointHeightData != null)
            setPointHeightData(angle);
    }

    public static void CallSetPointSurfaceData(Vector2 diff)
    {
        if (setPointSurfaceData != null)
            setPointSurfaceData(diff);
    }

    public static void CallSetLengthData(float diff)
    {
        if (setLengthData != null)
            setLengthData(diff);
    }

    public static void CallShowAngleData(List<GameObject> ptList)
    {
        if (showAngleData != null)
            showAngleData(ptList);
    }

    public static void CallDeletePointData(GameObject point)
    {
        if (deletePointData != null)
            deletePointData(point);
    }

    #endregion

    #region Point

    public static void CallPointInSpaceData(Vector3 pos, bool sel)
    {
        if (pointInSpaceData != null)
            pointInSpaceData(pos, sel);
    }

    public static void CallPointOnSurfaceData(Vector3 pos, bool sel)
    {
        if (pointOnSurfaceData != null)
            pointOnSurfaceData(pos, sel);
    }

    public static void CallSectionPointData(List<GameObject> ptList, float ratio)
    {
        if (sectionPointData != null)
            sectionPointData(ptList, ratio);
    }

    public static void CallMiddlePointData(List<GameObject> inputLine)
    {
        if (middlePointData != null)
            middlePointData(inputLine);
    }

    #endregion

    #region Line

    public static void CallLineData(List<GameObject> ptList)
    {
        if (lineData != null)
            lineData(ptList);
    }

    public static void CallPerpendicularLineData(List<GameObject> objectList)
    {
        if (perpendicularLineData != null)
            perpendicularLineData(objectList);
    }

    #endregion

    #region 2D Polygon

    public static void CallIrregularPolygonData(List<GameObject> ptList)
    {
        if (irregularPolygonData != null)
            irregularPolygonData(ptList);
    }

    public static void CallRegularPolygonData(List<GameObject> ptList, int vertices)
    {
        if (regularPolygonData != null)
            regularPolygonData(ptList, vertices);
    }

    #endregion

    #region 3D Shape

    public static void CallTetrahedronData(List<GameObject> objectList)
    {
        if (tetrahedronData != null)
            tetrahedronData(objectList);
    }

    public static void CallPyramidData(List<GameObject> objectList)
    {
        if (pyramidData != null)
            pyramidData(objectList);
    }

    public static void CallPrismData(List<GameObject> objectList)
    {
        if (prismData != null)
            prismData(objectList);
    }

    public static void CallCubeData(GameObject square)
    {
        if (cubeData != null)
            cubeData(square);
    }

    #endregion
}