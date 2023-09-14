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
