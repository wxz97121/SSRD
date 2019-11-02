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
    public Text pointText;

    public Skill skill;

    public Button targetButton;

    [Header("普通按钮的SS")]

    public SpriteState ssNormal;
    [Header("已选中的按钮的SS")]

    public SpriteState ssActive;
    [Header("不可选的按钮的SS")]

    public SpriteState ssDisable;

    [Header("默认状态")]
    public Sprite normalspr;
    public Sprite activespr;
    public Sprite disablespr;

    public int price11;
    public int price21;
    public int price22;
    public int price31;
    public int price32;



    public override void SetSelect()
    {
        buttons.Find(a => a.name == "Button11").Select();
        base.SetSelect();

    }

    public void InitInfo()
    {
        skill = targetButton.GetComponent<UISelectableItem>().skill;
        pointText.text = Player.Instance.skillPoint.ToString();
        Debug.Log(skill.m_name);

        button11.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade11.sprite; 
        button21.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade21.sprite;
        button22.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade22.sprite;
        button31.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade31.sprite;
        button32.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillUpgrade32.sprite;

        button11.transform.Find("Text").GetComponent<Text>().text = skill.skillUpgrade11.name;
        button21.transform.Find("Text").GetComponent<Text>().text = skill.skillUpgrade21.name;
        button22.transform.Find("Text").GetComponent<Text>().text = skill.skillUpgrade22.name;
        button31.transform.Find("Text").GetComponent<Text>().text = skill.skillUpgrade31.name;
        button32.transform.Find("Text").GetComponent<Text>().text = skill.skillUpgrade32.name;

        button11.transform.Find("Price").GetComponent<Text>().text = price11.ToString();
        button21.transform.Find("Price").GetComponent<Text>().text = price21.ToString();
        button22.transform.Find("Price").GetComponent<Text>().text = price22.ToString();
        button31.transform.Find("Price").GetComponent<Text>().text = price31.ToString();
        button32.transform.Find("Price").GetComponent<Text>().text = price32.ToString();

        if (skill.upgradeChoice1 == 0)
        {
            button11.GetComponent<Image>().sprite = normalspr;
            button11.spriteState = ssNormal;
        }

        else if (skill.upgradeChoice1 == 1)
        {
            button11.GetComponent<Image>().sprite = activespr;
            button11.spriteState = ssActive;
        }

        switch (skill.upgradeChoice2)
        {
            case 0:
                if (skill.upgradeChoice1 == 0)
                {
                    button21.GetComponent<Image>().sprite = disablespr;
                    button22.GetComponent<Image>().sprite = disablespr;

                    button21.spriteState = ssDisable;
                    button22.spriteState = ssDisable;
                }
                else
                {
                    button21.GetComponent<Image>().sprite = normalspr;
                    button22.GetComponent<Image>().sprite = normalspr;
                    button21.spriteState = ssNormal;
                    button22.spriteState = ssNormal;
                }
                break;
            case 1:
                button21.GetComponent<Image>().sprite = activespr;
                button22.GetComponent<Image>().sprite = disablespr;
                button21.spriteState = ssActive;
                button22.spriteState = ssDisable;

                break;
            case 2:
                button21.GetComponent<Image>().sprite = disablespr;
                button22.GetComponent<Image>().sprite = activespr;
                button21.spriteState = ssDisable;
                button22.spriteState = ssActive;
                break;
        }

        switch (skill.upgradeChoice3)
        {
            case 0:
                if (skill.upgradeChoice2 == 0)
                {
                    button31.GetComponent<Image>().sprite = disablespr;
                    button32.GetComponent<Image>().sprite = disablespr;
                    button31.spriteState = ssDisable;
                    button32.spriteState = ssDisable;
                }
                else
                {
                    button31.GetComponent<Image>().sprite = normalspr;
                    button32.GetComponent<Image>().sprite = normalspr;
                    button31.spriteState = ssNormal;
                    button32.spriteState = ssNormal;
                }
                break;
            case 1:
                button31.GetComponent<Image>().sprite = activespr;
                button32.GetComponent<Image>().sprite = disablespr;
                button31.spriteState = ssActive;
                button32.spriteState = ssDisable;

                break;
            case 2:
                button31.GetComponent<Image>().sprite = disablespr;
                button32.GetComponent<Image>().sprite = activespr;
                button31.spriteState = ssDisable;
                button32.spriteState = ssActive;
                break;
        }
    }


    public override void OnClick(Button button)
    {
        
        if (isFocus)
        {

            if (button.name == "Button11"&&skill.upgradeChoice1==0)
            {
                if (Player.Instance.skillPoint >= price11)
                {
                    StartCoroutine(SelectUpgrade(11));

                }
                else
                {
                    UIWindowController.Instance.noticeWindow.Open("No Enough Skill Point");

                }

            }
            if (button.name == "Button21" && skill.upgradeChoice2 == 0 && skill.upgradeChoice1 > 0)
            {

                if (Player.Instance.skillPoint >= price21)
                {
                    StartCoroutine(SelectUpgrade(21));

                }
                else
                {
                    UIWindowController.Instance.noticeWindow.Open("No Enough Skill Point");

                }
            }
            if (button.name == "Button22" && skill.upgradeChoice2 == 0&&skill.upgradeChoice1>0)
            {

                if (Player.Instance.skillPoint >= price22)
                {
                    StartCoroutine(SelectUpgrade(22));

                }
                else
                {
                    UIWindowController.Instance.noticeWindow.Open("No Enough Skill Point");

                }
            }

            if (button.name == "Button31" && skill.upgradeChoice3 == 0&&skill.upgradeChoice2>0)
            {

                if (Player.Instance.skillPoint >= price31)
                {
                    StartCoroutine(SelectUpgrade(31));

                }
                else
                {
                    UIWindowController.Instance.noticeWindow.Open("No Enough Skill Point");

                }
            }

            if (button.name == "Button32" && skill.upgradeChoice3 == 0 && skill.upgradeChoice2 > 0)
            {
                if (Player.Instance.skillPoint >= price32)
                {
                    StartCoroutine(SelectUpgrade(32));

                }
                else
                {
                    UIWindowController.Instance.noticeWindow.Open("No Enough Skill Point");

                }
            }
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


    public IEnumerator SelectUpgrade(int number)
    {



        UIWindowController.Instance.confirmWindow.Open("test confirm",this);
        yield return new WaitUntil(UIWindowController.Instance.confirmWindow.Handler);
        UIWindowController.Instance.confirmWindow.bSRD = false;
        switch (number)
        {
            case 11:

                    skill.upgradeChoice1 = 1;
                    Player.Instance.skillPoint -= price11;

                break;
            case 21:

                    skill.upgradeChoice2 = 1;
                    Player.Instance.skillPoint -= price21;

                break;
            case 22:

                    skill.upgradeChoice2 = 2;
                    Player.Instance.skillPoint -= price22;
                
                break;
            case 31:

                    skill.upgradeChoice3 = 1;
                    Player.Instance.skillPoint -= price31;

                break;
            case 32:

                    skill.upgradeChoice3 = 2;
                    Player.Instance.skillPoint -= price32;

                break;
        }

        InitInfo();


    }

}
