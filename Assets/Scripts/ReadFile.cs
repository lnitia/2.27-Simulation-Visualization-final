using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;

public class ReadFile : MonoBehaviour
{
    //保存数据
    [HideInInspector] public List<float> numberList1 = new List<float>();//节点
    [HideInInspector] public List<int> numberList2 = new List<int>();//节点连接
    [HideInInspector] public List<float> numberList3 = new List<float>();//强度、疲劳、压力值
    [HideInInspector] public List<float> numberList4 = new List<float>();//速度矢量

    string[] _nodeData_Array;
    float[] _nodeData_FloatArray;

    string[] _eleData_Array;
    int[] _eleData_IntArray;

    float _valueData;
    
    string[] _lineData_Array;
    float[] _lineData_FloatArray;

    //保存长度
    public int verticeLength;
    public int triangleLength;
    public int colorLength;
    public int lineLength;

    //单元值范围
    public float maxvalue;
    public float minvalue;
    
    /// <summary>
    /// 流场txt文件数据读取
    /// </summary>
    /// <param name="Nodes"></param>
    /// <param name="Elements"></param>
    /// <param name="Values"></param>
    /// <param name="Lines"></param>
    public void ReadData(TextAsset Nodes,TextAsset Elements,TextAsset Values,TextAsset Lines)
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
        print("read data successful");
    }
    
    /// <summary>
    /// xml文件FAE数据读取
    /// </summary>
    /// <param name="Nodes"></param>
    /// <param name="Elements"></param>
    /// <param name="Values"></param>
    public void ReadData(TextAsset Nodes, TextAsset Elements, TextAsset Values)
    {
        string _node;
        string _ele;
        string _nodeData_Space;
        string _eleData_Space;

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
        }
        verticeLength = nodeList.Count;
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
            // _eleData_IntArray = new int[3];
            //
            // for (int i = 0; i < 3; i++)
            // {
            //     _eleData_IntArray[i] = int.Parse(_eleData_Array[i]);
            //     numberList2.Add(_eleData_IntArray[i]);
            // }
            _eleData_IntArray = new int[4];

            for (int i = 0; i < 4; i++)
            {
                _eleData_IntArray[i] = int.Parse(_eleData_Array[i]);
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
        triangleLength = ElementList.Count * 12;//正常为3
        //print(triangleLength);
        
        //读取单元值
        updateData(Values);
        //print(colorLength);
        //print(maxvalue + "  " + minvalue);
    }
    
    public void updateData(TextAsset Values)
    {
        //读取单元值
        numberList3.Clear();
        XmlDocument xmlvalue = new XmlDocument();
        TextAsset assetvalue = Values;
        xmlvalue.LoadXml(assetvalue.text);

        XmlNodeList ValueList = xmlvalue.SelectSingleNode("Nodes").ChildNodes;//之前的文件中为Elements

        foreach (XmlNode item in ValueList)
        {
            XmlNode _valuelist = item.LastChild;
            _valueData = float.Parse(_valuelist.InnerText);
            numberList3.Add(_valueData);
        }
        colorLength = ValueList.Count;
        maxvalue = numberList3.Max() + 0.01f;
        minvalue = numberList3.Min() - 0.01f;
    }
}
