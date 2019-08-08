using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    private static Root _instance = null;
    public static Root Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameEngine").GetComponent<Root>();
            }
            return _instance;
        }
    }

    private Net net;

    public UIManager ui_manager = null; // UI管理器
    public RequestManager request_manager = null;
    public AudioManager audio_manager = null;
    public PlayerManager player_manager = null;
    public CameraManager camera_manager = null;

    private void Awake()
    {
        net = GetComponent<Net>();
        net.RegisterHandleEvents(HandleMessageBody);

        InitRequest();
    }

    private void InitRequest()
    {
        ui_manager = new UIManager();
        ui_manager.PushPanel(UIPanelName.StartPanel);
        request_manager = new RequestManager();
        audio_manager = new AudioManager();
        player_manager = new PlayerManager();
        camera_manager = new CameraManager();
    }

    // 处理消息体
    private void HandleMessageBody(byte[] body)
    {
        ActionCode action_code = (ActionCode)BitConverter.ToInt32(body, 0);
        string data = Encoding.UTF8.GetString(body, 4, body.Length - 4);
        request_manager.HandleResponse(action_code, data);
    }

    public void SendRequest(RequestCode request_code, ActionCode action_code, string data)
    {
        byte[] request_bytes = BitConverter.GetBytes((int)request_code);
        byte[] action_bytes = BitConverter.GetBytes((int)action_code);
        byte[] data_bytes = Encoding.UTF8.GetBytes(data);
        byte[] length_bytes = BitConverter.GetBytes(request_bytes.Length + action_bytes.Length + data_bytes.Length);
        net.Send(length_bytes.Concat(request_bytes).Concat(action_bytes).Concat(data_bytes).ToArray());
    }

    private void OnDestroy()
    {
        net.Close();
    }
}
