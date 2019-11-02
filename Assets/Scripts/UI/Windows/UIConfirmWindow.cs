using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmWindow : UIWindow
{
    public Text desctext;
    public UIWindow window;
    public bool bSRD;

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
            bSRD = true;
            Close();
        }
        if (button.name == "CANCEL")
        {
            window.StopAllCoroutines();
            Close();
        }
    }

    public void Open(string content,UIWindow p_window)
    {
        desctext.text = content;
        window = p_window;
        Open();

    }

    public bool Handler()
    {
        return bSRD;
    }

}
