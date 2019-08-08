using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板脚本基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    public virtual void OnEnter()
    {
        Debug.Log(gameObject.name + " Enter");
    }

    public virtual void OnExit()
    {
        Debug.Log(gameObject.name + " Exit");
    }

    public virtual void OnPause()
    {
        Debug.Log(gameObject.name + " Pause");
    }

    public virtual void OnResume()
    {
        Debug.Log(gameObject.name + " Resume");
    }

    // 播放点击声音
    protected void PlayClickAudio()
    {
        Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.BUTTON_CLICK);
    }
}
