using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死亡时给主角增加金钱
public class Buff_addmoneywhendie : Buff
{

    public override void BuffAdded(Character p_chara,string str="")
    {

        m_name = "addmoneywhendie";
        remainBeats = 3;

        //角色身上已经有本BUFF的情况，进行叠层

        Buff_addmoneywhendie oldbuff = p_chara.buffs.FindLast(b => b.m_name == "addmoneywhendie") as Buff_addmoneywhendie;
        if (oldbuff != null)
        {
            //如果层数已满，则什么都不发生
            if (oldbuff.multicount >= 100)
            {
                return;
            }

            oldbuff.multicount++;



            Debug.Log("死亡掉钱："+ oldbuff.multicount);
            return;
        }

        base.BuffAdded(p_chara,str);


        multicount = 1;

        Debug.Log("死亡掉钱：1 首次叠加");

    }



    public override void WhenDie(Character p_chara)
    {
        Player.Instance.AddMoney(multicount);
        Debug.Log("加钱啦！！！");
        base.WhenDie(p_chara);
    }

}
