using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance => instance;
    private static CameraFollow instance;
    
    public Player player;
    public bool follow;

    public float distance;

    public void StopFollow()
    {
        follow = false;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            follow = true;
        }
        
        if (follow)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance), Time.deltaTime);
        }
    }
    
}