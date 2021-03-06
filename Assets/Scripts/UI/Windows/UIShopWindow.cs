﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopWindow : UIWindow
{
    public Transform content;
    public Transform pos0;
    public Transform pos1;
    public Shop shop;
    public List<GameObject> Items;

    public Text moneyText;

    public UISkillDesc skillDesc;
    public Text EquipDesc;

    public Sprite notepower_spr;


    public void Open(Shop p_shop)
    {
        gameObject.SetActive(true);
        shop = p_shop;

        Open();

    }

    public override void Init()
    {
        if (Items == null)
        {
            Items = new List<GameObject>();
        }
        ShowGoodsList();
        base.Init();
    }

    public override void SetSelect()
    {
        Items[0].GetComponent<Button>().Select();
        base.SetSelect();

    }


    private void ShowGoodsList()
    {

        moneyText.text = Player.Instance.money.ToString();

        Debug.Log("show list ");
        if (Items.Count > 0)
        {
            for(int i = 0; i < Items.Count; i++)
            {
                Destroy(Items[i]);
            }
            Items.Clear();
        }

        foreach (Goods g in shop.goods)
        {
            var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_Goods1"), content);
            inst.GetComponent<UISelectableItem>().goods = g;

            if (g.type == GoodsType.skill)
            {
                inst.transform.Find("Image").GetComponent<Image>().sprite = g.skill.sprite;
                inst.transform.Find("Text").GetComponent<Text>().text = g.skill._name;
                inst.name = g.skill._name;

            }
            if (g.type == GoodsType.equipment)
            {
                inst.GetComponent<UISelectableItem>().goods = g;
                inst.transform.Find("Image").GetComponent<Image>().sprite = g.equipment.Icon;
                inst.transform.Find("Text").GetComponent<Text>().text = g.equipment.name;
                inst.name = g.equipment.name;

            }

            if (g.type == GoodsType.note)
            {
                inst.GetComponent<UISelectableItem>().goods = g;
                inst.transform.Find("Image").GetComponent<Image>().sprite = notepower_spr;
                inst.transform.Find("Text").GetComponent<Text>().text = "Note Power";
                inst.name = "NotePower";

            }



            inst.transform.Find("Text_price").GetComponent<Text>().text = g.price.ToString();
            if (g.isSold)
            {
                SetSold(inst);
            }
            Items.Add(inst);
            inst.transform.localPosition = pos0.localPosition - (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);

        }
    }


    private void RefreshGoodsList()
    {
        moneyText.text = Player.Instance.money.ToString();


        foreach (GameObject inst in Items)
        {
            Goods g = inst.GetComponent<UISelectableItem>().goods;

            if (g.type == GoodsType.skill)
            {
                inst.transform.Find("Image").GetComponent<Image>().sprite = g.skill.sprite;
                inst.transform.Find("Text").GetComponent<Text>().text = g.skill._name;
                inst.name = g.skill._name;

            }
            if (g.type == GoodsType.equipment)
            {
                inst.GetComponent<UISelectableItem>().goods = g;
                inst.transform.Find("Image").GetComponent<Image>().sprite = g.equipment.Icon;
                inst.transform.Find("Text").GetComponent<Text>().text = g.equipment.name;
                inst.name = g.equipment.name;

            }

            if (g.type == GoodsType.note)
            {
                inst.GetComponent<UISelectableItem>().goods = g;
                inst.transform.Find("Image").GetComponent<Image>().sprite = notepower_spr;
                inst.transform.Find("Text").GetComponent<Text>().text = "Note Power";
                inst.name = "NotePower";

            }
            inst.transform.Find("Text_price").GetComponent<Text>().text = g.price.ToString();
            if (g.isSold)
            {
                SetSold(inst);
            }

        }
    }

    public override void OnClick(Button button)
    {
        base.OnClick(button);

        button.GetComponent<UISelectableItem>().goods.Buy();



    }

    public override void Focus()
    {
        RefreshGoodsList();

        base.Focus();
        UpdateDesc(tempselect.GetComponent<Button>());

    }

    public override void Close(bool isChange=true)
    {
        if (Items.Count > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Destroy(Items[i]);
            }
            Items.Clear();
        }
        base.Close(isChange);
    }

    private void SetSold(GameObject go)
    {
        go.transform.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        Color tempcolor = go.transform.Find("Text").GetComponent<Text>().color;
        go.transform.Find("Text").GetComponent<Text>().color = new Color(tempcolor.r, tempcolor.g, tempcolor.b, 0.5f);
        go.transform.Find("Text_price").GetComponent<Text>().text = "sold";
    }


    public override void OnSelect(Button button)
    {
//        Debug.Log("button.GetComponent<UISelectableItem>().type == 1" + button.GetComponent<UISelectableItem>().type);

        base.OnSelect(button);
        UpdateDesc(button);
    }




    //更新右侧的详情
    public void UpdateDesc(Button button)
    {
        if (button.GetComponent<UISelectableItem>().goods.type == GoodsType.skill)
        {
            skillDesc.gameObject.SetActive(true);
            EquipDesc.gameObject.SetActive(false);

            skillDesc.Init(new Skill(button.GetComponent<UISelectableItem>().goods.skill));

        }
        if (button.GetComponent<UISelectableItem>().goods.type == GoodsType.equipment)
        {
            skillDesc.gameObject.SetActive(false);
            EquipDesc.gameObject.SetActive(true);
            EquipDesc.text = button.GetComponent<UISelectableItem>().goods.equipment.equipDesc;

        }

        if (button.GetComponent<UISelectableItem>().goods.type == GoodsType.note)
        {
            skillDesc.gameObject.SetActive(false);
            EquipDesc.gameObject.SetActive(true);
            EquipDesc.text = "To Upgrade Skills";

        }
    }
}
