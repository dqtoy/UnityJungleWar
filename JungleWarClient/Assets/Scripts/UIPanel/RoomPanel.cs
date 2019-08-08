using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomPanel : BasePanel
{
    private Text local_username = null;
    private Text local_totalcount = null;
    private Text local_wincount = null;
    private Text enemy_username = null;
    private Text enemy_totalcount = null;
    private Text enemy_wincount = null;

    private Transform blue_panel;
    private Transform red_panel;
    private Transform start_button;
    private Transform quit_button;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
    }

    private void Init()
    {
        local_username = transform.Find("BluePanel/Username").GetComponent<Text>();
        local_totalcount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        local_wincount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemy_username = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemy_totalcount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemy_wincount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        blue_panel = transform.Find("BluePanel");
        red_panel = transform.Find("RedPanel");
        start_button = transform.Find("StartButton");
        quit_button = transform.Find("QuitButton");

        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(OnQuitButtonClick);
    }

    public void SetHostPlayerInfo(string username, string totalcount, string wincount)
    {
        local_username.text = username;
        local_totalcount.text = "总场数\n" + totalcount;
        local_wincount.text = "胜利场数\n" + wincount;
    }
    public void SetNotHostPlayerInfo(string username, string totalcount, string wincount)
    {
        enemy_username.text = username;
        enemy_totalcount.text = "总场数\n" + totalcount;
        enemy_wincount.text = "胜利场数\n" + wincount;
    }
    public void ClearNotHostPlayerInfo()
    {
        enemy_username.text = "正在等待玩家..."; enemy_totalcount.text = ""; enemy_wincount.text = "";
    }

    private void OnStartButtonClick()
    {
        GetComponent<StartGameRequest>().SendRequest(); // 发送开始游戏请求
    }
    // 退出房间按钮点击事件
    private void OnQuitButtonClick()
    {
        GetComponent<QuitRoomRequest>().SendRequest(); // 发送退出房间请求
    }

    public override void OnEnter()
    {
        GetComponent<UpdateRoomRequest>().SendRequest();
        EnterAnim();
    }
    public override void OnExit()
    {
        ExitAnim();
    }
    public override void OnPause()
    {
        EnterAnim();
    }
    public override void OnResume()
    {
        GetComponent<UpdateRoomRequest>().SendRequest();
        ExitAnim();
    }

    private void EnterAnim()
    {
        blue_panel.localPosition = new Vector3(-1000, 0, 0);
        blue_panel.DOLocalMoveX(-179, 0.4f);
        red_panel.localPosition = new Vector3(1000, 0, 0);
        red_panel.DOLocalMoveX(179, 0.4f);
        start_button.localScale = Vector3.zero;
        start_button.DOScale(1, 0.4f);
        quit_button.localScale = Vector3.zero;
        quit_button.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        blue_panel.DOLocalMoveX(-1000, 0.4f);
        red_panel.DOLocalMoveX(1000, 0.4f);
        start_button.DOScale(0, 0.4f);
        quit_button.DOScale(0, 0.4f);
    }
}
