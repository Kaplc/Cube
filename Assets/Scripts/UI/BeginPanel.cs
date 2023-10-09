using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel<BeginPanel>
{
    public UIButton btnStart;
    public UIButton btnShop;
    public UILabel lbScore;
    public UILabel lbGem;

    protected override void Init()
    {
        btnStart.onClick.Add(new EventDelegate(() =>
        {
            GamePanel.Instance.Show();
            Map.Instance.StartTileFallDown(); // 方块开始掉落
            CameraFollow.Instance.follow = true; // 摄像机开始跟随
            Player.Instance.isOver = false;
            Hide();
        }));
        
        btnShop.onClick.Add(new EventDelegate(() =>
        {
            Map.Instance.gameObject.SetActive(false);
            ShopMap.Instance.CreateMap();
            ShopPanel.Instance.Show();
            Player.Instance.gameObject.SetActive(false);
            Hide();
        }));
        
        Player.Instance.isOver = true;
        // 初始化数据
        UpdateData();
    }

    public void UpdateScore(int score)
    {
        lbScore.text = score.ToString();
    }

    public void UpdateGemCount(int gemCount)
    {
        lbGem.text = gemCount.ToString();
    }

    public void UpdateData()
    {
        UpdateScore(DataManager.Instance.dataInfo.score);
        UpdateGemCount(DataManager.Instance.dataInfo.gemCount);
    }
}