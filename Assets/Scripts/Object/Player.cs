﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public int speed=50;

    public Pos oldPos;
    public Pos pos = new Pos(6, 2);
    private Vector3 mapPos;

    private Color depthColor = new Color(235 / 255f, 99 / 255f, 144 / 255f);
    private Color color = new Color(228 / 255f, 93 / 255f, 137 / 255f);

    private float cd; // 按键移动cd

    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // 地图坐标位置赋给player
        mapPos = GameManager.Instance.map.tilePos[pos.z][pos.x].transform.position;
        transform.position = new Vector3(mapPos.x, 0.254f / 2, mapPos.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));

        playerRigidbody = gameObject.AddComponent<Rigidbody>();
        playerRigidbody.useGravity = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isStart) return;
        
        // 玩家掉落
        if (GameManager.Instance.map.zIndex >= pos.z + 1)
        {
            gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(1f, 30f), Random.Range(1f, 30f), Random.Range(1f, 30f)) * 20);
            playerRigidbody.useGravity = true;
            Destroy(gameObject, 3);
            Dead();
        }

        Move();
        

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 碰到陷阱死亡
        if (other.CompareTag("Gem"))
        {
            // 碰到奖励加分
            GameManager.Instance.AddGameGem();
            PoolManager.Instance.PushObject(other.gameObject);
            return;
        }

        Dead();

    }

    #region 非系统代码
    
    
    public void Dead()
    {
        GameManager.Instance.isStart = false;
        // 摄像机停止跟随
        CameraFollow.Instance.StopFollow();
        // 停止下落
        GameManager.Instance.map.StopTileFallDown();
        // 结束界面渐变
        GamePanel.Instance.StartEndFade();
        // 隐藏摇杆
        GamePanel.Instance.HideRocker();
    }

    private void Move()
    {
        if (!GameManager.Instance.isStart)
        {
            return;
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
            
            // 增加分数
            GameManager.Instance.AddGameScore();
        }
        else
        {
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
                GameManager.Instance.AddGameScore();
            } 
        }
        
        mapPos = GameManager.Instance.map.tilePos[pos.z][pos.x].transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(mapPos.x, transform.position.y, mapPos.z), Time.deltaTime * speed);
        // 空洞掉落
        if (GameManager.Instance.map.tilePos[pos.z][pos.x].CompareTag("Untagged"))
        {
            transform.position = GameManager.Instance.map.tilePos[pos.z][pos.x].transform.position;
            playerRigidbody.useGravity = true;
            Dead();
        }
        // transform.position = new Vector3(mapPos.x, transform.position.y, mapPos.z);
    }

    public void MoveRight()
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
        GameManager.Instance.AddGameScore();
    }
    
    public void MoveLeft()
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
            
        // 增加分数
        GameManager.Instance.AddGameScore();
    }
    /// <summary>
    /// 地图边界
    /// </summary>
    /// <returns></returns>
    private bool MapLimit()
    {
        // 奇数行的0和 5不能
        if (pos.z % 2 != 0 && pos.x == 0 || pos.x == 5)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 生成轨迹
    /// </summary>
    private void CreateMark()
    {
        GameObject tile = GameManager.Instance.map.tilePos[oldPos.z][oldPos.x];
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

    #endregion
}