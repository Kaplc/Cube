using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player player;
    public bool follow;

    public float distance;

    public void StartOrStopFollow()
    {
        follow = !follow;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartOrStopFollow();
        }
        
        if (follow)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance), Time.deltaTime);
        }
    }
    
}