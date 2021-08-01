using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Line;
using UnityEngine;

public class InputDataHandler : MonoBehaviour
{
    private void OnEnable()
    {
        EventBroker.setSelectedPointList += SetSelectedPoint;
        EventBroker.clearSelectedPointList += ClearSelectedPoint;
        EventBroker.setAngleData += SetAngle;
        EventBroker.setPointHeightData += SetPointHeight;
        EventBroker.setPointSurfaceData += SetPointSurface;
        EventBroker.deletePointData += DeletePoint;
        EventBroker.setLengthData += SetLength;
        EventBroker.showAngleData += ShowAngle;

        EventBroker.pointInSpaceData += SpawnPointInSpace;
        EventBroker.pointOnSurfaceData += SpawnPointOnSurface;
        EventBroker.middlePointData += SpawnMiddlePoint;
        EventBroker.sectionPointData += SpawnSectionPoint;

        EventBroker.lineData += SpawnLine;
        EventBroker.perpendicularLineData += SpawnPerpendicularLine;

        EventBroker.irregularPolygonData += SpawnIrregularPolygon;
        EventBroker.regularPolygonData += SpawnRegularPolygon;

        EventBroker.tetrahedronData += SpawnTetrahedron;
        EventBroker.pyramidData += SpawnPyramid;
        EventBroker.prismData += SpawnPrism;
        EventBroker.cubeData += SpawnCube;
    }

    private void OnDisable()
    {
        EventBroker.setSelectedPointList -= SetSelectedPoint;
        EventBroker.clearSelectedPointList -= ClearSelectedPoint;
        EventBroker.setAngleData -= SetAngle;
        EventBroker.setPointHeightData -= SetPointHeight;
        EventBroker.setPointSurfaceData -= SetPointSurface;
        EventBroker.deletePointData -= DeletePoint;
        EventBroker.setLengthData -= SetLength;
        EventBroker.pointInSpaceData -= SpawnPointInSpace;
        EventBroker.showAngleData -= ShowAngle;
        EventBroker.pointOnSurfaceData -= SpawnPointOnSurface;
        EventBroker.middlePointData -= SpawnMiddlePoint;
        EventBroker.sectionPointData -= SpawnSectionPoint;
        EventBroker.lineData -= SpawnLine;
        EventBroker.perpendicularLineData -= SpawnPerpendicularLine;
        EventBroker.irregularPolygonData -= SpawnIrregularPolygon;
        EventBroker.regularPolygonData -= SpawnRegularPolygon;
        EventBroker.tetrahedronData -= SpawnTetrahedron;
        EventBroker.pyramidData -= SpawnPyramid;
        EventBroker.prismData -= SpawnPrism;
        EventBroker.cubeData -= SpawnCube;
    }


    #region Select

    private void SetSelectedPoint(List<GameObject> ptList)
    {
        PointManager.Instance.SetSelectedPoint(ptList);
    }

    private void ClearSelectedPoint(List<GameObject> ptList)
    {
        PointManager.Instance.ClearSelectedPoint(ptList);
    }

    private void SetAngle(float angle)
    {
        PointManager.Instance.SetAngle(angle);
    }

    private void ShowAngle(List<GameObject> ptList)
    {
        PointManager.Instance.ShowAngle(ptList);
    }

    private void SetPointHeight(float height)
    {
        PointManager.Instance.SetPointHeight(height);
    }
    
    private void SetPointSurface(Vector2 diff)
    {
        PointManager.Instance.SetPointSurface(diff);
    }

    private void SetLength(float diff)
    {
        PointManager.Instance.SetLength(diff);
    }
    
    private void DeletePoint(GameObject point)
    {
        PointManager.Instance.DeletePoint(point);
    }

    #endregion
    
    #region Point

    private void SpawnPointInSpace(Vector3 pos, bool sel)
    {
        PointManager.Instance.SpawnPointAtPosition(pos, sel);        
    }

    private void SpawnPointOnSurface(Vector3 pos, bool sel)
    {
        PointManager.Instance.SpawnPointAtPosition(pos, sel);   
    }

