using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    UIManager ui_manager;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Root.Instance.camera_manager.FollowTarget();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Root.Instance.camera_manager.WalkThrougnScene();
        }
    }
    private void TestUIManager()
    {
        ui_manager = new UIManager();
    }
}
