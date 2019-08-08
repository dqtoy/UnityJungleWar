using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomListPanel : BasePanel
{
    private RectTransform user_info = null;
    private RectTransform room_list = null;
    private VerticalLayoutGroup layout = null;
    private GameObject room_item_prefab = null;
    private void Start()
    {
        if (user_info == null) user_info = transform.Find("UserInfo").GetComponent<RectTransform>();
        if (room_list == null) room_list = transform.Find("RoomList").GetComponent<RectTransform>();
        if (layout == null) layout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        if (room_item_prefab == null) room_item_prefab = Resources.Load("UIPanel/RoomItem") as GameObject;

        RegisterButtonClick();
    }

    private void Update()
    {

    }

    private void SetUserInfo()
    {
        UserInfo info = Root.Instance.player_manager.user_info;
        user_info.transform.Find("Username").GetComponent<Text>().text = info.usrename;
        user_info.transform.Find("TotalCount").GetComponent<Text>().text = "总场数:" + info.totalcount.ToString();
        user_info.transform.Find("WinCount").GetComponent<Text>().text = "胜利场数:" + info.wincount.ToString();
    }

    public void LoadRoomItem(List<UserInfo> list)
    {
        if (list == null)
        {
            RoomItem[] items = layout.GetComponentsInChildren<RoomItem>();
            foreach (RoomItem item in items)
            {
                item.Destroy();
            }
            layout.GetComponent<RectTransform>().sizeDelta = new Vector2(layout.GetComponent<RectTransform>().sizeDelta.x, 0);
            return;
        }
        // debug
        Debug.Log("得到的用户信息数量有" + list.Count);
        RoomItem[] room_items = layout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem item in room_items)
        {
            item.Destroy(); // 销毁自身
        }

        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject item = GameObject.Instantiate(room_item_prefab);
            item.transform.SetParent(layout.transform);
            UserInfo info = list[i];
            item.GetComponent<RoomItem>().SetRoomInfo(info.id, info.usrename, info.totalcount, info.wincount);
        }

        // 设置高度
        int room_count = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = layout.GetComponent<RectTransform>().sizeDelta;
        layout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, room_count * (room_item_prefab.GetComponent<RectTransform>().sizeDelta.y + layout.spacing));
    }

    #region UI
    public override void OnEnter()
    {
        if (user_info == null) user_info = transform.Find("UserInfo").GetComponent<RectTransform>();
        if (room_list == null) room_list = transform.Find("RoomList").GetComponent<RectTransform>();
        if (layout == null) layout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        if (room_item_prefab == null) room_item_prefab = Resources.Load("UIPanel/RoomItem") as GameObject;

        EnterAnim();
        SetUserInfo();
        GetComponent<ListRoomRequest>().SendRequest();
    }
    public override void OnExit()
    {
        HideAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
        SetUserInfo();
        GetComponent<ListRoomRequest>().SendRequest();
    }
    public override void OnPause()
    {
        HideAnim();
    }
    #endregion

    private void RegisterButtonClick()
    {
        PlayClickAudio();
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
        transform.Find("RoomList/CreateButton").GetComponent<Button>().onClick.AddListener(OnCreateButtonClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshButtonClick);
    }
    private void OnCloseButtonClick()
    {
        PlayClickAudio();
        Root.Instance.ui_manager.PopPanel();
    }
    private void OnCreateButtonClick()
    {
        PlayClickAudio();
        GetComponent<CreateRoomRequest>().SendRequest();
    }
    private void OnRefreshButtonClick()
    {
        PlayClickAudio();
        GetComponent<ListRoomRequest>().SendRequest();
    }
    public void OnJoinButonClick(int room_id)
    {
        Debug.Log("RoomListPanel的JoinClick方法");
        GetComponent<JoinRoomRequest>().SendRequest(room_id);
    }

    public void JoinRoomResponse(string data, UserInfo info1, UserInfo info2)
    {
        Debug.Log("RoomListPanel JoinResponse");
        switch (data)
        {
            case "NoThisRoom":
                Root.Instance.ui_manager.ShowMessage("房间已不存在 请刷新");
                break;
            case "RoomIsFull":
                Root.Instance.ui_manager.ShowMessage("房间已满员");
                break;
            case "RoomIsNotWaiting":
                Root.Instance.ui_manager.ShowMessage("房间此时不可加入");
                break;
            default:
                Debug.Log("Success");
                Root.Instance.ui_manager.PushPanel(UIPanelName.RoomPanel);
                RoomPanel room_panel = (RoomPanel)Root.Instance.ui_manager.GetPanel(UIPanelName.RoomPanel);
                room_panel.SetHostPlayerInfo(info1.usrename, info1.totalcount.ToString(), info1.wincount.ToString());
                room_panel.SetNotHostPlayerInfo(info2.usrename, info2.totalcount.ToString(), info2.wincount.ToString());
                break;
        }
    }

    private void EnterAnim()
    {
        user_info.localPosition = new Vector3(-1000, 0, 0);
        user_info.DOLocalMoveX(-308, 0.5f);
        room_list.localPosition = new Vector3(1000, 0, 0);
        room_list.DOLocalMoveX(107, 0.5f);
    }
    private void HideAnim()
    {
        user_info.DOLocalMoveX(-1000, 0.5f);
        room_list.DOLocalMoveX(1000, 0.5f);
    }
}
