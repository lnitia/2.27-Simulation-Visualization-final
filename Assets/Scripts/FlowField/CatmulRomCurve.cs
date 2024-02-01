using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmulRomCurve : MonoBehaviour
{
    public Vector3[] CalculateCurve(Vector3[] points, int countBetween2Point)
    {
        Vector3[] curvePoints = new Vector3[countBetween2Point * (points.Length - 1) + 1];
        //依次计算相邻两点间曲线
        //由四个点确定一条曲线（当前相邻两点p1,p2，以及前后各一点p0,p3）
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 firstPos, curPos, nextPos, lastPos;
            //特殊位置增加虚拟点
            //如果p1点是第一个点，不存在p0点，由p1,p2确定一条直线，在向量(p2p1)方向确定虚拟点p0
            if (i == 0)
                firstPos = points[i] * 2 - points[i + 1];
            else
                firstPos = points[i - 1];
            //中间点
            curPos = points[i];
            nextPos = points[i + 1];
            //特殊位置增加虚拟点，同上
            if (i == points.Length - 2)
                lastPos = points[i + 1] * 2 - points[i];
            else
                lastPos = points[i + 2];

            CatmulRom(firstPos, curPos, nextPos, lastPos, ref curvePoints, countBetween2Point * i, countBetween2Point);
        }
        //加入最后一个点位
        curvePoints[curvePoints.Length - 1] = points[points.Length - 1];
        return curvePoints;
    }

    //平滑过渡两点间曲线（p1,p2为端点，p0,p3是控制点）
    void CatmulRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, ref Vector3[] rompoints, int startIndex, int countBetween2Point)
    {
        //计算Catmull-Rom样条曲线
        float t0 = 0;
        float t1 = GetT(t0, p0, p1);
        float t2 = GetT(t1, p1, p2);
        float t3 = GetT(t2, p2, p3);

        float t;
        for (int i = 0; i < countBetween2Point; i++)
        {
            t = t1 + (t2 - t1) / countBetween2Point * i;

            Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            rompoints[startIndex + i] = C;
        }
    }

    float GetT(float t, Vector3 p0, Vector3 p1)
    {
        return t + Mathf.Pow(Mathf.Pow((p1.x - p0.x), 2) + Mathf.Pow((p1.y - p0.y) , 2) + Mathf.Pow((p1.z - p0.z), 2), 0.5f);
    }
    //动态sin曲线
    // void Update()
    // {
    //     LineRenderer lineRenderer = GetComponent<LineRenderer>();
    //     var points = new Vector3[lengthOfLineRenderer];
    //     var t = Time.time;
    //     for (int i = 0; i < lengthOfLineRenderer; i++)
    //     {
    //         points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f);
    //     }
    //     lineRenderer.SetPositions(points);
    // }
}
