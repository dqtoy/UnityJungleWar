using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoginPanel : BasePanel
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
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginButtonClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
    }

    #region 按钮点击事件
    private void OnLoginButtonClick()
    {
        PlayClickAudio();
        string msg = "";
        string username = transform.Find("UsernameLabel/InputField").GetComponent<InputField>().text;
        string password = transform.Find("PasswordLabel/InputField").GetComponent<InputField>().text;
        if(string.IsNullOrEmpty(username)) msg += "用户名不能为空 ";
        if(string.IsNullOrEmpty(password)) msg += "密码不能为空";
        if (msg != "")
        {
            Root.Instance.ui_manager.ShowMessage(msg);
            return;
        }
        GetComponent<LoginRequest>().SendRequest(username, password); // 向服务器发送请求
    }
    private void OnCloseButtonClick()
    {
        PlayClickAudio();
        Root.Instance.ui_manager.PopPanel();
    }
    #endregion
}
