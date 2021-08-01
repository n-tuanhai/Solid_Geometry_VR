using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance;
    public List<ShapeData> shapeList;

    public GameObject shapePrefab;
    
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

    public void SpawnShape(List<GameObject> ptList, List<LineData> lnList, List<PolygonData> polList, ShapeData.ShapeType type)
    {
        var shape = Instantiate(shapePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ShapeData>();
        shape.pointList = ptList;
        shape.lineList = lnList;
        shape.polygonList = polList;
        shape.shapeType = type;
        shapeList.Add(shape.GetComponent<ShapeData>());
    }

    public List<ShapeData> FindShape(GameObject point)
    {
        List<ShapeData> sList = new List<ShapeData>();
        foreach (var shp in shapeList)
        {
            if (shp.pointList.Contains(point))
            {
                sList.Add(shp);
            }
        }

        return sList;
    }

    public void RemoveShape(ShapeData s)
    {
        shapeList.Remove(s);
    }
}
