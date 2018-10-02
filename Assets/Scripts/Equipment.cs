using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//装备类，之后一种装备写一个子类
public class Equipment : MonoBehaviour {

    /*装备类型
     1-护甲-改善防御
     2-武器-改善攻击
     3-卷轴-单次使用
     4-一次性道具，ROGUELIKE中不占装备槽的永久性道具
     */
    public enum equipType
    {
        Armor=1,
        Weapon=2,
        Scroll=3,
        onetime=4
    }

    public equipType type = equipType.Armor;
    //装备提供的基础属性加成
    public int ATK=0;
    public int DEF=0;
    public int maxHp=0;
    public int maxMp=0;

    //UI图
    public Sprite spr;

    virtual protected void init()
    {
        //装备戴上瞬间的效果在此实现
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //如果装备有实时的特殊效果，应该此这实现？

    }
}
