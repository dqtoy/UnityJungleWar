using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : BasePanel
{
    private float show_time = 1f;

    public void ShowMessage(string message)
    {
        GetComponent<Text>().CrossFadeAlpha(1, 0.2f, false);
        GetComponent<Text>().text = message;
        GetComponent<Text>().enabled = true;
        Invoke("Hide", show_time);
    }
    private void Hide()
    {
        GetComponent<Text>().CrossFadeAlpha(0, 1, false);
    }
}
