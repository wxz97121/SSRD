using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIWinWindow : UIWindow
{
    public Text desc;
    public Text title;

    public override void SetSelect()
    {
        base.SetSelect();
        buttons.Find(a => a.name == "Continue").Select();
    }

    public override void OnClick(Button button)
    {
        base.OnClick(button);
        if (button.name == "Continue")
        {
            SuperController.Instance.ContinueAfterWin();

            this.gameObject.SetActive(false);
        }
    }
}
