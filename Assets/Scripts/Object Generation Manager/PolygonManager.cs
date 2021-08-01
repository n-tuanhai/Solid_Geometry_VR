using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PolygonManager : MonoBehaviour
{
    public static PolygonManager Instance;

    public GameObject polygonPrefab;
    public List<PolygonData> polygonList;

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
    }

    public PolygonData SpawnPolygon(List<GameObject> ptList, [CanBeNull] List<LineData> lineList,
        PolygonData.PolygonType type)
    {
        var pol = Instantiate(polygonPrefab, Vector3.zero, Quaternion.identity, transform);
        var polData = pol.GetComponent<PolygonData>();
        polData.lineList = lineList;
        polData.pointList = ptList;
        polData.InitMesh();
        polData.polygonType = type;
        polygonList.Add(polData);
        return polData;
    }

    public List<Vector3> GetRegularPolygonVerticesPosition(List<GameObject> ptList, int verts)
    {
        List<Vector3> vertices = new List<Vector3>();
        float height = 0;
        float angleStep = 2 * Mathf.PI / verts;
        float currentAngle;
        List<Vector2> startingPoint = new List<Vector2>();
        Vector2 currentPos = Vector2.one;

        if (ptList.Count == 1)
        {
            var lineDt = ptList[0].GetComponent<LineData>();
            height = lineDt.startPoint.transform.position.y;
            currentPos = new Vector2(lineDt.endPoint.transform.position.x, lineDt.endPoint.transform.position.z);

            startingPoint.Add(new Vector2(lineDt.startPoint.transform.position.x,
                lineDt.startPoint.transform.position.z));
            startingPoint.Add(new Vector2(lineDt.endPoint.transform.position.x,
                lineDt.endPoint.transform.position.z));
        }
        else if (ptList.Count == 2)
        {
            height = ptList[0].transform.position.y;
            currentPos = new Vector2(ptList[1].transform.position.x, ptList[1].transform.position.z);
            startingPoint.Add(new Vector2(ptList[0].transform.position.x, ptList[0].transform.position.z));
            startingPoint.Add(new Vector2(ptList[1].transform.position.x, ptList[1].transform.position.z));
        }

        vertices.Add(startingPoint[0]);
        vertices.Add(startingPoint[1]);

        float length = Vector2.Distance(startingPoint[0], startingPoint[1]);
        currentAngle = Mathf.Acos((startingPoint[0].x - startingPoint[1].x) / length);
        if (currentAngle < 0)
        {
            currentAngle = 2 * Mathf.PI - currentAngle;
        }

        List<Vector3> vert = new List<Vector3>();
        for (int i = 0; i < verts - 1; i++)
        {
            currentPos = new Vector2(
                currentPos.x + Mathf.Cos(currentAngle) * length,
                currentPos.y + Mathf.Sin(currentAngle) * length
            );
            if (i != 0)
            {
                vert.Add(new Vector3(currentPos.x, height, currentPos.y));
            }

            currentAngle += angleStep;
        }

        vert.Reverse();
        foreach (var v in vert)
        {
            vertices.Add(v);
        }

        return vertices;
    }

    public List<PolygonData> FindPolygon(GameObject pt)
    {
        List<PolygonData> polList = new List<PolygonData>();
        foreach (var pol in polygonList)
        {
            foreach (var p in pol.pointList)
            {
                if (p.transform.position == pt.transform.position)
                {
                    polList.Add(pol);
                }
            }
        }

        return polList;
    }

    public void RemovePolygon(PolygonData pol)
    {
        polygonList.Remove(pol);
    }
}