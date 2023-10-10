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
        //
        GameManager.Instance.isStart = false;
        // 更新面板
        UpdateScore(GameManager.Instance.score);
        UpdateGemCount(GameManager.Instance.gemCount);
        
        //保存数据
        DataManager.Instance.dataInfo.score = GameManager.Instance.score> DataManager.Instance.dataInfo.score
            ? GameManager.Instance.score
            : DataManager.Instance.dataInfo.score;
        DataManager.Instance.dataInfo.gemCount += GameManager.Instance.gemCount;
        DataManager.Instance.SaveData();
    }

    private void UpdateScore(int score)
    {
        scoreLb.text = score.ToString();
    }

    private void UpdateGemCount(int gemCount)
    {
        gemCountLb.text = gemCount.ToString();
    }
}