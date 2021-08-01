using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using JetBrains.Annotations;
using UnityEngine;

namespace Line
{
    public class LineManager : MonoBehaviour
    {
        public static LineManager Instance;

        public List<LineData> lineList;
        public GameObject linePrefab;

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

        public LineData SpawnLine(List<GameObject> ptList)
        {
            var ln = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
            var lnData = ln.GetComponent<LineData>();
            var inputList = new List<PointData>();
            inputList.Add(ptList[0].GetComponent<PointData>());
            inputList.Add(ptList[1].GetComponent<PointData>());
            lnData.startPoint = inputList[0];
            lnData.endPoint = inputList[1];
            lnData.SpawnLine();
            lineList.Add(lnData);
            return lnData;
        }

        public void SpawnPerpendicularLine(List<GameObject> objectList)
        {
            var pt = objectList[0].GetComponent<PointData>();
            var pol = objectList[1].GetComponent<PolygonData>();
            var ln = objectList[1].GetComponent<LineData>();

            var tmpLineInput = new List<GameObject>();
            tmpLineInput.Add(pt.gameObject);

            if (pol != null)
            {
                var normVec = Vector3.Cross((pol.pointList[0].transform.position - pol.pointList[1].transform.position),
                    (pol.pointList[2].transform.position - pol.pointList[1].transform.position)).normalized;
                var normVecInv =
                    Vector3.Cross((pol.pointList[2].transform.position - pol.pointList[1].transform.position),
                        (pol.pointList[0].transform.position - pol.pointList[1].transform.position)).normalized;

                Vector3 a = pol.pointList[0].transform.position - pt.transform.position;
                float angle = Vector3.Angle(a.normalized, normVecInv);
                float length = Mathf.Cos(angle * Mathf.Deg2Rad) * a.magnitude;
                var pos = pt.transform.position + normVecInv * length;
                var p = PointManager.Instance.SpawnPointAtPosition(pos, false);

                tmpLineInput.Add(p);

                if (tmpLineInput.Count == 2)
                {
                    var l = SpawnLine(tmpLineInput);
                    l.constraint = LineData.LineConstraint.Perpendicular;
                    l.perPol = pol;
                    l.perLine = null;
                }
            }
            else if (ln != null)
            {
                var start = ln.startPoint;
                var end = ln.endPoint;
                var originVec = end.position - start.position;
                var desVec = pt.position - start.position;

                var length = Vector3.Dot(desVec, originVec);
                var pos = start.position + length * originVec.normalized;

                var p = PointManager.Instance.SpawnPointAtPosition(pos, false);
                tmpLineInput.Add(p);
                var l = SpawnLine(tmpLineInput); 
                l.constraint = LineData.LineConstraint.Perpendicular;
                l.perLine = ln;
                l.perPol = null;
            }
        }

        public List<LineData> FindLine(GameObject pt)
        {
            List<LineData> lnList = new List<LineData>();
            var pos = pt.transform.position;
            foreach (var ln in lineList)
            {
                if (ln.endPoint.position == pos || ln.startPoint.position == pos)
                {
                    lnList.Add(ln);
                }
            }

            return lnList;
        }

        public LineData FindSpecificLine(GameObject firstPt, GameObject secondPt)
        {
            LineData res = null;
            foreach (var ln in lineList)
            {
                if ((ln.startPoint == firstPt.GetComponent<PointData>() && ln.endPoint == secondPt.GetComponent<PointData>()) || 
                    (ln.endPoint == firstPt.GetComponent<PointData>() && ln.startPoint == secondPt.GetComponent<PointData>()))
                {
                    res = ln;
                }
            }
            
            return res;
        }

        public void RemoveLine(LineData ln)
        {
            lineList.Remove(ln);
        }
    }
}