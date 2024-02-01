using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class meshtxt : MonoBehaviour
{
    public TextAsset Nodes;
    public TextAsset Elements;
    public TextAsset Values;

    //读取数据
    private string _node;
    private string _ele;
    //保存数据
    private List<float> numberList1 = new List<float>();
    private List<int> numberList2 = new List<int>();
    private List<float> numberList3 = new List<float>();

    string _nodeData_Space;
    string[] _nodeData_Array;
    float[] _nodeData_FloatArray;

    string _eleData_Space;
    string[] _eleData_Array;
    int[] _eleData_IntArray;

    float _valueData;
    //保存长度
    string _nodeLength;
    int verticeLength;
    string _eleLength;
    int triangleLength;
    string _valueLength;
    int colorLength;
    //单元值范围
    float maxvalue;
    float minvalue;

    void Start()
    {
        ReadData();
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReadData()
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
        print(verticeLength);

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
            }
            numberList2.Add(_eleData_IntArray[0]);
            numberList2.Add(_eleData_IntArray[1]);
            numberList2.Add(_eleData_IntArray[2]);

            numberList2.Add(_eleData_IntArray[2]);
            numberList2.Add(_eleData_IntArray[1]);
            numberList2.Add(_eleData_IntArray[3]);

            numberList2.Add(_eleData_IntArray[2]);
            numberList2.Add(_eleData_IntArray[3]);
            numberList2.Add(_eleData_IntArray[0]);

            numberList2.Add(_eleData_IntArray[3]);
            numberList2.Add(_eleData_IntArray[1]);
            numberList2.Add(_eleData_IntArray[0]);
        }
        triangleLength = ElementList.Length * 12;
        print(triangleLength);


        //读取单元值
        TextAsset assetvalue = Values;
        string[] ValueList = assetvalue.text.Split('\n');

        foreach (string item in ValueList)
        {
            _valueData = float.Parse(item);
            numberList3.Add(_valueData);
        }
        colorLength = ValueList.Length * 12;
        maxvalue = numberList3.Max() + 0.01f;
        minvalue = numberList3.Min() - 0.01f;
        print(colorLength);
    }


    void CreateMesh()
    {
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

        Vector3[] vertices1 = new Vector3[verticeLength];
        Vector3[] vertices2 = new Vector3[triangleLength];
        int[] triangles1 = new int[triangleLength];
        int[] triangles2 = new int[triangleLength];
        Color[] colors = new Color[colorLength];

        //节点坐标赋给顶点坐标
        for (int i = 0, vi = 0; i < vertices1.Length; i++, vi += 3)
        {
            vertices1[i].Set(numberList1[vi] - 1, numberList1[vi + 1], -numberList1[vi + 2]);
        }

        //单元连接赋给三角形序列
        for (int i = 0; i < triangles1.Length; i++)
        {
            triangles1[i] = numberList2[i] - 1;
        }
        //转换成单个三角形
        for (int i = 0, vi = 0; i < vertices2.Length; i += 3, vi += 3)
        {
            vertices2[i] = vertices1[triangles1[vi]];
            vertices2[i + 1] = vertices1[triangles1[vi + 1]];
            vertices2[i + 2] = vertices1[triangles1[vi + 2]];
        }

        for (int i = 0; i < triangles1.Length; i++)
        {
            triangles2[i] = i;
        }

        //单元值转换成顶点颜色
        float _range = maxvalue - minvalue + 1;
        //将_data映射到不同的色彩区间中
        for (int i = 0, vi = 0; i < colors.Length; i += 12, vi++)
        {
            float _data = numberList3[vi];
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
            for (int j = 1; j < 12; j++)
            {
                colors[j + i] = colors[i];
            }
        }

        mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mf.mesh.vertices = vertices2;
        mf.mesh.triangles = triangles2;
        mf.mesh.colors = colors;
        mf.mesh.RecalculateNormals();
    }
}