    private void SpawnMiddlePoint(List<GameObject> inputLine)
    {
        PointManager.Instance.SpawnMiddlePoint(inputLine);
    }

    private void SpawnSectionPoint(List<GameObject> inputLine, float ratio)
    {
        PointManager.Instance.SpawnSectionPoint(inputLine, ratio);
    }

    #endregion

    #region Line

    private void SpawnLine(List<GameObject> ptList)
    {
        LineManager.Instance.SpawnLine(ptList);
    }

    private void SpawnPerpendicularLine(List<GameObject> objectList)
    {
        LineManager.Instance.SpawnPerpendicularLine(objectList);
    }

    #endregion

    #region 2D Polygon

    private void SpawnIrregularPolygon(List<GameObject> ptList)
    {
        var pointList = new List<GameObject>();
        var linePointList = new List<GameObject>();
        var lineList = new List<LineData>();

        foreach (var p in ptList)
        {
            var ll = LineManager.Instance.FindLine(p);
            if (ll.Count > 0)
            {
                foreach (var o in ll)
                {
                    Destroy(o);
                }
            }
            pointList.Add(p);
        }
        
        for (int i = 0; i < pointList.Count; i++)
        {
            if (i == pointList.Count - 1)
            {
                linePointList.Add(pointList[i]);
                linePointList.Add(pointList[0]);
            }
            else
            {
                linePointList.Add(pointList[i]);
                linePointList.Add(pointList[i+1]);
            }
        
            var l = LineManager.Instance.FindSpecificLine(linePointList[0], linePointList[1]);
            if (l == null)
            {
                var ln = LineManager.Instance.SpawnLine(linePointList);
                lineList.Add(ln);
            }
            else lineList.Add(l);
            linePointList.Clear();
        }
        PolygonManager.Instance.SpawnPolygon(pointList, lineList, PolygonData.PolygonType.Irregular);
    }

    private void SpawnRegularPolygon(List<GameObject> ptList, int vertices)
    {
        var tmpList = PolygonManager.Instance.GetRegularPolygonVerticesPosition(ptList, vertices);
        var pointList = new List<GameObject>();
        var lineList = new List<LineData>();
        var linePointList = new List<GameObject>();

        if (ptList.Count == 2)
        {
            pointList.Add(ptList[0]);
            pointList.Add(ptList[1]);
        }
        else if (ptList.Count == 1)
        {
            pointList.Add(ptList[0].GetComponent<LineData>().startPoint.gameObject);
            pointList.Add(ptList[0].GetComponent<LineData>().endPoint.gameObject);
        }
        
        for (int i = 0; i < tmpList.Count; i++)
        {
            if (i != 0 && i != 1)
            {
                var p = PointManager.Instance.SpawnPointAtPosition(tmpList[i], false);
                pointList.Add(p);
            }
        }

        if (ptList.Count == 1)
        {
            Destroy(ptList[0]);
            ptList.Clear();
        }
        
        for (int i = 0; i < pointList.Count; i++)
        {
            if (i == pointList.Count - 1)
            {
                linePointList.Add(pointList[i]);
                linePointList.Add(pointList[0]);
            }
            else
            {
                linePointList.Add(pointList[i]);
                linePointList.Add(pointList[i+1]);
            }
            var ln = LineManager.Instance.SpawnLine(linePointList);
            lineList.Add(ln);
            linePointList.Clear();
        }
        PolygonManager.Instance.SpawnPolygon(pointList, lineList, PolygonData.PolygonType.Regular);
    }

    #endregion

    #region 3D Shape

