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
            GameManager.Instance.map.StartTileFallDown(); // 方块开始掉落
            CameraFollow.Instance.follow = true; // 摄像机开始跟随
            GameManager.Instance.isStart = true;
            Hide();
        }));
        
        btnShop.onClick.Add(new EventDelegate(() =>
        {
            ShopPanel.Instance.Show();
            GameManager.Instance.player.gameObject.SetActive(false);
            GameManager.Instance.CreateShopMap();
            Hide();
        }));
        
        // 初始化数据
        UpdateData();
    }

    private void UpdateScore(int score)
    {
        lbScore.text = score.ToString();
    }

    private void UpdateGemCount(int gemCount)
    {
        lbGem.text = gemCount.ToString();
    }

    private void UpdateData()
    {
        UpdateScore(DataManager.Instance.dataInfo.score);
        UpdateGemCount(DataManager.Instance.dataInfo.gemCount);
    }
}