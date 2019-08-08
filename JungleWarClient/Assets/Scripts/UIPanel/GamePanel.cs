using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanel : BasePanel
{
    private Text timer = null;
    private Button success_button;
    private Button fail_button;

    private void Start()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        success_button = transform.Find("SuccessButton").GetComponent<Button>();
        fail_button = transform.Find("FailButton").GetComponent<Button>();
        success_button.gameObject.SetActive(false);
        fail_button.gameObject.SetActive(false);
        success_button.onClick.AddListener(OnResultButtonClick);
        fail_button.onClick.AddListener(OnResultButtonClick);
    }

    public void ShowTime(int time)
    {
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color temp_color = timer.color;
        temp_color.a = 1;
        timer.color = temp_color;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
        Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.ALERT); // 播放Alert声效
    }

    public override void OnEnter()
    {
        transform.Find("SuccessButton").gameObject.SetActive(false);
        transform.Find("FailButton").gameObject.SetActive(false);
    }
    public override void OnExit()
    {
        success_button.gameObject.SetActive(false);
        success_button.gameObject.SetActive(false);
    }
    public override void OnResume() { }
    public override void OnPause() { }

    public void OnGameOverResponse(string data)
    {
        if (data == "Success")
        {
            success_button.gameObject.SetActive(true);
            success_button.transform.localScale = Vector3.zero;
            success_button.transform.DOScale(1, 0.5f);
        }
        else if (data == "Fail")
        {
            fail_button.gameObject.SetActive(true);
            fail_button.transform.localScale = Vector3.zero;
            fail_button.transform.DOScale(1, 0.5f);
        }
        else
        {
            Debug.Log("接收的什么鬼啊");
        }
        Root.Instance.camera_manager.WalkThrougnScene();
        Root.Instance.player_manager.GameOver();
    }

    public void OnResultButtonClick()
    {
        Debug.Log("点击结果按钮");
        Root.Instance.ui_manager.PopPanel();
        Root.Instance.ui_manager.PopPanel();
    }
}
