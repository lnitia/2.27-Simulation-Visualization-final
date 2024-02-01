using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private CatmulRomCurve catmulRomCurve;
    private BezierCurve bezierCurve;

    [SerializeField] private TextAsset Nodes;
    [SerializeField] private TextAsset Elements;
    [SerializeField] private TextAsset Values;
    [SerializeField] private TextAsset Lines;
    [SerializeField] private  int startPoint;//起始点
    [SerializeField] private  float scale = 0.1f;//点的坐标缩放大小

    [SerializeField] private  int CountBetween2Point = 10;//两点间插值数量

    Vector3[] points; // 存储点的坐标
    Vector3[] directions; // 存储点的速度方向矢量
    Color[] colors; // 存储点的颜色

    Vector3[] linePoints;
    Color[] lineColors;

    [HideInInspector] public Vector3[] curvePoints;
    [HideInInspector] public Gradient gradient;
    
    void Start () 
    {
        GetPoints();
        FindLinePoints();
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        catmulRomCurve = gameObject.AddComponent<CatmulRomCurve>();
        bezierCurve = gameObject.AddComponent<BezierCurve>();
        
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.enableInstancing = true;
        lineRenderer.widthMultiplier = 0.001f;
        
        gradient = new Gradient();
        GradientColorKey[] colorKey;
        if (lineColors.Length >= 8)
        {
            colorKey = new GradientColorKey[8];
            for (int i = 0; i < 8; i++)
            {
                colorKey[i] = new GradientColorKey(lineColors[i], (float)i / 7);
            }
        }
        else
        {
            colorKey = new GradientColorKey[lineColors.Length];
            for (int i = 0; i < lineColors.Length; i++)
            {
                colorKey[i] = new GradientColorKey(lineColors[i], (float)i / (lineColors.Length - 1));
            }
        }
        
        gradient.SetKeys(
            colorKey,
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1f)}
        );
        lineRenderer.colorGradient = gradient;
        
        //curvePoints = new Vector3[CountBetween2Point * (linePoints.Length - 1) + 1];
        curvePoints = new Vector3[CountBetween2Point + 1];
        lineRenderer.positionCount = curvePoints.Length;
    }
    
    void Update()
    {
        //curvePoints = catmulRomCurve.CalculateCurve(linePoints, CountBetween2Point);
        curvePoints = bezierCurve.CalculateCurve(linePoints, CountBetween2Point);
        Vector3 offset = gameObject.transform.localPosition + transform.parent.position;
        for (int i = 0; i < curvePoints.Length; i++)
        {
            curvePoints[i] *= scale;
            curvePoints[i] += offset;
        }
        lineRenderer.SetPositions(curvePoints);
    }
    
    void GetPoints()
    {
        ReadFile readFile = gameObject.AddComponent<ReadFile>();
        readFile.ReadData(Nodes, Elements, Values, Lines);
        
        Vector3[] vertices = new Vector3[readFile.verticeLength];
        Vector3[] vertices2 = new Vector3[readFile.triangleLength / 4];
        int[] triangles = new int[readFile.triangleLength];
        Vector3[] vertices3 = new Vector3[readFile.lineLength];

        if (readFile.numberList1 != null)
        {
            for (int i = 0, vi = 0; i < vertices.Length; i++, vi += 3)
            {
                vertices[i].Set(readFile.numberList1[vi] - 1f, readFile.numberList1[vi + 1], - readFile.numberList1[vi + 2]);
            }
            
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = readFile.numberList2[i] - 1;
            }
            
            //计算四面体中心
            for(int i = 0, j = 0; i < triangles.Length/4; i++, j += 4)
            {
                vertices2[i].x = (vertices[triangles[j]].x + vertices[triangles[j + 1]].x + vertices[triangles[j + 2]].x + vertices[triangles[j + 3]].x) / 4.0f;
                vertices2[i].y = (vertices[triangles[j]].y + vertices[triangles[j + 1]].y + vertices[triangles[j + 2]].y + vertices[triangles[j + 3]].y) / 4.0f;
                vertices2[i].z = (vertices[triangles[j]].z + vertices[triangles[j + 1]].z + vertices[triangles[j + 2]].z + vertices[triangles[j + 3]].z) / 4.0f;
            }

            points = vertices2; 
            float linelength = 200f;
            for (int i = 0, vi = 0; i < vertices3.Length; i++, vi += 3)
            {
                //vertices3[i].Set(readFile.numberList4[vi]  / linelength + points[i].x, readFile.numberList4[vi + 1]  / linelength + points[i].y, - readFile.numberList4[vi + 2]  / linelength + points[i].z);
                vertices3[i].Set(readFile.numberList4[vi], readFile.numberList4[vi + 1], - readFile.numberList4[vi + 2]);
            }

            directions = vertices3;

            SwitchToColor switchToColor = gameObject.AddComponent<SwitchToColor>();
            colors = switchToColor.Switch(readFile.numberList3, readFile.colorLength, readFile.maxvalue, readFile.minvalue,1);
        }
    }

    void FindLinePoints()
    {
        List<Vector3> pointsList = new List<Vector3>();
        List<Color> colorsList = new List<Color>();
        int startIndex = startPoint;
        // // 找到最边上的点（最大的z坐标）
        // int startIndex = -1;
        // float maxZ = float.MinValue;
        //
        // for (int i = 0; i < points.Length; i++)
        // {
        //     if (points[i].z > maxZ)
        //     {
        //         maxZ = points[i].z;
        //         startIndex = i;
        //     }
        // }
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].z + 0.1f >= points[startIndex].z && points[i].y < 1.4f && points[i].y > -0.02f)
            {
                print(i);
            }
        }
        
        // 从最边上的点开始，根据方向矢量寻找下一个最接近的点，并添加到线的列表中
        int currentIndex = startIndex;
        bool[] visited = new bool[points.Length]; // 跟踪已访问的点
        visited[startIndex] = true;

        pointsList.Add(points[currentIndex]);
        colorsList.Add(colors[currentIndex]);

        while (currentIndex < points.Length)
        {
            Vector3 currentDirection = directions[currentIndex];
            float minAngle = float.MaxValue;
            int nextIndex = -1;
    
            // 遍历所有点，找到与给定方向矢量最接近的点
            for (int i = 0; i < points.Length; i++)
            {
                if (i != currentIndex && !visited[i]) // 排除当前点和已访问的点
                {
                    Vector3 toNextPoint = points[i] - points[currentIndex];
                    float dist = Vector3.Distance(points[i], points[currentIndex]);
                    if (dist < 0.3f)
                    {
                        float angle = Vector3.Angle(currentDirection, toNextPoint);
                        if (angle < minAngle & angle <= 30f)
                        {
                            minAngle = angle;
                            nextIndex = i;
                        }
                    }
                }
            }
        
            if (nextIndex != -1)
            {
                currentIndex = nextIndex;
                pointsList.Add(points[currentIndex]);
                colorsList.Add(colors[currentIndex]);
                visited[currentIndex] = true;
            }
            else break; // 如果无法找到下一个点，则跳出循环
        }

        linePoints = new Vector3[pointsList.Count];
        lineColors = new Color[pointsList.Count];

        for (int i = 0; i < pointsList.Count; i++)
        {
            linePoints[i] = pointsList[i];
            lineColors[i] = colorsList[i];
            print(linePoints[i]);
            print(lineColors[i]);
        }
    }
}
