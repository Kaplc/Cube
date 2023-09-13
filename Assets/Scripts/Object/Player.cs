using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance => instance;

    public int speed;

    public Pos oldPos;
    public Pos pos = new Pos(6, 2);
    private Vector3 mapPos;

    private Color depthColor = new Color(235 / 255f, 99 / 255f, 144 / 255f);
    private Color color = new Color(228 / 255f, 93 / 255f, 137 / 255f);

    private float cd = 0; // 按键移动cd
    public static bool isOver = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 地图坐标位置赋给player
        mapPos = Map.TilePos[pos.z][pos.x].transform.position;
        transform.position = new Vector3(mapPos.x, 0.254f / 2, mapPos.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
    }

    private void ResetPos()
    {
        pos.x = 3;
        pos.z = 2;
        mapPos = Map.TilePos[3][2].transform.position;
        transform.position = new Vector3(mapPos.x, 0.254f / 2, mapPos.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
            Dead();
        }

        Move();
    }

    private void Dead()
    {
        // print($"map{Map.Zindex} z{pos.z}");
        if (Map.Zindex >= pos.z + 1)
        {
            isOver = true;
            gameObject.AddComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(1f, 30f), Random.Range(1f, 30f), Random.Range(1f, 30f)) * 20);
            ;
            Destroy(gameObject, 3);
            CameraFollow.StartOrStopFollow();
        }
    }

    private void Move()
    {
        if (isOver)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ResetPos();
        }

        if (Input.GetKey(KeyCode.A))
        {
            cd += Time.deltaTime;
            if (cd <= 0.1f) return;
            cd = 0;

            oldPos = pos;
            // 向左
            pos.z++;
            // 奇数行z-1
            if (pos.z % 2 == 0)
            {
                pos.x--;
            }

            // 边界判断
            if (!MapLimit()) pos = oldPos;
            // 每次移动创建轨迹
            CreateMark();
        }

        if (Input.GetKey(KeyCode.D))
        {
            cd += Time.deltaTime;
            if (cd <= 0.1f) return;
            cd = 0;

            oldPos = pos;
            // 向右
            pos.z++;
            // 奇数行z-1
            if (pos.z % 2 != 0)
            {
                pos.x++;
            }

            if (!MapLimit()) pos = oldPos;
            CreateMark();
        }

        mapPos = Map.TilePos[pos.z][pos.x].transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(mapPos.x, transform.position.y, mapPos.z), Time.deltaTime * speed);
        // transform.position = new Vector3(mapPos.x, transform.position.y, mapPos.z);
    }

    private bool MapLimit()
    {
        // 奇数行的0和 5不能
        if (pos.z % 2 != 0 && pos.x == 0 || pos.x == 5)
        {
            return false;
        }

        return true;
    }

    private void CreateMark()
    {
        GameObject tile = Map.TilePos[oldPos.z][oldPos.x];
        GameObject ground;
        // 获取底部砖块
        if (tile.CompareTag("GroundSpike"))
        {
          ground = tile.transform.Find("moving_spikes_a2").gameObject;
        }
        else if (tile.CompareTag("SkySpike"))
        {
            ground = tile.transform.Find("smashing_spikes_a2").gameObject;
        }
        else if (tile.CompareTag("Tile"))
        { 
            ground = tile.transform.Find("normal_a2").gameObject;
        }
        else
        {
            return;
        }
        // 偶数行深色
        ground.GetComponent<MeshRenderer>().material.color = pos.z % 2 == 0 ? depthColor : color;
        
    }
}