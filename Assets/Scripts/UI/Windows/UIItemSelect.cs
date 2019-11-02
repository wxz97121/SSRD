﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//准备界面点进去，选择要替换的技能、装备
public class UIItemSelect : UIWindow
{
    public Transform content;
    public Transform pos0;
    public Transform pos1;
    public Button targetButton;
    public List<GameObject> Items;


    public void Open(Button button)
    {
        gameObject.SetActive(true);
        targetButton = button;

        Open();

    }

    // Start is called before the first frame update
    public override void Init()
    {
        Items = new List<GameObject>();
        ShowSameItemList(targetButton.GetComponent<UISelectableItem>());
        base.Init();
    }


    public override void SetSelect()
    {
        Items[0].GetComponent<Button>().Select();

        base.SetSelect();
    }

    

    private void ShowSameItemList(UISelectableItem item)
    {
        if (item.type == 1)
        {
            foreach (Skill skill in Player.Instance.skillList)
            {
                if (skill.type == item.skill.type)
                {
                    var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_Item1"), content);
                    inst.GetComponent<UISelectableItem>().skill = skill;
                    inst.transform.Find("Image").GetComponent<Image>().sprite = skill.Icon;
                    inst.transform.Find("Text").GetComponent<Text>().text = skill.m_name;
                    inst.transform.Find("isEquiped").gameObject.SetActive(skill.isEquiped);

                    Items.Add(inst);
                    inst.transform.localPosition = pos0.localPosition - (pos1.localPosition-pos0.localPosition) * (Items.Count - 1);
                }

            }
        }else if(item.type == 2)
        {
            Debug.Log("222");
            foreach (Equipment equipment in Player.Instance.equipmentList)
            {
                Debug.Log("equiptype : " + equipment.type + "   item type : " + item.equipment.type);
                if (equipment.type == item.equipment.type)
                {
                    Debug.Log("333");

                    var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_Item1"), content);
                    inst.GetComponent<UISelectableItem>().equipment = equipment;
                    inst.transform.Find("Image").GetComponent<Image>().sprite = equipment.Icon;
                    inst.transform.Find("Text").GetComponent<Text>().text = equipment.name;
                    inst.transform.Find("isEquiped").gameObject.SetActive(equipment.isEquiped);

                    Items.Add(inst);
                    inst.transform.localPosition = pos0.localPosition - (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);
                }

            }
        }
    }


    public override void OnClick(Button button)
    {
        if (isFocus)
        {
            base.OnClick(button);
            switch (targetButton.name)
            {
                case "Attack":
                    Player.Instance.EquipSkill(0, button.GetComponent<UISelectableItem>().skill);
                    break;
                case "Defend":
                    Player.Instance.EquipSkill(1, button.GetComponent<UISelectableItem>().skill);
                    break;
                case "Special1":
                    Player.Instance.EquipSkill(2, button.GetComponent<UISelectableItem>().skill);
                    break;
                case "Special2":
                    Player.Instance.EquipSkill(3, button.GetComponent<UISelectableItem>().skill);
                    break;
                case "Ulti":
                    Player.Instance.EquipSkill(4, button.GetComponent<UISelectableItem>().skill);
                    break;
                case "Weapon":
                    Player.Instance.EquipEquipment(button.GetComponent<UISelectableItem>().equipment);
                    break;
                case "Cloth":
                    Player.Instance.EquipEquipment(button.GetComponent<UISelectableItem>().equipment);
                    break;
                case "Amulet":
                    Player.Instance.EquipEquipment(button.GetComponent<UISelectableItem>().equipment);
                    break;
            }

            Close();
        }

    }

    public override void Close()
    {
        foreach (GameObject go in Items)
        {
            Destroy(go);
        }
        Items.Clear();
        base.Close();
    }
}
