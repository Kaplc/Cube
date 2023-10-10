using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocker : MonoBehaviour
{
    public Camera uiCamera;
    public Vector3 center;

    private void Start()
    {
        center = transform.position;
    }

    private void Update()
    {
        if (transform.localPosition.x > 0)
        {
            // 向右
            GameManager.Instance.player.MoveRight();
        }
        else if (transform.localPosition.x < 0)
        {
            GameManager.Instance.player.MoveLeft();
        }
    }

    public void OnDrag(Vector2 delta)
    {
        transform.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Vector3.Distance(transform.position ,center) > 0.25f)
        {
            transform.position = center + (transform.position - center).normalized * 0.25f;
        }
    }

    public void OnDragEnd()
    {
        transform.localPosition = Vector3.zero;
    }
}
