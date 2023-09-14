using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel<BeginPanel>
{
    public UIButton startBtn;
    
    
    protected override void Init()
    {
        startBtn.onClick.Add(new EventDelegate(() =>
        {
            GamePanel.Instance.Show();
            Map.Instance.StartTileFallDown(); // 方块开始掉落
            CameraFollow.Instance.follow = true; // 摄像机开始跟随
            Hide();
        }));
    }
}
