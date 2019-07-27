using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_oneTimeShield : Buff
{
    public override void BuffAdded(Character p_chara,string str="")
    {
        base.BuffAdded(p_chara,str);

        //TODO :添加视觉效果
        Object.Instantiate(Resources.Load("VFX/Shield"), character.transform.position, Quaternion.identity);


    }


    public override void BuffRemove()
    {
        base.BuffRemove();
        //TODO :移除视觉效果
        character.transform.Find("oneTimeShield").GetComponent<VFX>().StartCoroutine("FadeOutLarger");

    }
}
