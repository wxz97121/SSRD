using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//选择技能天赋界面
public class UIUpgradeSkill : UIWindow
{

    public Button button11;
    public Button button21;
    public Button button22;
    public Button button31;
    public Button button32;

    public Text textDesc;

    public Button targetButton;

    [Header("普通按钮")]

    public SpriteState ssNormal;
    [Header("已选中的按钮")]

    public SpriteState ssActive;
    [Header("不可选的按钮")]

    public SpriteState ssDisable;


    public override void SetSelect()
    {
        buttons.Find(a => a.name == "button11").Select();
        base.SetSelect();

    }

    public void InitInfo()
    {
        Skill skill = targetButton.GetComponent<UISelectableItem>().skill;

        button11.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade11.sprite; 
        button21.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade21.sprite;
        button22.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade22.sprite;
        button31.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade31.sprite;
        button32.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade32.sprite;





        if (skill.upgradeChoice1 == 0)
        {
            button11.spriteState = ssNormal;
        }

        else if (skill.upgradeChoice1 == 1)
        {
            button11.spriteState = ssActive;
        }

        switch (skill.upgradeChoice2)
        {
            case 0:
                if (skill.upgradeChoice1 == 0)
                {
                    button21.spriteState = ssDisable;
                    button22.spriteState = ssDisable;
                }
                else
                {
                    button21.spriteState = ssNormal;
                    button22.spriteState = ssNormal;
                }
                break;
            case 1:
                button21.spriteState = ssActive;
                button22.spriteState = ssDisable;

                break;
            case 2:
                button21.spriteState = ssDisable;
                button22.spriteState = ssActive;
                break;
        }

        switch (skill.upgradeChoice3)
        {
            case 0:
                if (skill.upgradeChoice2 == 0)
                {
                    button31.spriteState = ssDisable;
                    button32.spriteState = ssDisable;
                }
                else
                {
                    button31.spriteState = ssNormal;
                    button32.spriteState = ssNormal;
                }
                break;
            case 1:
                button31.spriteState = ssActive;
                button32.spriteState = ssDisable;

                break;
            case 2:
                button31.spriteState = ssDisable;
                button32.spriteState = ssActive;
                break;
        }
    }


    public override void OnClick(Button button)
    {
        if (isFocus)
        {


        }

    }


    public override void OnSelect(Button button)
    {
        base.OnSelect(button);
        if (button.GetComponent<UISelectableItem>().type == 1)
        {
            textDesc.text = button.GetComponent<UISelectableItem>().skill.Desc;

        }
    }


    public override void Init()
    {
        base.Init();


        InitInfo();
    }


    public override void Focus()
    {
        base.Focus();
        InitInfo();
    }


    public void Open(Button button)
    {
        gameObject.SetActive(true);
        targetButton = button;

        Open();

    }

    public IEnumerator SelectUpgrade(int i)
    {
        yield return new WaitUntil(HandleFunc);
    }

    bool HandleFunc()
    {
        return true;
    }

}
