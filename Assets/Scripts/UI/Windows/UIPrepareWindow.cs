using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPrepareWindow : UIWindow
{
    public Button buttonSkillAttack;
    public Button buttonSkillDefend;
    public Button buttonSkillSpecial1;
    public Button buttonSkillSpecial2;
    public Button buttonSkillUlti;

    public Button buttonEquipWeapon;
    public Button buttonEquipCloth;
    public Button buttonEquipAmulet;

    public Text textATK;
    public Text textDEF;
    public Text textMoney;
    public Text textNote;

    public Text textDesc;


    public override void SetSelect()
    {
        buttons.Find(a => a.name == "Attack").Select();
        base.SetSelect();

    }


    public override void OnClick(Button button)
    {
        if (isFocus)
        {
            base.OnClick(button);


            UIWindowController.Instance.itemSelect.Open(button);
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


        Debug.Log("init");
        InitInfo();
    }

    public void InitInfo()
    {
        if (Player.Instance.skillSlots[0].skill != null)
        {
            buttonSkillAttack.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[0].skill;
            buttonSkillAttack.GetComponent<UISelectableItem>().type = 1;
            buttonSkillAttack.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[0].skill.Icon;
        }
        else
        {
            buttonSkillAttack.GetComponent<UISelectableItem>().skill = new Skill() { type= SkillType.attack};
            buttonSkillAttack.GetComponent<UISelectableItem>().type = 1;
            //todo:增加缺省图标
        }

        if (Player.Instance.skillSlots[1].skill != null)
        {
            buttonSkillDefend.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[1].skill;
            buttonSkillDefend.GetComponent<UISelectableItem>().type = 1;
            buttonSkillDefend.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[1].skill.Icon;
        }
        else
        {
            buttonSkillDefend.GetComponent<UISelectableItem>().skill = new Skill() { type = SkillType.defend };
            buttonSkillDefend.GetComponent<UISelectableItem>().type = 1;
        }


        if (Player.Instance.skillSlots[2].skill != null)
        {
            buttonSkillSpecial1.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[2].skill;
            buttonSkillSpecial1.GetComponent<UISelectableItem>().type = 1;
            buttonSkillSpecial1.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[2].skill.Icon;
        }
        else
        {
            buttonSkillSpecial1.GetComponent<UISelectableItem>().skill = new Skill() { type = SkillType.special };
            buttonSkillSpecial1.GetComponent<UISelectableItem>().type = 1;
        }

        if (Player.Instance.skillSlots[3].skill != null)
        {
            buttonSkillSpecial2.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[3].skill;
            buttonSkillSpecial2.GetComponent<UISelectableItem>().type = 1;
            buttonSkillSpecial2.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[3].skill.Icon;
        }
        else
        {
            buttonSkillSpecial2.GetComponent<UISelectableItem>().skill = new Skill() { type = SkillType.special };
            buttonSkillSpecial2.GetComponent<UISelectableItem>().type = 1;
        }

        if (Player.Instance.skillSlots[4].skill != null)
        {
            buttonSkillUlti.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[4].skill;
            buttonSkillUlti.GetComponent<UISelectableItem>().type = 1;
            buttonSkillUlti.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[4].skill.Icon;
        }
        else
        {
            buttonSkillUlti.GetComponent<UISelectableItem>().skill =  new Skill() { type = SkillType.ulti };
            buttonSkillUlti.GetComponent<UISelectableItem>().type = 1;
        }

    }



}
