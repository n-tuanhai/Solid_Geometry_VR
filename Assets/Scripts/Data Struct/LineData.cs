using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LineData : MonoBehaviour
{
    public PointData startPoint;
    public PointData endPoint;

    public float length;

    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;
    private Mesh mesh;
    public LineData perLine;
    public PolygonData perPol;
    public bool isUpdating;

    public LineConstraint constraint;
    public VRTK_ObjectTooltip tooltip;

    public bool isSelected;

    public enum LineConstraint
    {
        Perpendicular = 1,
        None = 2
    }

    private void OnEnable()
    {
        isSelected = false;
        isUpdating = false;
    }

    private void Start()
    {
        isSelected = false;
        isUpdating = false;
    }

    private void Update()
    {
        if (!isUpdating) return;

        switch (constraint)
        {
            case LineConstraint.Perpendicular:
                if (perLine != null)
                {
                    var start = perLine.startPoint;
                    var end = perLine.endPoint;
                    var originVec = end.position - start.position;
                    var desVec = startPoint.position - start.position;

                    var length = Vector3.Dot(desVec, originVec);
                    var pos = start.position + length * originVec.normalized;

                    endPoint.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
                    endPoint.position = new Vector3(pos.x, pos.y, pos.z);
                }
                else if (perPol != null)
                {
                    var normVecInv =
                        Vector3.Cross((perPol.pointList[2].transform.position - perPol.pointList[1].transform.position),
                                (perPol.pointList[0].transform.position - perPol.pointList[1].transform.position))
                            .normalized;

                    Vector3 a = perPol.pointList[0].transform.position - startPoint.position;
                    float angle = Vector3.Angle(a.normalized, normVecInv);
                    float length = Mathf.Cos(angle * Mathf.Deg2Rad) * a.magnitude;
                    var pos = startPoint.transform.position + normVecInv * length;
                    endPoint.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
                    endPoint.position = new Vector3(pos.x, pos.y, pos.z);
                }

                break;
            case LineConstraint.None:
                break;
            default:
                break;
        }

        SpawnLine();
    }

    public void SpawnLine()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
        length = Vector3.Magnitude(startPoint.position - endPoint.position);
        mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }

    public Vector3 GetMiddlePoint()
    {
        return (startPoint.position + endPoint.position) / 2;
    }

    public Vector3 GetSectionPoint(float ratio)
    {
        return startPoint.position + (endPoint.position - startPoint.position) * ratio;
    }
}