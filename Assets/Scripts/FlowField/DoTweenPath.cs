using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenPath : MonoBehaviour
{
    // [SerializeField] private  Color c1 = Color.yellow;
    // [SerializeField] private  Color c2 = Color.red;
    // [SerializeField] private  Color c3 = Color.blue;
    // [SerializeField] private int pointLength = 20;
    // [SerializeField] private float scale = 0.01f;
    [SerializeField] private float duration = 6;

    private Vector3[] points;
    [SerializeField] private Vector3[] spoints;
    private Tween tween;
    private Tween colorTween;
    private Material material;
    [SerializeField] private Gradient gradient;
    private float alpha = 1.0f;
    private bool isPlayed = false;

    void Start()
    {
        GameObject line = transform.parent.gameObject;
        DrawLine drawLine = line.GetComponent<DrawLine>();
        points = drawLine.points;
        spoints = new Vector3[points.Length];
        gradient = drawLine.gradient;

        material = gameObject.GetComponent<MeshRenderer>().material;
        material.color = gradient.Evaluate(0.0f);
        
        Vector3 offset = gameObject.transform.localPosition + transform.parent.position;
        for (int i = 0; i < points.Length; i++)
        {
            spoints[i] = points[i]; //+ offset;
        }
        SetTween();
    }

    private void SetTween()
    {
        tween = transform.DOLocalPath(spoints, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        tween.Pause();

        colorTween = material.DOGradientColor(gradient, "_Color", duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        colorTween.Pause();
    }

    public void DoPlay()
    {
        colorTween.Play();
        tween.Play();
    }
    
    public void DoPause()
    {
        tween.Pause();
        colorTween.Pause();
    }

    public void DORestart()
    {
        tween.Restart();
        colorTween.Restart();
        tween.Pause();
        colorTween.Pause();
    }
}
