using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BezierCurve catmulRomCurve;

    [SerializeField] private  Color c1 = Color.yellow;
    [SerializeField] private  Color c2 = Color.red;
    [SerializeField] private  Color c3 = Color.blue;
    [SerializeField] private  int pointLength = 20;
    [SerializeField] private  int CountBetween2Point = 10;
    [SerializeField] private  float scale = 0.02f;
    [HideInInspector] public Vector3[] points;
    Vector3[] curvePoints;
    [HideInInspector] public Gradient gradient;
    
    void Start () {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        catmulRomCurve = gameObject.AddComponent<BezierCurve>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.enableInstancing = true;
        lineRenderer.widthMultiplier = 0.001f;

        float alpha = 1.0f;
        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 0.5f),new GradientColorKey(c3, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 0.5f) , new GradientAlphaKey(alpha, 1.0f)}
        );
        
        lineRenderer.colorGradient = gradient;
        points = new Vector3[pointLength];
        for (int i = 0, j = 0; i < points.Length; i++, j += 2)
        {
            points[i] = new Vector3(i * 0.5f * scale, Mathf.Sin(i + j) * scale, j * 0.5f * scale);
        }

        curvePoints = new Vector3[CountBetween2Point + 1];//[CountBetween2Point * (points.Length - 1) + 1];
        lineRenderer.positionCount = curvePoints.Length;
    }
    
    void Update()
    {
        curvePoints = catmulRomCurve.CalculateCurve(points, CountBetween2Point);
        Vector3 offset = gameObject.transform.localPosition + transform.parent.position;
        for (int i = 0; i < curvePoints.Length; i++)
        {
            curvePoints[i] += offset;
        }
        lineRenderer.SetPositions(curvePoints);
    }
}
