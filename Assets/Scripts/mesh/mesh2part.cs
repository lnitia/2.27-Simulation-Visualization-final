using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class mesh2part : MonoBehaviour
{
    [SerializeField] private TextAsset Nodes;
    [SerializeField] private TextAsset Elements;
    [SerializeField] private TextAsset Values;
    [SerializeField] private float ValueOffset;

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
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = i; j - i < 3; j++)
            //    {
            //        if (j < 4)
            //            numberList2.Add(_eleData_IntArray[j]);
            //        else
            //            numberList2.Add(_eleData_IntArray[j - 4]);
            //    }
            //}
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

            _eleLength = item.Attributes["id"].Value;
        }
        triangleLength = int.Parse(_eleLength) * 12;
        //print(triangleLength);


        //读取单元值
        XmlDocument xmlvalue = new XmlDocument();
        TextAsset assetvalue = Values;
        xmlvalue.LoadXml(assetvalue.text);

        XmlNodeList ValueList = xmlvalue.SelectSingleNode("Elements").ChildNodes;

        foreach (XmlNode item in ValueList)
        {
            XmlNode _valuelist = item.LastChild;
            _valueData = float.Parse(_valuelist.InnerText);
            numberList3.Add(_valueData);
            _valueLength = item.Attributes["id"].Value;
        }
        colorLength = int.Parse(_valueLength);
        maxvalue = numberList3.Max() + 0.01f;
        minvalue = numberList3.Min() - 0.01f;
        //print(colorLength);
        print(maxvalue + "  " + minvalue);
    }


    void CreateMesh()
    {
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[verticeLength];
        int[] triangles = new int[triangleLength];
        Color[] colors = new Color[colorLength];

        //节点坐标赋给顶点坐标
        for (int i = 0, vi = 0; i < vertices.Length; i++, vi += 3)
        {
            vertices[i].Set(numberList1[vi] - 0.73f, numberList1[vi + 1], - numberList1[vi + 2]);
        }

        //单元连接赋给三角形序列
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = numberList2[i] - 1;
        }

        //单元值转换成顶点颜色
        float _range = maxvalue - minvalue;
        //将_data映射到不同的色彩区间中
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i] - ValueOffset;
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
        }

        mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.colors = colors;
        mf.mesh.RecalculateNormals();
    }
}

