 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockerArea : MonoBehaviour
{
    public Rocker rocker;
    public Transform rockerBg;
    public Camera uiCamera;

    public void OnPress()
    {
        rockerBg.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
        rocker.transform.position = uiCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
