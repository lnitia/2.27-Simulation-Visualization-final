using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    [SerializeField] private Transform IW;
    [SerializeField] private Transform IF;
    [SerializeField] private Transform FW;
    [SerializeField] private Transform FF;

    private Vector3 Positionw;
    private Quaternion Rotationw;
    private Vector3 Scalew;
    
    private Vector3 Positionf;
    private Quaternion Rotationf;
    private Vector3 Scalef;
    
    private void Update()
    {
        var transw = IW;
        Positionw = transw.position;
        Rotationw = transw.rotation;
        Scalew = transw.localScale;
        
        var transf = IF;
        Positionf = transf.position;
        Rotationf = transf.rotation;
        Scalef = transf.localScale;
    }

    /// <summary>
    /// 把iw的位置赋给fw，if的位置赋给ff
    /// </summary>
    public void setPosition()
    {
        FW.position = Positionw;
        FW.rotation = Rotationw;
        FW.localScale = Scalew;
        FF.position = Positionf;
        FF.rotation = Rotationf;
        FF.localScale = Scalef;
    }
}
