using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public enum E_TileType
{
    Normal,
    Null,
    GroundSpike,
    SkySpike
}

public struct Pos
{
    public int x;
    public int z;

    public Pos(int z, int x)
    {
        this.x = x;
        this.z = z;
    }
}

public class Map : MonoBehaviour
{
    // 对角线长度
    private float diagonal = 0.35921f;

    // 砖块深浅颜色
    public Color color = new Color(103 / 255f, 85 / 255f, 127 / 255f);
    public Color depthColor = new Color(90 / 255f, 77 / 255f, 115 / 255f);
    public Color wallColor = new Color(82 / 255f, 59 / 255f, 102 / 255f);

    // 砖块坐标数据集合
    public static List<GameObject[]> TilePos => tilePos;
    private static List<GameObject[]> tilePos = new List<GameObject[]>();

    // 当前掉落行号
    public static int Zindex = 0;
    private Coroutine startTileFallDown;

    // 砖块类型概率
    private int prNull = 0;
    private int prGroundSpike = 0;                
    private int prSkySpike = 0;

    void Awake()
    {
        CreatMapItem(0);
        prNull += 2;
        prGroundSpike += 1;
        prSkySpike += 1;
    }

    private void Update()
    {
        NewMap();
        if (Input.GetKeyUp(KeyCode.O))
        {
            StartTileFallDown();
        }

        if (Player.isOver)
        {
            StopTileFallDown();
        }
    }

    private void StartTileFallDown()
    {
        startTileFallDown = StartCoroutine(TileFallDown());
    }

    private void StopTileFallDown()
    {
        StopCoroutine(startTileFallDown);
    }

    /// <summary>
    /// 协程控制砖块掉落
    /// </summary>
    /// <returns></returns>
    private IEnumerator TileFallDown()
    {
        while (true)
        {
            for (int x = 0; x < tilePos[Zindex].Length; x++)
            {
                // 添加刚体受重力下落, 并随机旋转
                Rigidbody tileRigidbody = tilePos[Zindex][x].AddComponent<Rigidbody>();
                tileRigidbody.AddTorque(new Vector3(Random.Range(1f, 30f), Random.Range(1f, 30f), Random.Range(1f, 30f)) * 20);
                // 延时销毁
                if (tileRigidbody.gameObject.CompareTag("GroundSpike") ||tileRigidbody.gameObject.CompareTag("SkySpike") )
                {
                    tileRigidbody.GetComponent<Spike>().ObjDestroy();
                }
                Destroy(tileRigidbody.gameObject, 2);
                tilePos[Zindex][x] = null;
            }

            Zindex++;
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void ReName()
    {
        for (int z = 0; z < tilePos.Count; z++)
        {
            for (int x = 0; x < tilePos[z].Length; x++)
            {
                if (tilePos[z][x])
                {
                    tilePos[z][x].name = $"{z}-{x}";
                }
            }
        }
    }

    private void NewMap()
    {
        if ((tilePos.Count - 1) - Player.Instance.pos.z < 15)
        {
            // 每行偏移2倍0.35921
            CreatMapItem(diagonal * tilePos.Count / 2);
        }
    }

    private void CreatMapItem(float offSetZ)
    {
        // 10次x2=20行
        for (int z = 0; z < 6; z++)
        {
            GameObject[] OddTiles = new GameObject[5];
            // 偶数行
            for (int x = 0; x < 5; x++)
            {
                
                int pr = Random.Range(1, 101);
                GameObject tile;
           
                if (pr >= 1 && pr <= prNull)
                {
                    // 生成空洞
                    tile = new GameObject();
                    tile.transform.SetParent(transform);
                    tile.transform.rotation = Quaternion.Euler(-90, 45, 0);
                }
                else if (pr > prNull && pr <= prNull + prGroundSpike)
                {
                    // 生成地面陷阱
                    tile = Instantiate(Resources.Load<GameObject>("Spikes/GroundSpikes"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    // 添加陷阱脚本
                    tile.AddComponent<Spike>();
                }
                else if (pr > prGroundSpike && pr <= prNull + prGroundSpike + prSkySpike)
                {
                    // 生成天空陷阱
                    tile = Instantiate(Resources.Load<GameObject>("Spikes/SkySpikes"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    tile.AddComponent<Spike>();
                }
                else
                {
                    // 生成普通地砖
                    tile = Instantiate(Resources.Load<GameObject>("Tile/Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    // rgb数值/255f
                    tile.GetComponent<MeshRenderer>().material.color = depthColor;
                    tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = depthColor;
                }

                // 每个方块偏移0.35
                tile.transform.position = new Vector3(diagonal + x * diagonal, 0, z * diagonal + offSetZ);

                OddTiles[x] = tile;
            }

            tilePos.Add(OddTiles);

            // 奇数行 
            GameObject[] evenTiles = new GameObject[6];
            for (int x = 0; x < 6; x++)
            {
                GameObject tile;
                if (x == 0 || x == 5)
                {
                    // 墙
                    tile = Instantiate(Resources.Load<GameObject>("Tile/Wall"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal + offSetZ);
                    tile.GetComponent<MeshRenderer>().material.color = color;
                }
                else
                {
                    int pr = Random.Range(1, 101);
                    if (pr >= 1 && pr <= prNull)
                    {
                        // 生成空洞
                        tile = new GameObject();
                        tile.transform.SetParent(transform);
                        tile.transform.rotation = Quaternion.Euler(-90, 45, 0);
                    }
                    else if (pr > prNull && pr <= prNull + prGroundSpike)
                    {
                        // 生成地面陷阱
                        tile = Instantiate(Resources.Load<GameObject>("Spikes/GroundSpikes"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0),
                            transform);
                        tile.AddComponent<Spike>();
                    }
                    else if (pr > prGroundSpike && pr <= prNull + prGroundSpike + prSkySpike)
                    {
                        // 生成天空陷阱
                        tile = Instantiate(Resources.Load<GameObject>("Spikes/SkySpikes"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0),
                            transform);
                        tile.AddComponent<Spike>();
                    }
                    else
                    {
                        // 生成普通地砖
                        tile = Instantiate(Resources.Load<GameObject>("Tile/Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                        // rgb数值/255f
                        tile.GetComponent<MeshRenderer>().material.color = color;
                        tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = color;
                    }

                    // 先偏移0.175f,每个方块偏移0.35
                    tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal + offSetZ);
                }

                evenTiles[x] = tile;
            }

            tilePos.Add(evenTiles);
        }

        // 重命名
        ReName();
    }
}