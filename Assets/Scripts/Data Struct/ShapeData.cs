using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeData : MonoBehaviour
{
   public enum ShapeType
   {
      Cube = 1,
      Tetrahedron = 2,
      Pyramid = 3,
      Prism = 4
   }

   public List<GameObject> pointList;
   public List<LineData> lineList;
   public List<PolygonData> polygonList;
   public ShapeType shapeType;
}
