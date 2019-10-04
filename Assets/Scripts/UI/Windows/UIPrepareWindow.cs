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
        base.OnClick(button);
        if (button.name == "Continue")
        {

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
        if(Player.Instance.skillSlots[0].skill!=null)
            buttonSkillAttack.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[0].skill.Icon;
        if (Player.Instance.skillSlots[1].skill != null)
            buttonSkillDefend.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[1].skill.Icon;
        if (Player.Instance.skillSlots[2].skill != null)
            buttonSkillSpecial1.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[2].skill.Icon;
        if (Player.Instance.skillSlots[3].skill != null)
            buttonSkillSpecial2.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[3].skill.Icon;
        if (Player.Instance.skillSlots[4].skill != null)
            buttonSkillUlti.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[4].skill.Icon;

    }



}
