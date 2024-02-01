using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class meshsolid : MonoBehaviour
{
    public TextAsset Nodes;
    public TextAsset Elements;

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

    void ReadData()
    {
        //读取节点坐标
        XmlDocument xmlnode = new XmlDocument();
        TextAsset assetnode = Nodes;
        //翻译字符串为xml对象
        xmlnode.LoadXml(assetnode.text);
        //获取XML中的节点
        XmlNodeList nodeList = xmlnode.SelectSingleNode("Nodes").ChildNodes;

        foreach (XmlNode item in nodeList)
        {
            _node = item.SelectSingleNode("gcoord").InnerText;
            //SPlit函数只能分割单个空格，所以两个空格替换成一个空格
            _nodeData_Space = _node.Replace("  ", " ");
            //分割字符串
            _nodeData_Array = _nodeData_Space.Split(' ');
            //保存获取到的数据
            _nodeData_FloatArray = new float[3];
            for (int i = 0; i < 3; i++)
            {
                _nodeData_FloatArray[i] = float.Parse(_nodeData_Array[i]);
                numberList1.Add(_nodeData_FloatArray[i]);
            }
            _nodeLength = item.Attributes["id"].Value;
        }
        verticeLength = int.Parse(_nodeLength);
        //print(verticeLength);

        //读取节点连接
        XmlDocument xmltab = new XmlDocument();
        TextAsset assettab = Elements;
        xmltab.LoadXml(assettab.text);

        XmlNodeList ElementList = xmltab.SelectSingleNode("Elements").ChildNodes;

        foreach (XmlNode item in ElementList)
        {
            _ele = item.SelectSingleNode("node").InnerText;
            _eleData_Space = _ele.Replace("  ", " ");
            _eleData_Array = _eleData_Space.Split(' ');
            _eleData_IntArray = new int[4];

            for (int i = 0; i < 4; i++)
            {
                _eleData_IntArray[i] = int.Parse(_eleData_Array[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = i; j - i < 3; j++)
                {
                    if (j < 4)
                        numberList2.Add(_eleData_IntArray[j]);
                    else
                        numberList2.Add(_eleData_IntArray[j - 4]);
                }
            }
            _eleLength = item.Attributes["id"].Value;
        }
        triangleLength = int.Parse(_eleLength) * 12;
        //print(triangleLength);
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
        //print(vertices1.Length);
        //单元连接赋给三角形序列
        for (int i = 0; i < triangles1.Length; i++)
        {
            triangles1[i] = numberList2[i] - 1;
        }
        //print(triangles1.Length);
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

        mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mf.mesh.vertices = vertices2;
        mf.mesh.triangles = triangles2;
        mf.mesh.RecalculateNormals();
    }
}
