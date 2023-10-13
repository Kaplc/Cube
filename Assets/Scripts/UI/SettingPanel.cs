using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel<SettingPanel>
{
    public UIButton btnBack;
    public UIToggle tgMusicMute;
    public UISlider sldMusicVolume;
    public UIButton btnClose;

    protected override void Init()
    {
        btnBack.onClick.Add(new EventDelegate(() =>
        {
            Hide();
        }));
        
        tgMusicMute.onChange.Add(new EventDelegate(() =>
        {
            GameManager.Instance.bgm.Mute(!tgMusicMute.value);
            DataManager.Instance.musicData.musicMute = !tgMusicMute.value;
        }));
        
        sldMusicVolume.onChange.Add(new EventDelegate(() =>
        {
            GameManager.Instance.bgm.SetVolume(sldMusicVolume.value);
            DataManager.Instance.musicData.musicVolume = sldMusicVolume.value;
        }));
        btnClose.onClick.Add(new EventDelegate(() =>
        {
           DataManager.Instance.SaveData();
        }));
        Hide();
    }
    
}
