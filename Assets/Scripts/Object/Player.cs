using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Speed;

    public Pos oldPos = new Pos();
    public Pos pos = new Pos(3, 2);
    private Vector3 mapPos;

    private Color depthColor = new Color(235 / 255f, 99 / 255f, 144 / 255f);
    private Color color = new Color(228 / 255f, 93 / 255f, 137 / 255f);

    private float cd = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 地图坐标位置赋给player
        mapPos = Map.TilePos[pos.x][pos.z].transform.position;
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
        Move();
    }

    private void Move()
    {
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
            pos.x++;
            // 奇数行z-1
            if (pos.x % 2 == 0)
            {
                pos.z--;
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
            pos.x++;
            // 奇数行z-1
            if (pos.x % 2 != 0)
            {
                pos.z++;
            }

            if (!MapLimit()) pos = oldPos;
            CreateMark();
        }

        mapPos = Map.TilePos[pos.x][pos.z].transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(mapPos.x, transform.position.y, mapPos.z), Time.deltaTime * Speed);
        // transform.position = new Vector3(mapPos.x, transform.position.y, mapPos.z);
        
    }

    private bool MapLimit()
    {
        // 奇数行的0和 5不能
        if (pos.x % 2 != 0 && pos.z == 0 || pos.z == 5)
        {
            return false;
        }

        return true;
    }

    private void CreateMark()
    {
        // 获取底部砖块
        GameObject tile = Map.TilePos[oldPos.x][oldPos.z].transform.Find("normal_a2").gameObject;

        if (pos.x % 2 == 0)
        {
            // 偶数行深色
            tile.GetComponent<MeshRenderer>().material.color = depthColor;
        }
        else
        {
            tile.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}