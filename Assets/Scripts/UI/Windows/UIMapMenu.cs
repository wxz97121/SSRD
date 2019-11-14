using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMapMenu : UIWindow
{
    public MapArea mapArea;


    public override void SetSelect()
    {
        buttons.Find(a => a.name == "Prepare").Select();
        base.SetSelect();

    }


    public override void OnClick(Button button)
    {
        base.OnClick(button);
        if (button.name == "Prepare")
        {
            UIWindowController.Instance.prepareWindow.Open();
        }
        if (button.name == "Shop")
        {
            UIWindowController.Instance.shopWindow.Open(mapArea.shop);
        }
        if (button.name == "Upgrade")
        {
            UIWindowController.Instance.SkillUpgradeSelectSkill.Open();
        }

    }

    public void OpenMapMenu(MapArea map)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        UIWindowController.Instance.arrow.transform.localScale = Vector3.one;
        mapArea = map;
        if (UIWindowController.Instance.focusWindow != null)
        {
            lastWindow = UIWindowController.Instance.focusWindow;
            UIWindowController.Instance.focusWindow.Unfocus();
        }
        UIWindowController.Instance.focusWindow = this;
        AllButtonConnect();
        StartCoroutine(OpenMapMenuCR());
    }


    IEnumerator OpenMapMenuCR()
    {
        Transform panel = transform.Find("Panel");


        float posx = panel.transform.localPosition.x;
        float posy = panel.transform.localPosition.y;
        float posz = panel.transform.localPosition.z;


        panel.transform.localPosition = new Vector3(posx + 500f, panel.transform.localPosition.y, panel.transform.localPosition.z);
        float time = 0.2f;
        float timecount = 0f;
        float a = 0f;
        //Vector3 startpos = arrow.transform.localPosition;
        while (timecount <= time)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }

            panel.transform.localPosition = Vector3.Lerp(
                    new Vector3(posx + 500f, posy, posz),
                    new Vector3(posx, posy, posz),
                    a
                );
        }

        Init();
        Focus();

    }

    public override void Focus()
    {
        base.Focus();
        if (mapArea.levelData.shopData == null)
        {
            buttons.Find(a => a.name == "Shop").enabled=false;

        }
        else
        {
            buttons.Find(a => a.name == "Shop").enabled = true;

        }
    }

}
