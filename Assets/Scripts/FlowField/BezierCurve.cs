using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    private Vector3[] curvePoints;
    public Vector3[] CalculateCurve(Vector3[] points, int countBetween2Point)
    {
        // 计算贝塞尔曲线上的点集
        curvePoints = new Vector3[countBetween2Point + 1];
        float t = 0f;
        float step = 1f / countBetween2Point;
        int degree = points.Length - 1;
        Vector3 point = Vector3.zero;

        for (int i = 0; i <= countBetween2Point; i++)
        {
            for (int j = 0; j <= degree; j++)
            {
                float blend = BinomialCoefficient(degree, j) * Mathf.Pow(t, j) * Mathf.Pow(1 - t, degree - j);
                point += points[j] * blend;
            }
            curvePoints[i] = point;
            t += step;
            point = Vector3.zero;
        }
        return curvePoints;
    }

    private int BinomialCoefficient(int n, int k)
    {
        int result = 1;
        int min = Mathf.Min(k, n - k);

        for (int i = 0; i < min; i++)
        {
            result *= (n - i);
            result /= (i + 1);
        }
        return result;
    }
}
