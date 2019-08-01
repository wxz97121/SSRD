using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_sisyphus : Buff
{

    public override void BuffAdded(Character p_chara,string str="")
    {
        m_name = "sisyphus";
        remainBeats = -1;

        //角色身上已经有本BUFF的情况
        if (p_chara.hasBuff<Buff_autoenergy>())
        {
            return;
        }

        base.BuffAdded(p_chara,str);

        multicount = -1;
    }

    public override void PlayerUpdate()
    {
        base.PlayerUpdate();

    }

    public override void AfterNoAction()
    {
        Player.Instance.life = Player.Instance.maxLife;
        Player.Instance.Hp = Player.Instance.maxHp;
        DuelController.Instance.GetCurAI().life = DuelController.Instance.GetCurAI().maxLife;
        DuelController.Instance.GetCurAI().Hp = DuelController.Instance.GetCurAI().maxHp;
    }
}
