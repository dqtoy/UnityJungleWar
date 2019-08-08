using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    private const string PATH_PREFIX = "UIPanel/";

    private GameObject canvas;

    private Dictionary<UIPanelName, BasePanel> panel_dict = new Dictionary<UIPanelName, BasePanel>();
    private LinkedList<BasePanel> panel_context = new LinkedList<BasePanel>();

    public UIManager()
    {
        Init();
    }


    /// <summary>
    /// 初始化
    /// 根据UIPanelName枚举创建出所有面板的GameObject并放置到Canvas下 Active设置为false
    /// 将所有面板与枚举类型一一对应交给字典管理
    /// </summary>
    private void Init()
    {
        canvas = GameObject.Find("Canvas");

        string[] panel_names = Enum.GetNames(typeof(UIPanelName));
        foreach (string item in panel_names)
        {
            GameObject panel = GameObject.Instantiate(Resources.Load(PATH_PREFIX + item) as GameObject);
            panel.transform.SetParent(canvas.transform, false);
            panel_dict.Add((UIPanelName)Enum.Parse(typeof(UIPanelName), item), panel.GetComponent<BasePanel>());
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        BasePanel message_panel = null;
        panel_dict.TryGetValue(UIPanelName.MessagePanel, out message_panel);
        MessagePanel panel = message_panel as MessagePanel;
        panel.transform.SetAsLastSibling();
        panel.gameObject.SetActive(true);
        panel.ShowMessage(message);
    }

    public BasePanel GetPanel(UIPanelName name)
    {
        BasePanel panel = null;
        bool is_get = panel_dict.TryGetValue(name, out panel);
        if(is_get) return panel;
        return null;
    }

    public void PushPanel(UIPanelName name)
    {
        // debug
        Debug.Log("PushPanel:" + Enum.GetName(typeof(UIPanelName), name));
        if (panel_context.Count > 0)
        {
            BasePanel top_panel = panel_context.Last.Value;
            // 顶部面板处理
            top_panel.OnPause();
            top_panel.gameObject.SetActive(false);
        }

        BasePanel show_panel = null;
        panel_dict.TryGetValue(name, out show_panel);
        if (panel_context.Contains(show_panel))
        {
            panel_context.Remove(show_panel);
        }
        panel_context.AddLast(show_panel);
        show_panel.gameObject.SetActive(true);
        show_panel.transform.SetAsLastSibling();
        show_panel.OnEnter();
    }

    public void PopPanel()
    {
        if(panel_context.Count <= 0) return;
        BasePanel top_panel = panel_context.Last.Value;
        panel_context.RemoveLast();
        top_panel.OnExit();
        top_panel.gameObject.SetActive(false);

        if (panel_context.Count > 0)
        {
            BasePanel show_panel = panel_context.Last.Value;
            show_panel.transform.SetAsLastSibling();
            show_panel.gameObject.SetActive(true);
            show_panel.OnResume();
        }
    }

    // debug
    private void TestPrint()
    {
        Debug.Log("-------------------------------------");
        foreach (BasePanel item in panel_context)
        {
            Debug.Log("名字 " + item.gameObject.name);
        }
        Debug.Log("--------------------------------------");
    }
}
