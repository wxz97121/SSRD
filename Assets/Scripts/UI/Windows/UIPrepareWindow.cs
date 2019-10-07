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

            UISelectableItem item = button.GetComponent<UISelectableItem>();
            bool hasitem = false;
            if (item.type == 1)
            {
                foreach (Skill skill in Player.Instance.skillList)
                {
                    if (skill.type == item.skill.type)
                    {
                        hasitem = true;
                        break;
                    }

                }
            }
            else if (item.type == 2)
            {
                Debug.Log("222");
                foreach (Equipment equipment in Player.Instance.equipmentList)
                {
                    Debug.Log("equiptype : " + equipment.type + "   item type : " + item.equipment.type);
                    if (equipment.type == item.equipment.type)
                    {
                        hasitem = true;
                        break;
                    }

                }
            }

            if (hasitem)
            {
                UIWindowController.Instance.itemSelect.Open(button);
            }
            else
            {
                UIWindowController.Instance.noticeWindow.Open("no such items");
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


        Debug.Log("init");
        InitInfo();
    }

    public void InitInfo()
    {
        if (Player.Instance.skillSlots[0].skill != null)
        {
            buttonSkillAttack.GetComponent<UISelectableItem>().skill = Player.Instance.skillSlots[0].skill;
            buttonSkillAttack.GetComponent<UISelectableItem>().type = 1;
            Debug.Log("icon"+ Player.Instance.skillSlots[0].skill.Icon);

            buttonSkillAttack.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.skillSlots[0].skill.Icon;
        }
        else
        {
            buttonSkillAttack.GetComponent<UISelectableItem>().skill = new Skill() { type= SkillType.attack};
            buttonSkillAttack.GetComponent<UISelectableItem>().type = 1;
            buttonSkillAttack.transform.Find("Image").GetComponent<Image>().sprite = null;
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
            buttonSkillDefend.transform.Find("Image").GetComponent<Image>().sprite = null;

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
            buttonSkillSpecial1.transform.Find("Image").GetComponent<Image>().sprite = null;

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
            buttonSkillSpecial2.transform.Find("Image").GetComponent<Image>().sprite = null;

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
            buttonSkillUlti.transform.Find("Image").GetComponent<Image>().sprite = null;

        }


        if (Player.Instance.currentWeapon != null)
        {
            buttonEquipWeapon.GetComponent<UISelectableItem>().equipment = Player.Instance.currentWeapon;
            buttonEquipWeapon.GetComponent<UISelectableItem>().type = 2;
            buttonEquipWeapon.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.currentWeapon.Icon;
        }
        else
        {
            buttonEquipWeapon.GetComponent<UISelectableItem>().equipment = new Equipment { type = equipType.Weapon };
            buttonEquipWeapon.GetComponent<UISelectableItem>().type = 2;
            buttonEquipWeapon.transform.Find("Image").GetComponent<Image>().sprite = null;
        }

        if (Player.Instance.currentCloth != null)
        {
            buttonEquipCloth.GetComponent<UISelectableItem>().equipment = Player.Instance.currentCloth;
            buttonEquipCloth.GetComponent<UISelectableItem>().type = 2;
            buttonEquipCloth.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.currentCloth.Icon;
        }
        else
        {
            buttonEquipCloth.GetComponent<UISelectableItem>().equipment = new Equipment { type = equipType.Cloth };
            buttonEquipCloth.GetComponent<UISelectableItem>().type = 2;
            buttonEquipCloth.transform.Find("Image").GetComponent<Image>().sprite = null;
        }

        if (Player.Instance.currentAmulet != null)
        {
            buttonEquipAmulet.GetComponent<UISelectableItem>().equipment = Player.Instance.currentAmulet;
            buttonEquipAmulet.GetComponent<UISelectableItem>().type = 2;
            buttonEquipAmulet.transform.Find("Image").GetComponent<Image>().sprite = Player.Instance.currentAmulet.Icon;
        }
        else
        {
            buttonEquipAmulet.GetComponent<UISelectableItem>().equipment = new Equipment { type = equipType.Amulet };
            buttonEquipAmulet.GetComponent<UISelectableItem>().type = 2;
            buttonEquipAmulet.transform.Find("Image").GetComponent<Image>().sprite = null;
        }

    }

    public override void Focus()
    {
        base.Focus();
        InitInfo();
    }

}
