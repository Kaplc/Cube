using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel<BeginPanel>
{
    public UIButton startBtn;
    public UILabel scoreLb;
    public UILabel gemLb;

    protected override void Init()
    {
        startBtn.onClick.Add(new EventDelegate(() =>
        {
            GamePanel.Instance.Show();
            Map.Instance.StartTileFallDown(); // 方块开始掉落
            CameraFollow.Instance.follow = true; // 摄像机开始跟随
            Player.Instance.isOver = false;
            Hide();
        }));
        Player.Instance.isOver = true;
        // 初始化数据
        UpdateData();
    }

    public void UpdateScore(int score)
    {
        scoreLb.text = score.ToString();
    }

    public void UpdateGemCount(int gemCount)
    {
        gemLb.text = gemCount.ToString();
    }

    public void UpdateData()
    {
        UpdateScore(DataManager.Instance.dataInfo.score);
        UpdateGemCount(DataManager.Instance.dataInfo.gemCount);
    }
}