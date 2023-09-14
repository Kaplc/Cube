using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GameObject gemLight;
    public GameObject cube;

    public float dir;
    public float rotationSpeed;

    // 反向间隔
    private float covTime;
    // 游戏时间
    private float time;

    // Update is called once per frame
    void Update()
    {
        cube.transform.Rotate(Vector3.up, rotationSpeed * 2 * Time.deltaTime, Space.World);
        gemLight.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        
        // 匀速上下移动
        if (dir>=0)
        {
            cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition,new Vector3(0,0,0.1f),
                Time.deltaTime);
        }
        else
        {
            cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition,new Vector3(0,0,0),
                Time.deltaTime);
            // cube.transform.position = Vector3.zero;
        }

        if (covTime <= 0)
        {
            dir = -dir;
            covTime = 0.5f;
            time = 0;
        }

        covTime -= Time.deltaTime;
        time += Time.deltaTime;
    }
}