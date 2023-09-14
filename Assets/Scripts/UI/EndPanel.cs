using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : BasePanel<EndPanel>
{
    public UIButton exitBtn;
    public UILabel scoreLb;
    public UILabel gemCountLb;

    protected override void Init()
    {
        exitBtn.onClick.Add(new EventDelegate(() =>
        {
            SceneManager.LoadScene("GameScene");
        }));
        
        // 更新面板
        UpdateScore(DataManager.Instance.newDataInfo.score);
        UpdateGemCount(DataManager.Instance.newDataInfo.gemCount);
        
        //保存数据
        DataManager.Instance.newDataInfo.score = DataManager.Instance.newDataInfo.score > DataManager.Instance.dataInfo.score
            ? DataManager.Instance.newDataInfo.score
            : DataManager.Instance.dataInfo.score;
        DataManager.Instance.SaveData();
    }

    public void UpdateScore(int score)
    {
        scoreLb.text = score.ToString();
    }

    public void UpdateGemCount(int gemCount)
    {
        gemCountLb.text = gemCount.ToString();
    }
}