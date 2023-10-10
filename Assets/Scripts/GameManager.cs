using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public Map map;
    public ShopMap shopMap;
    public Player player;
    
    public int nowCubeID;
    public bool isStart;
    
    public int gemCount;
    public int score;
    
    private void Awake()
    {
        instance = this;
        if (!map)
        {
            map = new GameObject("Map").AddComponent<Map>();
            map.transform.SetParent(transform);
        }
        if (!shopMap)
        {
            shopMap = new GameObject("ShopMap").AddComponent<ShopMap>();
            shopMap.transform.SetParent(transform);
        }

        nowCubeID = DataManager.Instance.dataInfo.lastChooseCubeID;
        
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        player = Instantiate(Resources.Load<GameObject>(DataManager.Instance.shopData.cubeInfos[nowCubeID].path)).AddComponent<Player>();
        Camera.main.GetComponent<CameraFollow>().player = player;
    }
    
    public void CreateMap()
    {
        if (!map)
        {
            map = new GameObject("Map").AddComponent<Map>();
            map.transform.SetParent(transform);
        }

        if (shopMap)
        {
            shopMap.DestroyMap();
        }
    }

    public void CreateShopMap()
    {
        if (!shopMap)
        {
            shopMap = new GameObject("ShopMap").AddComponent<ShopMap>();
            shopMap.transform.SetParent(transform);
            
        }
        shopMap.CreateMap();
        if (map)
        {
            map.DestroyMap();
        }
    }

    public void AddGameGem()
    {
        gemCount++;
        // 更新到面板
        GamePanel.Instance.UpdateGemCount(gemCount);
    }

    public void AddGameScore()
    {
        score++;
        GamePanel.Instance.UpdateScore(score);
    }
}
