using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMapMenu : UIWindow
{

    public override void SetSelect()
    {
        buttons.Find(a => a.name == "Prepare").Select();
        base.SetSelect();

    }


    public override void OnClick(Button button)
    {
        base.OnClick(button);
        if (button.name == "Continue")
        {

        }


    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("cancel");
                
        }

    }
}
