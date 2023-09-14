using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanel : BasePanel<GamePanel>
{
    public UIButton suspendBtn;
    public UILabel scoreLb;
    public UILabel gemLb;
    public UITexture backGround;

    // 结束时背景渐变动画
    public Coroutine fadeCoroutine;

    // Update is called once per frame
    protected override void Init()
    {
        suspendBtn.onClick.Add(new EventDelegate(() =>
        {
            SuspendPanel.Instance.Show();
            Time.timeScale = 0;
        }));

        Hide();
    }
    

    public void UpdateScore(int score)
    {
        scoreLb.text = score.ToString();
    }

    public void UpdateGemCount(int gemCount)
    {
        gemLb.text = gemCount.ToString();
    }

    public void StartEndFade()
    {
        fadeCoroutine = StartCoroutine(EndFade());
    }

    // public void StopEndFade()
    // {
    //     StopCoroutine(fadeCoroutine);
    // }

    public IEnumerator EndFade()
    {
        while (true)
        {
            if (backGround.color.a >= 0.99f)
            {
                // 渐变结束进入结束场景
                SceneManager.LoadScene("EndScene");
                break;
            }

            float alpha = backGround.color.a + 0.1f;
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, alpha);
            yield return new WaitForSeconds(0.1f);
        }
    }
}