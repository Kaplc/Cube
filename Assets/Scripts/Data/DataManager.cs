using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;

    public DataInfo dataInfo;
    public ShopData shopData;
    public DataInfo newDataInfo = new DataInfo();

    public DataManager()
    {
        // 初始化数据
        dataInfo = XmlDataManager.Instance.Load(typeof(DataInfo),"data") as DataInfo;
        shopData = XmlDataManager.Instance.Load(typeof(ShopData), "ShopData") as ShopData;
        
        // 创建动态游戏数据
        newDataInfo.score = 0;
        newDataInfo.gemCount = 0;
    }

    public void SaveData()
    {
        newDataInfo.gemCount += dataInfo.gemCount;
        XmlDataManager.Instance.Save(newDataInfo, "data");
        
        // 刷新旧数据
        dataInfo.score = newDataInfo.score;
        dataInfo.gemCount = newDataInfo.gemCount;

        newDataInfo.score = 0;
        newDataInfo.gemCount = 0;
    }
}
