using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GoodsType
{
    skill = 1,
    equipment = 2,
    health = 3,
    energy = 4,
    note = 5
}

[System.Serializable]


//商品类
public class Goods
{
    public string datapath;
    public GoodsType type;
    public int price;
    public SkillData skill;
    public Equipment equipment;
    public bool isSold;

    public void Buy()
    {
        if (isSold)
        {
            UIWindowController.Instance.noticeWindow.Open("You Have Already Bought It");
            return;
        }

        if (Player.Instance.money >= price)
        {
            switch (type)
            {
                case GoodsType.skill:
                    Player.Instance.AddSkill(skill);
                    UIWindowController.Instance.noticeWindow.Open("Now You Have " +skill._name);

                    break;
                case GoodsType.equipment:
                    Player.Instance.AddEquip(equipment);
                    UIWindowController.Instance.noticeWindow.Open("Now You Have " + equipment.name);

                    break;
            }

            isSold = true;

        }
        else
        {
            UIWindowController.Instance.noticeWindow.Open("Not Enough Money");
        }

    }

}
