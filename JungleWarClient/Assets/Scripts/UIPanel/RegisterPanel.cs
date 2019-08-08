using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RegisterPanel : BasePanel
{
    public override void OnEnter()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.4f);
    }
    public override void OnExit()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
    }
    public override void OnResume()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.4f);
    }
    public override void OnPause()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
    }

    private void Start()
    {
        RegisterButtonClick();
    }

    private void RegisterButtonClick()
    {
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterButtonClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
    }

    #region 按钮点击事件
    private void OnRegisterButtonClick()
    {
        PlayClickAudio();
        string username = transform.Find("UsernameLabel/InputField").GetComponent<InputField>().text;
        string password = transform.Find("PasswordLabel/InputField").GetComponent<InputField>().text;
        string repassword = transform.Find("RepeatPasswordLabel/InputField").GetComponent<InputField>().text;
        string msg = "";
        if(string.IsNullOrEmpty(username)) msg += "输入用户名啊!";
        if(string.IsNullOrEmpty(password)) msg += " 输入密码啊!";
        if(string.IsNullOrEmpty(repassword)) msg += " 确认密码啊!";
        if (msg != "") { Root.Instance.ui_manager.ShowMessage(msg); return; }
        // 发送注册请求
        GetComponent<RegisterRequest>().SendRequest(username, password);
    }
    private void OnCloseButtonClick()
    {
        PlayClickAudio();
        Root.Instance.ui_manager.PopPanel();
    }
    #endregion
}
