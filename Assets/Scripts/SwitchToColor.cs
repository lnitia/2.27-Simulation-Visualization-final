using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToColor : MonoBehaviour
{
    /// <summary>
    /// 结果数据转换为颜色值
    /// </summary>
    /// <param name="numberList3"></param>
    /// <param name="colorLength"></param>
    /// <param name="maxvalue"></param>
    /// <param name="minvalue"></param>
    /// <param name="mode"></param> 1：红色最大值渐变（强度、流固、变形） 2：蓝色最大值渐变（疲劳寿命）
    /// <returns></returns>
    public Color[] Switch(List<float> numberList3, int colorLength, float maxvalue, float minvalue, int mode)
    {
        Color[] colors = new Color[colorLength];
        //单元值转换成顶点颜色
        float _range = maxvalue - minvalue + 0.00001f;
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
            if (mode == 1)
            {
                if (_data < minvalue)
                    colors[i] = new Color(0, 0, 0);
                if (_data > maxvalue)
                    colors[i] = new Color(1, 1, 1);
                if (idx == 0)
                    colors[i] = new Color(0, local_r, 1);
                if (idx == 1)
                    colors[i] = new Color(0, 1, 1 - local_r);
                if (idx == 2)
                    colors[i] = new Color(local_r, 1, 0);
                if (idx == 3)
                    colors[i] = new Color(1, 1 - local_r, 0);
            }
            else if (mode == 2)
            {
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
            
        }
        return colors;
    }
    /*原始
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i];
            float r = (_data - minvalue) / _range;
            int step = _range / 5;
            int idx = (int)(r * 5.0);
            int h = (idx + 1) * step + minvalue;
            int m = idx * step + minvalue;
            float local_r = (_data - m) / (h - m);
            if (_data < minvalue)
                colors[i] = new Color(0, 0, 0);
            if (_data > maxvalue)
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
        }*/
    
    /*红色最大渐变
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i];
            float r = (_data - minvalue) / _range;
            float step = _range / 4;
            int idx = (int)(r * 4);
            float h = (idx + 1) * step + minvalue;
            float m = idx * step + minvalue;
            float local_r = (_data - m) / (h - m);
            if (_data < minvalue)
                colors[i] = new Color(0, 0, 0);
            if (_data > maxvalue)
                colors[i] = new Color(0.4f, 0.4f, 0.4f);
            if (idx == 0)
                colors[i] = new Color(0, 0.4f * local_r, 0.4f);
            if (idx == 1)
                colors[i] = new Color(0, 0.4f, 0.4f * (1 - local_r));
            if (idx == 2)
                colors[i] = new Color(0.4f * local_r, 0.4f, 0);
            if (idx == 3)
                colors[i] = new Color(0.4f, 0.4f * (1 - local_r), 0);
           // print(i + " : " + colors[i] + _data + "  " + h + "  " + m);
        }*/
}
