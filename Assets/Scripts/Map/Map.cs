using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Pos
{
    public int x ;
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
    public List<GameObject[]> tilePos = new List<GameObject[]>();

    // 当前掉落行号
    public int zIndex;
    private Coroutine startTileFallDown;

    // 砖块类型概率
    private int prNull;
    private int prGroundSpike ;
    private int prSkySpike;

    // 奖励生成概率
    private int prGem = 1;

    void Awake()
    {
        CreatMapItem(0);
        prNull = 2;
        prGroundSpike = 1;
        prSkySpike = 1;
    }

    private void Update()
    {
        NewMap();
    }

    public void StartTileFallDown()
    {
        startTileFallDown = StartCoroutine(TileFallDown());
    }

    public void StopTileFallDown()
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
            for (int x = 0; x < tilePos[zIndex].Length; x++)
            {
                GameObject tile = tilePos[zIndex][x];
                // 当前砖块为陷阱时
                if (tile.CompareTag("GroundSpike") || tile.CompareTag("SkySpike"))
                {
                    tile.GetComponent<Spike>().FallDown();
                }
                else
                {
                    // 普通砖块添加刚体受重力下落, 并随机旋转
                    tile.GetComponent<Tile>()?.FallDown();
                }
                
                // 列表内该位置置空
                tilePos[zIndex][x] = null;
            }

            zIndex++;

            // 按照砖块掉落进程提高陷阱概率
            if (zIndex == 200 || zIndex == 350 || zIndex == 450 || zIndex == 500)
            {
                prNull++;
                prGroundSpike++;
                prSkySpike++;
            }

            yield return new WaitForSeconds(1f);
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
        if ((tilePos.Count - 1) - GameManager.Instance.player.pos.z < 15)
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
                    // 创建地面陷阱
                    tile = CreateGroundSpikes();
                }
                else if (pr > prGroundSpike && pr <= prNull + prGroundSpike + prSkySpike)
                {
                    // 创建空中陷阱
                    tile = CreateSkySpikes();
                }
                else
                {
                    // 创建普通方块
                    tile = CreateNormalTile(depthColor);
                    // 随机概率在普通方块上生成奖励
                    int nowPrGem = Random.Range(1, 101);
                    if (nowPrGem <= prGem)
                    {
                        GameObject gem = PoolManager.Instance.GetObject("gem/gem");
                        if (gem)
                        {
                            gem.transform.SetParent(tile.transform);
                            gem.transform.position = tile.transform.position + new Vector3(0, 0.1f, 0);
                            gem.transform.rotation = tile.transform.rotation;
                        }
                        else
                        {
                            gem = Instantiate(Resources.Load<GameObject>("gem/gem"), tile.transform.position + new Vector3(0, 0.1f, 0),
                                tile.transform.rotation, tile.transform);
                        }
                    }
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
                    tile = PoolManager.Instance.GetObject("Tile/Wall");
                    tile.transform.SetParent(transform);
                    tile.transform.localPosition = Vector3.zero;
                    tile.transform.rotation = Quaternion.Euler(-90, 45, 0);
                    tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal + offSetZ);
                    tile.GetComponent<Tile>().ChangeColor(wallColor);
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
                        tile = CreateGroundSpikes();
                    }
                    else if (pr > prGroundSpike && pr <= prNull + prGroundSpike + prSkySpike)
                    {
                        // 生成天空陷阱
                        tile = CreateSkySpikes();
                    }
                    else
                    {
                        // 生成普通地砖
                        tile = CreateNormalTile(color);
                        
                        int nowPrGem = Random.Range(1, 101);
                        if (nowPrGem <= prGem)
                        {
                            GameObject gem = PoolManager.Instance.GetObject("gem/gem");
                            if (gem)
                            {
                                gem.transform.SetParent(tile.transform);
                                gem.transform.position = tile.transform.position + new Vector3(0, 0.1f, 0);
                                gem.transform.rotation = tile.transform.rotation;
                            }
                            else
                            {
                                Instantiate(Resources.Load<GameObject>("gem/gem"), tile.transform.position + new Vector3(0, 0.1f, 0),
                                    tile.transform.rotation, tile.transform);
                            }
                        }
                    }

                    // 先偏移0.175f,每个方块偏移0.35
                    tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal + offSetZ);
                }

                evenTiles[x] = tile;
            }

            tilePos.Add(evenTiles);
        }

        // 重命名
        // ReName();
    }

    private GameObject CreateNormalTile(Color tileColor)
    {
        GameObject normalTile = PoolManager.Instance.GetObject("Tile/Tile");

        normalTile.transform.SetParent(transform);
        normalTile.transform.localPosition = Vector3.zero;
        normalTile.transform.rotation = Quaternion.Euler(-90, 45, 0);
        normalTile.GetComponent<Tile>().ChangeColor(tileColor);
        return normalTile;
    }

    private GameObject CreateSkySpikes()
    {
        GameObject skySpikes = PoolManager.Instance.GetObject("Spikes/SkySpikes");

        skySpikes.transform.SetParent(transform);
        skySpikes.transform.localPosition = Vector3.zero;
        skySpikes.transform.rotation = Quaternion.Euler(-90, 45, 0);

        return skySpikes;
    }

    private GameObject CreateGroundSpikes()
    {
        GameObject groundSpikes = PoolManager.Instance.GetObject("Spikes/GroundSpikes");

        groundSpikes.transform.SetParent(transform);
        groundSpikes.transform.localPosition = Vector3.zero;
        groundSpikes.transform.rotation = Quaternion.Euler(-90, 45, 0);
        
        return groundSpikes;
    }

    public void DestroyMap()
    {
        GameManager.Instance.map = null;
        Destroy(gameObject);
    }
}