using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIWindow
{

    public override void SetSelect()
    {
        base.SetSelect();
        buttons.Find(a => a.name == "Start").Select();
    }
    public override void onClick(Button button)
    {
        base.onClick(button);
        if (button.name == "Start")
        {
            SuperController.Instance.NextStep("start game");

            this.gameObject.SetActive(false);
        }
    }
}
