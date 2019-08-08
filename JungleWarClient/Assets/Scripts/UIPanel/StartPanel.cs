using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    public override void OnEnter()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
    }
    public override void OnExit()
    {
        transform.DOScale(0, 0.4f);
    }
    public override void OnResume()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
    }
    public override void OnPause()
    {
        transform.DOScale(0, 0.4f);
    }

    private void Start()
    {
        RegisterButtonClick();
    }

    // 注册按钮点击事件
    private void RegisterButtonClick()
    {
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginButtonClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterButtonClick);
    }

    private void OnLoginButtonClick()
    {
        PlayClickAudio();
        Root.Instance.ui_manager.PushPanel(UIPanelName.LoginPanel);
    }

    private void OnRegisterButtonClick()
    {
        PlayClickAudio();
        Root.Instance.ui_manager.PushPanel(UIPanelName.RegisterPanel);
    }


}
