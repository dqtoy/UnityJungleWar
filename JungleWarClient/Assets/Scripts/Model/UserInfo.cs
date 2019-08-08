using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    public int id { get; set; }
    public string usrename { get; set; }
    public int totalcount { get; set; }
    public int wincount { get; set; }

    public UserInfo(string username, int totalcount, int wincount)
    {
        this.usrename = username; this.totalcount = totalcount; this.wincount = wincount;
    }
    public UserInfo(int id, string username, int totalcount, int wincount)
    {
        this.id = id; this.usrename = username; this.totalcount = totalcount; this.wincount = wincount;
    }
    public UserInfo(string str)
    {
        string[] strs = str.Split(',');
        this.id = int.Parse(strs[0]);
        this.usrename = strs[1];
        this.totalcount = int.Parse(strs[2]);
        this.wincount = int.Parse(strs[3]);
    }
}
