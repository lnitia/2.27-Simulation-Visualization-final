using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FlowField : MonoBehaviour
{
    [SerializeField] private TextAsset Nodes;
    [SerializeField] private TextAsset Elements;
    [SerializeField] private TextAsset Values;
    [SerializeField] private TextAsset Lines;
    [SerializeField] private float radius = 0.01f;//小球的直径
    [SerializeField] private float linelength = 200f;//线条长度变换值，值越小线条越长

    //保存数据
    private List<float> numberList1 = new List<float>();
    private List<int> numberList2 = new List<int>();
    private List<float> numberList3 = new List<float>();
    private List<float> numberList4 = new List<float>();

    string[] _nodeData_Array;
    float[] _nodeData_FloatArray;

    string[] _eleData_Array;
    int[] _eleData_IntArray;

    float _valueData;
    
    string[] _lineData_Array;
    float[] _lineData_FloatArray;

    //保存长度
    int verticeLength;
    int triangleLength;
    int colorLength;
    int lineLength;

    //单元值范围
    float maxvalue;
    float minvalue;

    //保存中心点
    private List<float> centerlist = new List<float>();
    private List<float> colorlist = new List<float>();
    private List<float> centertolist = new List<float>();

    void Start()
    {
        ReadData();
        GetCenter();
    }

    private void ReadData()
    {
        //读取节点坐标
        TextAsset assetnode = Nodes;
        string[] nodeList = assetnode.text.Split('\n');
        foreach (string item in nodeList)
        {
            //分割字符串
            _nodeData_Array = item.Split(' ');
            //保存获取到的数据
            _nodeData_FloatArray = new float[3];
            for (int i = 0, j = 2; i < 3; i++, j++)
            {
                _nodeData_FloatArray[i] = float.Parse(_nodeData_Array[j]);
                numberList1.Add(_nodeData_FloatArray[i]);
            }
        }
        verticeLength = nodeList.Length;

        //读取节点连接
        TextAsset assettab = Elements;
        string[] ElementList = assettab.text.Split('\n');
        foreach (string item in ElementList)
        {
            _eleData_Array = item.Split(' ');
            _eleData_IntArray = new int[_eleData_Array.Length];

            for (int i = 0; i < _eleData_Array.Length; i++)
            {
                int.TryParse(_eleData_Array[i], out _eleData_IntArray[i]);
                numberList2.Add(_eleData_IntArray[i]);
            }
        }
        triangleLength = ElementList.Length * 4;

        //读取单元值
        TextAsset assetvalue = Values;
        string[] ValueList = assetvalue.text.Split('\n');
        foreach (string item in ValueList)
        {
            _valueData = float.Parse(item);
            numberList3.Add(_valueData);
        }
        colorLength = ValueList.Length;
        maxvalue = numberList3.Max() + 0.01f;
        minvalue = numberList3.Min() - 0.01f;
        
        //读取速度方向坐标
        TextAsset assetline = Lines;
        string[] lineList = assetline.text.Split('\n');
        foreach (string item in lineList)
        {
            //分割字符串
            _lineData_Array = item.Split(' ');
            //保存获取到的数据
            _lineData_FloatArray = new float[3];
            for (int i = 0; i < 3; i++)
            {
                _lineData_FloatArray[i] = float.Parse(_lineData_Array[i]);
                numberList4.Add(_lineData_FloatArray[i]);
            }
        }
        lineLength = lineList.Length;
    }

    private void GetCenter()
    {
        Vector3[] vertices1 = new Vector3[verticeLength];
        Vector3[] vertices2 = new Vector3[triangleLength / 4];
        int[] triangles = new int[triangleLength];
        Color[] colors = new Color[colorLength];
        Vector3[] vertices3 = new Vector3[lineLength];

        //节点坐标赋给顶点坐标
        for (int i = 0, vi = 0; i < vertices1.Length; i++, vi += 3)
        {
            vertices1[i].Set(numberList1[vi] - 1f, numberList1[vi + 1], - numberList1[vi + 2]);
            //print(i + $"({Math.Round(vertices1[i].x, 6)}, {Math.Round(vertices1[i].y, 6)}, {Math.Round(vertices1[i].z, 6)})");
        }

        //单元连接赋给三角形序列
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = numberList2[i] - 1;
        }

        //计算四面体中心
        for(int i = 0, j = 0; i < triangles.Length/4; i++, j += 4)
        {
            vertices2[i].x = (vertices1[triangles[j]].x + vertices1[triangles[j + 1]].x + vertices1[triangles[j + 2]].x + vertices1[triangles[j + 3]].x) / 4.0f;
            vertices2[i].y = (vertices1[triangles[j]].y + vertices1[triangles[j + 1]].y + vertices1[triangles[j + 2]].y + vertices1[triangles[j + 3]].y) / 4.0f;
            vertices2[i].z = (vertices1[triangles[j]].z + vertices1[triangles[j + 1]].z + vertices1[triangles[j + 2]].z + vertices1[triangles[j + 3]].z) / 4.0f;
            centerlist.Add(vertices2[i].x);
            centerlist.Add(vertices2[i].y);
            centerlist.Add(vertices2[i].z);
            //print(i + $"({Math.Round(vertices2[i].x, 6)}, {Math.Round(vertices2[i].y, 6)}, {Math.Round(vertices2[i].z, 6)})");
        }

        //单元值转换成顶点颜色
        float _range = maxvalue - minvalue + 1;
        //将_data映射到不同的色彩区间中
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i];
            float r = (_data - minvalue) / _range;
            float step = _range / 4;
            int idx = (int)(r * 4.0);
            float h = (idx + 1) * step + minvalue;
            float m = idx * step + minvalue;
            float local_r = (_data - m) / (h - m);
            if (_data < minvalue)
                colors[i] = new Color(0, 0, 0);
            if (_data > maxvalue)
                colors[i] = new Color(1, 1, 1);
            if (idx == 0)
                colors[i] = new Color(1, local_r, 0);
            if (idx == 1)
                colors[i] = new Color(1 - local_r, 1, 0);
            if (idx == 2)
                colors[i] = new Color(0, 1, local_r);
            if (idx == 3)
                colors[i] = new Color(0, 1 - local_r, 1);
            colorlist.Add(colors[i].r);
            colorlist.Add(colors[i].g);
            colorlist.Add(colors[i].b);
        }
        
        //速度方向坐标赋给centerto[]
        for (int i = 0, vi = 0; i < vertices3.Length; i++, vi += 3)
        {
            vertices3[i].Set(numberList4[vi] / linelength, numberList4[vi + 1] / linelength, - numberList4[vi + 2] / linelength);
            centertolist.Add(vertices3[i].x + vertices2[i].x);
            centertolist.Add(vertices3[i].y + vertices2[i].y);
            centertolist.Add(vertices3[i].z + vertices2[i].z);
            //print(i + $"({Math.Round(vertices3[i].x, 6)}, {Math.Round(vertices3[i].y, 6)}, {Math.Round(vertices3[i].z, 6)})");
        }
    }

    public void OnDrawGizmos()
    {
        
        Vector3[] center = new Vector3[colorLength];
        Color[] colors = new Color[colorLength];
        Vector3[] centerto = new Vector3[colorLength];
        //Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        //MeshRenderer mr = GetComponent<MeshRenderer>(); 
        
        for (int i = 0, vi = 0; i < colorLength; i++, vi += 3)
        {
            center[i].Set(centerlist[vi], centerlist[vi + 1], centerlist[vi + 2]);
            centerto[i].Set(centertolist[vi], centertolist[vi + 1], centertolist[vi + 2]);
            colors[i].r = colorlist[vi];
            colors[i].g = colorlist[vi + 1];
            colors[i].b = colorlist[vi + 2];
            colors[i].a = 1;
        }
        for (int i = 0; i < center.Length; i++)
        {
            Gizmos.color = colors[i];
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(center[i], radius);
            Gizmos.DrawLine(center[i], centerto[i]);
            //Graphics.DrawMesh(mesh, center[i], transform.rotation, mr.sharedMaterial, 0);
        }
    }

    // [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
    // static void RegisterTypes()
    // {
    //     UnityEngine.WSA.Application.InvokeOnUIThread(() =>
    //     {
    //         // Your code here
    //     }, false);
    // }
}
