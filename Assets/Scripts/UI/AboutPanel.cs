using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanel : BasePanel<AboutPanel>
{
    public UIButton btnClose;
    
    protected override void Init()
    {
        btnClose.onClick.Add(new EventDelegate(() =>
        {
            Hide();
        }));
        
        Hide();
    }
}