    private void SpawnTetrahedron(List<GameObject> objectList)
    {
        var inputPtList = new List<GameObject>();
        var inputLnList = new List<LineData>();
        var inputPolList = new List<PolygonData>();
        
        var pol = objectList[0].GetComponent<PolygonData>();
        var pt = objectList[1].GetComponent<PointData>();

        inputPtList.Add(pt.gameObject);
        inputPolList.Add(pol);
        foreach (var ln in pol.lineList)
        {
            inputLnList.Add(ln);
        }
        
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            var tmpPolInput = new List<GameObject>
            {
                pt.gameObject,
                pol.pointList[i],
                i == pol.pointList.Count - 1 ? pol.pointList[0] : pol.pointList[i + 1]
            };
            var tmpLnInput = new List<GameObject>
            {
                pt.gameObject,
                pol.pointList[i]
            };
            
            var pl = PolygonManager.Instance.SpawnPolygon(tmpPolInput, null, PolygonData.PolygonType.Irregular);
            inputPolList.Add(pl);
            tmpPolInput.Clear();

            var l = LineManager.Instance.SpawnLine(tmpLnInput);
            inputLnList.Add(l);
            tmpLnInput.Clear();
            
            inputPtList.Add(pol.pointList[i]);
        }
        ShapeManager.Instance.SpawnShape(inputPtList, inputLnList, inputPolList, ShapeData.ShapeType.Tetrahedron);
    }

    private void SpawnPyramid(List<GameObject> objectList)
    {
        var inputPtList = new List<GameObject>();
        var inputLnList = new List<LineData>();
        var inputPolList = new List<PolygonData>();
        
        var pol = objectList[0].GetComponent<PolygonData>();
        var pt = objectList[1].GetComponent<PointData>();

        inputPtList.Add(pt.gameObject);
        inputPolList.Add(pol);
        foreach (var ln in pol.lineList)
        {
            inputLnList.Add(ln);
        }
        
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            var tmpPolInput = new List<GameObject>
            {
                pt.gameObject,
                pol.pointList[i],
                i == pol.pointList.Count - 1 ? pol.pointList[0] : pol.pointList[i + 1]
            };
            var tmpLnInput = new List<GameObject>
            {
                pt.gameObject,
                pol.pointList[i]
            };
            
            var pl = PolygonManager.Instance.SpawnPolygon(tmpPolInput, null, PolygonData.PolygonType.Irregular);
            inputPolList.Add(pl);
            tmpPolInput.Clear();

            var l = LineManager.Instance.SpawnLine(tmpLnInput);
            inputLnList.Add(l);
            tmpLnInput.Clear();
            
            inputPtList.Add(pol.pointList[i]);
        }
        ShapeManager.Instance.SpawnShape(inputPtList, inputLnList, inputPolList, ShapeData.ShapeType.Pyramid);
    }

    private void SpawnPrism(List<GameObject> objectList)
    {
        var inputPtList = new List<GameObject>();
        var inputLnList = new List<LineData>();
        var inputPolList = new List<PolygonData>();
        var upperPolygonPtList = new List<GameObject>();

        var pol = objectList[0].GetComponent<PolygonData>();
        var pt = objectList[1].GetComponent<PointData>();

        inputPtList.Add(pt.gameObject);
        inputPolList.Add(pol);
        upperPolygonPtList.Add(pt.gameObject);
        
        foreach (var ln in pol.lineList)
        {
            inputLnList.Add(ln);
        }
        
        //base vec
        Vector3 baseVec = pt.position - pol.pointList[0].transform.position;
        float length = baseVec.magnitude;
        
        //spawn points
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            if (i != 0)
            {
                var pos = pol.pointList[i].transform.position + baseVec.normalized * length;
                var p = PointManager.Instance.SpawnPointAtPosition(pos, false);
                upperPolygonPtList.Add(p);    
                inputPtList.Add(p);
            }
        }
        
        //spawn lines & poly
        var poly = PolygonManager.Instance.SpawnPolygon(upperPolygonPtList, null, PolygonData.PolygonType.Irregular);
        inputPolList.Add(poly);

        for (int i = 0; i < upperPolygonPtList.Count; i++)
        {
            var tmpLineList = new List<GameObject>();
            if (i == upperPolygonPtList.Count - 1)
            {
                tmpLineList.Add(upperPolygonPtList[i]);
                tmpLineList.Add(upperPolygonPtList[0]);
            }
            else
            {
                tmpLineList.Add(upperPolygonPtList[i]);
                tmpLineList.Add(upperPolygonPtList[i+1]);
            }
            var ln = LineManager.Instance.SpawnLine(tmpLineList);
            inputLnList.Add(ln);
            tmpLineList.Clear();
        }
        
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            var tmpPolInput = new List<GameObject>
            {
                pol.pointList[i],
                i == pol.pointList.Count - 1 ? pol.pointList[0] : pol.pointList[i + 1],
                i == pol.pointList.Count - 1 ? upperPolygonPtList[0] : upperPolygonPtList[i+1], 
                upperPolygonPtList[i]
            };
            var tmpLnInput = new List<GameObject>
            {
                upperPolygonPtList[i],
                pol.pointList[i]
            };
            
            var pl = PolygonManager.Instance.SpawnPolygon(tmpPolInput, null, PolygonData.PolygonType.Irregular);
            inputPolList.Add(pl);
            tmpPolInput.Clear();

            var l = LineManager.Instance.SpawnLine(tmpLnInput);
            inputLnList.Add(l);
            tmpLnInput.Clear();
        }
        ShapeManager.Instance.SpawnShape(inputPtList, inputLnList, inputPolList, ShapeData.ShapeType.Prism);
    }

    private void SpawnCube(GameObject square)
    {
        var inputPtList = new List<GameObject>();
        var inputLnList = new List<LineData>();
        var inputPolList = new List<PolygonData>();
        var upperPolygonPtList = new List<GameObject>();
        
        var pol = square.GetComponent<PolygonData>();
        var length = Vector3.Magnitude(pol.pointList[1].transform.position - pol.pointList[0].transform.position);
        inputPolList.Add(pol);
        foreach (var ln in pol.lineList)
        {
            inputLnList.Add(ln);
        }
        
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            var pos = new Vector3(pol.pointList[i].transform.position.x, pol.pointList[i].transform.position.y + length,
                pol.pointList[i].transform.position.z);
            var p = PointManager.Instance.SpawnPointAtPosition(pos, false);
            upperPolygonPtList.Add(p);
            inputPtList.Add(pol.pointList[i]);
        }
        
        
        var poly = PolygonManager.Instance.SpawnPolygon(upperPolygonPtList, null, PolygonData.PolygonType.Irregular);
        inputPolList.Add(poly);
        
        for (int i = 0; i < upperPolygonPtList.Count; i++)
        {
            var tmpLineList = new List<GameObject>();
            if (i == upperPolygonPtList.Count - 1)
            {
                tmpLineList.Add(upperPolygonPtList[i]);
                tmpLineList.Add(upperPolygonPtList[0]);
            }
            else
            {
                tmpLineList.Add(upperPolygonPtList[i]);
                tmpLineList.Add(upperPolygonPtList[i+1]);
            }
            var ln = LineManager.Instance.SpawnLine(tmpLineList);
            inputLnList.Add(ln);
            tmpLineList.Clear();
        }
        
        for (int i = 0; i < pol.pointList.Count; i++)
        {
            var tmpPolInput = new List<GameObject>
            {
                pol.pointList[i],
                i == pol.pointList.Count - 1 ? pol.pointList[0] : pol.pointList[i + 1],
                i == pol.pointList.Count - 1 ? upperPolygonPtList[0] : upperPolygonPtList[i+1], 
                upperPolygonPtList[i]
            };
            var tmpLnInput = new List<GameObject>
            {
                upperPolygonPtList[i],
                pol.pointList[i]
            };
            
            var pl = PolygonManager.Instance.SpawnPolygon(tmpPolInput, null, PolygonData.PolygonType.Irregular);
            inputPolList.Add(pl);
            tmpPolInput.Clear();

            var l = LineManager.Instance.SpawnLine(tmpLnInput);
            inputLnList.Add(l);
            tmpLnInput.Clear();
        }
        ShapeManager.Instance.SpawnShape(inputPtList, inputLnList, inputPolList, ShapeData.ShapeType.Cube);
    }

    #endregion
}
