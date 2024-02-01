using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(MeshFilter))]
public class MeshStandard : MonoBehaviour
{
    [SerializeField] private TextAsset Nodes;
    [SerializeField] private TextAsset Elements;
    [SerializeField] private TextAsset[] Values;
    [SerializeField] private Vector3 offset;
    [SerializeField] private int mode = 1;
    [SerializeField] private float maxvalue;
    [SerializeField] private float minvalue;

    private ReadFile readFile;
    private MeshFilter mf;
    private MeshRenderer mr;
    private SwitchToColor switchToColor;
    private Color[] colors;
    private float totalTime;
    private float j = 0;

    void Start()
    {
        CreateMesh();
        //InvokeRepeating("UpdateColor",1,1);
    }

    public void ChangeColor(int num)
    {
        if (num <= Values.Length)
        {
            readFile.updateData(Values[num - 1]);
        }
        colors = switchToColor.Switch(readFile.numberList3, readFile.colorLength, maxvalue, minvalue, mode);
        mf.mesh.colors = colors;
    }
    
    private void UpdateColor()
    {
        for (int i = 0; i < Values.Length; i++)
        {
            readFile.updateData(Values[i]);
            if (i == Values.Length - 1)
            {
                i = 0;
            }
        }
        colors = switchToColor.Switch(readFile.numberList3, readFile.colorLength, maxvalue, minvalue, mode);
        mf.mesh.colors = colors;
    }
    private void Update()
    {
        // 颜色改变测试
        // totalTime += Time.deltaTime;
        // if (totalTime >= 1)
        // {
        //     for (int i = 0; i < readFile.colorLength; i++)
        //     {
        //         colors[i] = new Color((i+j)/readFile.colorLength*2,(i-j)/readFile.colorLength*2,i/readFile.colorLength*2);
        //     }
        //     j += 1f;
        //     mf.mesh.colors = colors;
        //     totalTime = 0;
        // }
    }

    void CreateMesh()
    {
        readFile = gameObject.AddComponent<ReadFile>();
        readFile.ReadData(Nodes, Elements, Values[0]);
        
        mf = gameObject.GetComponent<MeshFilter>();
        mr = gameObject.GetComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[readFile.verticeLength];
        int[] triangles = new int[readFile.triangleLength];
        colors = new Color[readFile.colorLength];

        //节点坐标赋给顶点坐标
        for (int i = 0, vi = 0; i < vertices.Length; i++, vi += 3)
        {
            vertices[i].Set(readFile.numberList1[vi] - offset.x, readFile.numberList1[vi + 1] - offset.y, - (readFile.numberList1[vi + 2] - offset.z));
        }

        //节点连接赋给三角形序列
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = readFile.numberList2[i] - 1;
        }

        //单元值转换成顶点颜色
        switchToColor = gameObject.AddComponent<SwitchToColor>();
        colors = switchToColor.Switch(readFile.numberList3, readFile.colorLength, maxvalue, minvalue, mode);
        
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.colors = colors;
        mf.mesh.RecalculateNormals();
    }
}

