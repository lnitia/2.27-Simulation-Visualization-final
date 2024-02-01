using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.SampleQRCodes;
using TMPro;
using UnityEngine;

public class PositionReader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] transTexts;

    private Transform qrt;
    private GameObject merget;
    private GameObject framet;
    private GameObject wheelsett;
    public QRCodesVisualizer qRCodesVisualizer;

    void Start()
    {
        merget = GameObject.Find("mix6");
        framet = GameObject.Find("SF6");
        wheelsett = GameObject.Find("SW6");
    }

    void Update()
    {
        if (qRCodesVisualizer.qrCodePrefabTransform != null)
        {
            qrt = qRCodesVisualizer.qrCodePrefabTransform;
            transTexts[0].text = $"QR:{qrt.transform.position.ToString("F6")},{qrt.transform.eulerAngles.ToString("F6")}";
        }
        else
        {
            transTexts[0].text = "QR:";
        }
        
        if (merget != null)
        {
            transTexts[1].text = $"MERGE:{merget.transform.position.ToString("F6")},{merget.transform.eulerAngles.ToString("F6")}";
        }
        else
        {
            transTexts[1].text = "MERGE:";
        }
        
        if (framet == null)
        {
            framet = GameObject.Find("SF6");
            transTexts[2].text = "FRAME:";
        }
        else
        {
            transTexts[2].text = $"FRAME:{framet.transform.localPosition.ToString("F6")},{framet.transform.eulerAngles.ToString("F6")},{framet.transform.localScale.ToString("F6")}";
        }
        
        if (wheelsett == null)
        {
            wheelsett = GameObject.Find("SW6");
            transTexts[3].text = "WHEELSET:";
        }
        else
        {
            transTexts[3].text = $"WHEELSET:{wheelsett.transform.localPosition.ToString("F6")},{wheelsett.transform.eulerAngles.ToString("F6")},{wheelsett.transform.localScale.ToString("F6")}";
        }

    }
}
