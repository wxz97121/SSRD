using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINoticeWindow : UIWindow
{
    public Text desctext;
    public override void SetSelect()
    {
        buttons.Find(a => a.name == "OK").Select();
        base.SetSelect();

    }

    public override void OnClick(Button button)
    {
        base.OnClick(button);
        if (button.name == "OK")
        {
            Close();
        }
    }

    public void Open(string content)
    {
        Open();
        desctext.text = content;
    }
}
