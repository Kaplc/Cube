using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : BasePanel<ShopPanel>
{
    public UIButton btnRight;
    public UIButton btnLeft;

    protected override void Init()
    {
        btnRight.onClick.Add(new EventDelegate(() =>
        {
            ShopMap.Instance.RightCube();
        }));
        btnLeft.onClick.Add(new EventDelegate(() =>
        {
            ShopMap.Instance.LeftCube();
        }));
        Hide();
    }
}