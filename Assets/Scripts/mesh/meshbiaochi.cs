using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshbiaochi : MonoBehaviour
{
    public float width = 0.1f;

    int _pmax = 500;
    int _pmin = -500;

    void Start()
    {
        CreateMesh();
    }

    void CreateMesh()
    {

        //GameObject obj = new GameObject("mesh");
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();


        Vector3[] vertices = new Vector3[10];

        for (int i = 0, vi = 0; i < vertices.Length; i += 2, vi += 1)
        {

            vertices[i] = new Vector3(0, (float)(width * vi), 0);
            vertices[i + 1] = new Vector3(width, (float)(width * vi), 0);
            print(vertices[i]);
        }

        int[] triangles = new int[24];

        for (int i = 0, ti = 0; i < triangles.Length; i += 6, ti += 2)
        {
            triangles[i] = ti;
            triangles[i + 1] = ti + 2;
            triangles[i + 2] = ti + 1;

            triangles[i + 3] = ti + 1;
            triangles[i + 4] = ti + 2;
            triangles[i + 5] = ti + 3;
            print(triangles[i]);
        }

        Color[] colors = new Color[vertices.Length];
        float[] numberList = new float[vertices.Length];

        //for (int i = 0; i < vertices.Length; i++)
        //{
         //   colors[i] = new Color(0, 0, 0);
        //}

        int _range = _pmax - _pmin + 1;
        for (int i = 0, ci = 0; i < vertices.Length; i +=2, ci++)
        {
            numberList[i] = numberList[i+1] = _pmin + ci * ((_range - 1)/(colors.Length * 0.5f - 1));
            print(numberList[i]);
        }

        //单元值转换成顶点颜色
        //将_data映射到不同的色彩区间中
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList[i];
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

        mr.material = new Material(Shader.Find("Custom/Shader"));

    }
}
