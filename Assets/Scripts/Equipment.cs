using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum equipType
{
    Armor = 1,
    Cloth = 2,
    Amulet = 3,
    Onetime = 4
}
[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment Data")]

public class Equipment: ScriptableObject{

    /*装备类型
     1-护甲-改善防御
     2-武器-改善攻击
     3-卷轴-单次使用
     4-一次性道具，ROGUELIKE中不占装备槽的永久性道具
     */
    public int id;
    public string equipName;
    public string equipDesc;
    //UI图
    public Sprite Icon;

    public equipType type = equipType.Armor;
    //装备提供的基础属性加成
    public int ATK=0;
    public int DEF=0;
    public int maxHp=0;
    public int maxMp=0;

    public List<string> buffs;



    // Update is called once per frame
    virtual protected void Update()
    {
        //如果装备有实时的特殊效果，应该此这实现？

    }

    public void AddBuffs()
    {
        Debug.Log("Add Buffs");
        foreach (var buffstr in buffs)
        {
            Type buffType = Type.GetType("Buff_"+buffstr);
            var buff = (Buff)Activator.CreateInstance(buffType);
            Debug.Log(buff);
            buff.BuffAdded(Player.Instance);
        }
    }

    public virtual void OnEquip()
    {
        AddBuffs();
    }
}
