using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;

    public DataInfo dataInfo;
    public ShopData shopData;

    private DataManager()
    {
        // 初始化数据
        dataInfo = XmlDataManager.Instance.Load(typeof(DataInfo),"data") as DataInfo;
        shopData = XmlDataManager.Instance.Load(typeof(ShopData), "ShopData") as ShopData;
    }

    public void SaveData()
    {
        XmlDataManager.Instance.Save(dataInfo, "data");
    }

    public void SubGemCount(int count)
    {
        dataInfo.gemCount -= count;
    }
}
