using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : BasePanel<ShopPanel>
{
    public UIButton btnRight;
    public UIButton btnLeft;
    public UIButton btnChoose;
    public UIButton btnUnlock;
    public UIButton btnBack;
    public UILabel lbUnlockGemCount;
    public UILabel lbGemCount;
    
    protected override void Init()
    {
        btnRight.onClick.Add(new EventDelegate(() =>
        {
            ShopMap.Instance.RightCube();
        }));
        
        btnLeft.onClick.Add(new EventDelegate(() =>
        {
            ShopMap.Instance.LeftCube();
        }));
        
        btnChoose.onClick.Add(new EventDelegate(() =>
        {
            GameManager.Instance.CreatePlayer();
            GameManager.Instance.CreateMap();
            BeginPanel.Instance.Show();
            Hide();
        }));
        
        btnUnlock.onClick.Add(new EventDelegate(() =>
        {
            if (DataManager.Instance.dataInfo.gemCount >= DataManager.Instance.shopData.cubeInfos[GameManager.Instance.nowCubeID].unlockGemCount)
            {
                // 减少gem
                DataManager.Instance.SubGemCount(DataManager.Instance.shopData.cubeInfos[GameManager.Instance.nowCubeID].unlockGemCount);
                // 更新面板
                ChangeGemCount(DataManager.Instance.dataInfo.gemCount);
                // 添加
                DataManager.Instance.dataInfo.unlockCube.Add(GameManager.Instance.nowCubeID);
                // 保存
                DataManager.Instance.SaveData();
                // 购买成功后检测是否解锁更新按钮
                GameManager.Instance.shopMap.CheckUnlock();
            }
        }));
        
        btnBack.onClick.Add(new EventDelegate(() =>
        {
            BeginPanel.Instance.Show();
            GameManager.Instance.CreateMap();
            GameManager.Instance.player.gameObject.SetActive(true);
            Hide();
        }));
        Hide();
    }
    
    public void ChangeUnlockBtnGemCount(int count)
    {
        lbUnlockGemCount.text = count.ToString();
    }
    
    public void ChangeGemCount(int count)
    {
        lbGemCount.text = count.ToString();
    }
    
    public override void Show()
    {
        base.Show();
        ChangeGemCount(DataManager.Instance.dataInfo.gemCount);
    }
}