using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIWindow
{

    public override void SetSelect()
    {
        buttons.Find(a => a.name == "Start").Select();
        base.SetSelect();

    }
    public override void OnClick(Button button)
    {
        base.OnClick(button);
        if (button.name == "Start")
        {
            SuperController.Instance.NextStep("start game");

            this.gameObject.SetActive(false);
        }
    }
}
