using System.Collections;
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

    public void Open(Shop p_shop)
    {
        gameObject.SetActive(true);
        shop = p_shop;

        Open();

    }

    public override void Init()
    {
        Items = new List<GameObject>();
        ShowGoodsList();
        base.Init();
    }

    public override void SetSelect()
    {
        base.SetSelect();
        Items[0].GetComponent<Button>().Select();
    }


    private void ShowGoodsList()
    {

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
            inst.transform.Find("Text_price").GetComponent<Text>().text = g.price.ToString();
            if (g.isSold)
            {
                SetSold(inst);
            }
            Items.Add(inst);
            inst.transform.localPosition = pos0.localPosition - (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);

        }
    }

    public override void OnClick(Button button)
    {
        base.OnClick(button);

        button.GetComponent<UISelectableItem>().goods.Buy();



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

    private void SetSold(GameObject go)
    {
        go.transform.Find("Image").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        Color tempcolor = go.transform.Find("Text").GetComponent<Text>().color;
        go.transform.Find("Text").GetComponent<Text>().color = new Color(tempcolor.r, tempcolor.g, tempcolor.b, 0.5f);
        go.transform.Find("Text_price").GetComponent<Text>().text = "sold";
    }
}
