using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspendPanel : BasePanel<SuspendPanel>
{
    public UIButton continueBtn;
    
    protected override void Init()
    {
        continueBtn.onClick.Add(new EventDelegate(() =>
        {
            Time.timeScale = 1;
            Hide();
        }));
        
        Hide();
    }
}
