using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectableItem : MonoBehaviour
{
    //类型
    //1:skill  2:equip 3:goods
    public int type;

    //玩家已经有的技能
    public Skill skill;

    //玩家已有的的装备
    public Equipment equipment;

    //商品
    public Goods goods;

}
