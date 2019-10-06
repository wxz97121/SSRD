﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        base.SetSelect();
        Items[0].GetComponent<Button>().Select();
    }

    

    private void ShowSameItemList(UISelectableItem item)
    {
        if (item.type == 1)
        {
            foreach (Skill skill in Player.Instance.skillListInBag)
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