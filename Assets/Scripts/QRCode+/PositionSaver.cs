using UnityEngine;
using System.IO;
using System.Text;

public class PositionSaver : MonoBehaviour
{
    private string path;
    private string transText;

    private GameObject merge;
    private Transform frameTransform;
    private Transform wheelsetTransform;
    void Start()
    {
        merge = GameObject.Find("mix6");
        frameTransform = merge.transform.Find("SF6");
        wheelsetTransform = merge.transform.Find("SW6");

        path = Application.persistentDataPath + @"\MyPosition.txt";
        if (!File.Exists(path))
        {
            string createText = "0";
            File.WriteAllText(path, createText, Encoding.UTF8);
        }
        else
        {
            SetTrans();
        }
    }

    public void SavePosition()
    {
        string framePosition = frameTransform.localPosition.ToString("F6");
        string frameRotation = frameTransform.eulerAngles.ToString("F6");
        string frameScale = frameTransform.localScale.ToString("F6");

        string wheelsetPosition = wheelsetTransform.localPosition.ToString("F6");
        string wheelsetRotation = wheelsetTransform.eulerAngles.ToString("F6");
        string wheelsetScale = wheelsetTransform.localScale.ToString("F6");
        
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(framePosition);
            writer.WriteLine(frameRotation);
            writer.WriteLine(frameScale);

            writer.WriteLine(wheelsetPosition);
            writer.WriteLine(wheelsetRotation);
            writer.WriteLine(wheelsetScale);
        }
    }

    public void SetTrans()
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string framePosition = reader.ReadLine();
            string frameRotation = reader.ReadLine();
            string frameScale = reader.ReadLine();
            
            string wheelsetPosition = reader.ReadLine();
            string wheelsetRotation = reader.ReadLine();
            string wheelsetScale = reader.ReadLine();

            if (frameRotation != null)
            {
                SetTransformValues(frameTransform, framePosition,frameRotation, frameScale);
                SetTransformValues(wheelsetTransform, wheelsetPosition,wheelsetRotation, wheelsetScale); 
            }
        }
    }
    
    private Vector3 StringToVector3(string str)
    {
        str = str.Replace("(", " ").Replace(")", " ");
        string[] split = str.Split(',');
        if (split.Length == 3)
        {
            float x, y, z;
            if (float.TryParse(split[0], out x) && float.TryParse(split[1], out y) && float.TryParse(split[2], out z))
            {
                return new Vector3(x, y, z);
            }
        }
        return Vector3.zero;
    }
    
    private void SetTransformValues(Transform target, string position,string rotation, string scale)
    {
        Vector3 pos = StringToVector3(position);
        Vector3 rot = StringToVector3(rotation);
        Vector3 sca = StringToVector3(scale);

        target.localPosition = pos;
        target.eulerAngles = rot;
        target.localScale = sca;
    }

}

