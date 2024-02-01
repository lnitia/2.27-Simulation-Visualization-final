using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorValueMesh : MonoBehaviour
{
    private const float Width = 0.1f;

    //public GameObject parent;
    public int mode = 1;
    
    //private ReadFile readFile;
    private SwitchToColor switchToColor;
    [SerializeField]private float maxvalue;
    [SerializeField]private float minvalue; 
    
    private MeshFilter mf;
    
    public TextMeshPro[] text;
    private float[] textValue;

    private void Start()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        switchToColor = gameObject.AddComponent<SwitchToColor>();
        //readFile = parent.GetComponent<ReadFile>();
        //StartCoroutine(WaitNonZeroValue());
        CreateMesh();
        SetText();
    }

    // private IEnumerator WaitNonZeroValue()
    // {
    //     bool NonZeroValue()
    //     {
    //         return readFile.maxvalue != 0;
    //     }
    //     yield return new WaitUntil(NonZeroValue);
    //     maxvalue = readFile.maxvalue;
    //     minvalue = readFile.minvalue; 
    //     print( "max:" + maxvalue + "  min:" + minvalue);
    //     CreateMesh();
    //     SetText();
    // }

    private void CreateMesh()
    {
        Vector3[] vertices = new Vector3[10];

        for (int i = 0, vi = 0; i < vertices.Length; i += 2, vi += 1)
        {
            vertices[i] = new Vector3(0, (float)(Width * vi), 0);
            vertices[i + 1] = new Vector3(Width, (float)(Width * vi), 0);
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
        }

        Color[] colors = new Color[vertices.Length];
        float[] numberList = new float[vertices.Length];
        List<float> colorList = new List<float>();

        float _range = maxvalue - minvalue + 1;
        for (int i = 0, ci = 0; i < vertices.Length; i +=2, ci++)
        {
            numberList[i] = numberList[i+1] = minvalue + ci * ((_range - 1)/(colors.Length * 0.5f - 1));
            colorList.Add(numberList[i]);
            colorList.Add(numberList[i+1]);
        }

        colors = switchToColor.Switch(colorList, vertices.Length, maxvalue, minvalue, mode);
        
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.colors = colors;
        mf.mesh.RecalculateNormals();
    }

    private void SetText()
    {
        textValue = new float[text.Length]; 
        float textRange = maxvalue - minvalue;
        //textValue[0] = Mathf.RoundToInt(minvalue);
        textValue[0] = minvalue;
        for (int i = 1; i < text.Length; i++)
        {
            textValue[i] = (textRange * i) / (text.Length - 1)+ minvalue;
        }
        
        for (int i = 0; i < text.Length; i++)
        {
            text[i].rectTransform.localPosition = new Vector3(Width, (Width * 4 * i) / (text.Length - 1), 0);
            text[i].rectTransform.pivot = new Vector2(0, 0.5f);
            text[i].text = textValue[i].ToString("F2");
        }
    }
}
