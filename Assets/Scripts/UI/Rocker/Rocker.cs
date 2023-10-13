using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocker : MonoBehaviour
{
    public Transform rockerBg;
    public Camera uiCamera;
    public Vector3 center;
    public UISprite spire;
    public UISprite rockerBgSprite;
    
    private void Start()
    {
        center = transform.position;
        spire = gameObject.GetComponent<UISprite>();
        rockerBgSprite = rockerBg.gameObject.GetComponent<UISprite>();
        
        spire.color = new Color(spire.color.r, spire.color.g, spire.color.b, 0.05f);
        rockerBgSprite.color = new Color(rockerBgSprite.color.r, rockerBgSprite.color.g, rockerBgSprite.color.b, 0.05f);
    }

    private void Update()
    {
        // 鼠标非按下就跟随
        if (!Input.GetMouseButton(0))
        {
            if (Input.mousePosition.y < Screen.height*3/4f)
            {
                rockerBg.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
                center = transform.position;  
            }
        }
        
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
        // 拖动才提高透明度
        spire.color = new Color(spire.color.r, spire.color.g, spire.color.b, 1f);
        rockerBgSprite.color = new Color(rockerBgSprite.color.r, rockerBgSprite.color.g, rockerBgSprite.color.b, 0.3f);
    }

    public void OnDragEnd()
    {
        transform.localPosition = Vector3.zero;
        // 拖动才提高透明度
        spire.color = new Color(spire.color.r, spire.color.g, spire.color.b, 0.05f);
        rockerBgSprite.color = new Color(rockerBgSprite.color.r, rockerBgSprite.color.g, rockerBgSprite.color.b, 0.05f);
    }
}
