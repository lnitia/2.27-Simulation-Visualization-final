using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class mesh : MonoBehaviour
{
    //读取数据
    private string _node;
    private string _tab;
    private string _p;
    //保存数据
    private List<float> numberList1 = new List<float>();
    private List<int> numberList2 = new List<int>();
    private List<float> numberList3 = new List<float>();

    string _nodeData_Space;
    string[] _nodeData_Array;
    float[] _nodeData_FloatArray;

    string _tabData_Space;
    string[] _tabData_Array;
    int[] _tabData_IntArray;

    float _pData;
    //保存长度
    string _nodeLength;
    int verticeLength;
    string _tabLength;
    int triangleLength;
    string _pLength;
    int colorLength;
    //单元值范围
    int _pmax = 500;
    int _pmin = -500;

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
        //xml.Load(Application.streamingAssetsPath + "/Slun_node.xml");
        TextAsset assetnode = Resources.Load<TextAsset>("Slun_node");
        print("1" + assetnode.text);
        // 通过这个方法 就能够翻译字符串为xml对象
        xmlnode.LoadXml(assetnode.text);
        //  获取XML中的根节点
        XmlNode nodes = xmlnode.SelectSingleNode("Nodes");
        //  再通过跟节点去获取下面的子节点
        //XmlNode node = nodes.SelectSingleNode("node");
        //  获取节点中包裹的内容
        // print(node.Attributes["id"].Value);
        XmlNodeList nodeList = nodes.SelectNodes("node");
        //  通过遍历这个List就可以得到其中的所有子节点了
        foreach (XmlNode item in nodeList)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("gcoord").InnerText);
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
            }
            //添加到List数组中
            for (int i = 0; i < 3; i++)
            {
                numberList1.Add(_nodeData_FloatArray[i]);
            }
            _nodeLength = item.Attributes["id"].Value;
        }
        verticeLength = int.Parse(_nodeLength);
        //print(verticeLength);

        //读取节点连接
        XmlDocument xmltab = new XmlDocument();
        TextAsset assettab = Resources.Load<TextAsset>("Slun_tab");

        print(assettab.text);
        xmltab.LoadXml(assettab.text);

        XmlNode Element = xmltab.SelectSingleNode("Elements");

        XmlNodeList ElementList = Element.SelectNodes("Element");

        foreach (XmlNode item in ElementList)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("node").InnerText);

            _tab = item.SelectSingleNode("node").InnerText;
            _tabData_Space = _tab.Replace("  ", " ");
            _tabData_Array = _tabData_Space.Split(' ');
            _tabData_IntArray = new int[3];

            for (int i = 0; i < 3; i++)
            {
                _tabData_IntArray[i] = int.Parse(_tabData_Array[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                numberList2.Add(_tabData_IntArray[i]);
            }
            _tabLength = item.Attributes["id"].Value;
        }
        triangleLength = int.Parse(_tabLength);
        //print(triangleLength);


        //读取单元值
        XmlDocument xmlp = new XmlDocument();
        TextAsset assetp = Resources.Load<TextAsset>("Slun_p");

        print(assetp.text);
        xmlp.LoadXml(assetp.text);

        XmlNode Elementp = xmlp.SelectSingleNode("Elements");

        XmlNodeList ElementListp = Elementp.SelectNodes("Element");

        foreach (XmlNode item in ElementListp)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("Solid_P").InnerText);
            _p = item.SelectSingleNode("Solid_P").InnerText;
            _pData = float.Parse(_p);
            numberList3.Add(_pData);
            _pLength = item.Attributes["id"].Value;
        }
        colorLength = int.Parse(_pLength);
        //print(colorLength);
    }


    void CreateMesh()
    {

        //GameObject obj = new GameObject("mesh");
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();


        Vector3[] vertices = new Vector3[verticeLength];//[verticeLength];
        int[] triangles = new int[triangleLength];//[triangleLength];
        Color[] colors = new Color[colorLength];//[colorLength];

        //节点坐标赋给顶点坐标
        for (int i = 0, vi = 0; i < vertices.Length; i++, vi += 3)
        {
            vertices[i].Set(numberList1[vi], numberList1[vi + 1], numberList1[vi + 2]);
            //print(vertices[i]);
        }

        //单元连接赋给三角形序列
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = numberList2[i] - 1;
            //print(triangles[i]);
        }

        //单元值转换成顶点颜色
        int _range = _pmax - _pmin + 1;
        //将_data映射到不同的色彩区间中
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i];
            float r = (_data - _pmin) / _range;
            int step = _range / 5;
            int idx = (int)(r * 5.0);
            int h = (idx + 1) * step + _pmin;
            int m = idx * step + _pmin;
            float local_r = (_data - m) / (h - m);
            if (_data < _pmin)
                colors[i] = new Color(0, 0, 0);
            if (_data > _pmax)
                colors[i] = new Color(1, 1, 1);
            if (idx == 0)
                colors[i] = new Color(0, local_r, 1);
            if (idx == 1)
                colors[i] = new Color(0, 1, 1 - local_r);
            if (idx == 2)
                colors[i] = new Color(local_r, 1, 0);
            if (idx == 3)
                colors[i] = new Color(1, 1 - local_r, 0);
            if (idx == 4)
                colors[i] = new Color(1, 0, local_r);
        }

        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.colors = colors;

    }
}
