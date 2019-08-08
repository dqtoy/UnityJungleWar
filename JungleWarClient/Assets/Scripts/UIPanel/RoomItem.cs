using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private Text username = null;
    private Text totalcount = null;
    private Text wincount = null;
    public Button joinbutton = null;

    private int room_id = -1;

    private void Awake()
    {
        Init();
    }

    public void SetRoomInfo(int id, string username, int totalcount, int wincount)
    {
        this.room_id = id;
        this.username.text = username;
        this.totalcount.text = "总场数\n" + totalcount;
        this.wincount.text = "胜利场数\n" + wincount;
    }
    public void SetRoomInfo(int id, string username, string totalcount, string wincount)
    {
        this.room_id = id;
        this.username.text = username;
        this.totalcount.text = "总场数\n" + totalcount;
        this.wincount.text = "胜利场数\n" + wincount;
    }

    private void Init()
    {
        username = transform.Find("Username").GetComponent<Text>();
        totalcount = transform.Find("TotalCount").GetComponent<Text>();
        wincount = transform.Find("WinCount").GetComponent<Text>();
        if (joinbutton == null)
        {
            joinbutton = transform.Find("JoinButton").GetComponent<Button>();
            joinbutton.onClick.AddListener(OnJoinButtonClick);
        }
    }

    private void OnJoinButtonClick()
    {
        Debug.Log("加入按钮点击一下");
        RoomListPanel room_list_panel = transform.parent.parent.parent.parent.GetComponent<RoomListPanel>();
        room_list_panel.OnJoinButonClick(this.room_id);
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
