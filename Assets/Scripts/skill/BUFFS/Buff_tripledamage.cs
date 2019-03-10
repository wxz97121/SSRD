using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_tripledamage : Buff
{

    public override void BuffAdded(Character p_chara)
    {
        m_name = "tripledamage";
        remainBeats = -1;

        //角色身上已经有本BUFF的情况，进行叠层
        Buff_tripledamage oldbuff = p_chara.buffs.FindLast(b => b.m_name == "tripledamage") as Buff_tripledamage;
        if (oldbuff != null)
        {
            //如果层数已满，则直接攻击
            if (oldbuff.multicount >= 3)
            {
                p_chara.Hit(3);
                //p_chara.buffs.Remove(oldbuff);
                return;
            }

            oldbuff.multicount++;
            oldbuff.damageMulti *= 3;

            switch (oldbuff.multicount)
            {
                case 1:
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMax = Player.Instance.spec.MaxColor1;
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMin = Player.Instance.spec.MinColor1;


                    break;
                case 2:
                    Debug.Log("in case 2 now!");
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMax = Player.Instance.spec.MaxColor2;
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMin = Player.Instance.spec.MinColor2;

                    break;
                case 3:
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMax = Player.Instance.spec.MaxColor3;
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMin = Player.Instance.spec.MinColor3;
                    break;

            }
            Player.Instance.spec._specDictionary["SpecTripledamage"].RebuildSpectrum();
            Player.Instance.spec._specDictionary["SpecTripledamage"].isEnabled = true;
            Debug.Log("双倍伤害叠加："+ oldbuff.multicount+","+ oldbuff.damageMulti);
            return;
        }

        base.BuffAdded(p_chara);


        multicount = 1;
        damageMulti = 3;

       

                Player.Instance.spec._specDictionary["SpecTripledamage"].colorMax = Player.Instance.spec.MaxColor1;
                Player.Instance.spec._specDictionary["SpecTripledamage"].colorMin = Player.Instance.spec.MinColor1;

        Player.Instance.spec._specDictionary["SpecTripledamage"].RebuildSpectrum();
        Player.Instance.spec._specDictionary["SpecTripledamage"].isEnabled = true;
    }



    public override void AfterAttack(Character p_chara)
    {
        base.AfterAttack(p_chara);
        p_chara.buffs.Remove(this);
        Player.Instance.spec._specDictionary["SpecTripledamage"].isEnabled = false;

    }

    public override void AfterAttacked(Character p_chara)
    {
        base.AfterAttacked(p_chara);
        p_chara.buffs.Remove(this);
        Player.Instance.spec._specDictionary["SpecTripledamage"].isEnabled = false;

    }

}
