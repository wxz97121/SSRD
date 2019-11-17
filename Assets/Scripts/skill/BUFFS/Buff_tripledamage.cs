using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_tripledamage : Buff
{
    //最大层数
    int maxMultiCount=2;

    //每层倍数
    int attackMulti = 2;

    //挨打是否消失
    bool isAttackedCancel = false;

    public override void BuffAdded(Character p_chara,string str="")
    {


        string[] InstancedEffstr = str.Split('&');

        foreach (string s in InstancedEffstr)
        {
            switch (s.Split(':')[0])
            {
                case ("count"):
                    maxMultiCount = int.Parse(s.Split(':')[1]);
                    break;
                case ("multi"):

                    attackMulti = int.Parse(s.Split(':')[1]);
                    break;
                case ("cancel"):
                    isAttackedCancel = true;
                    break;

            }
        }






        Player.Instance.spec.transform.localScale=new Vector3(0.0195f,0.0195f,0.0195f);
        
        m_name = "tripledamage";
        remainBeats = -1;

        //角色身上已经有本BUFF的情况，进行叠层
        //Buff_tripledamage oldbuff = p_chara.buffs.FindLast(b => b.m_name == "tripledamage") as Buff_tripledamage;
        Buff_tripledamage oldbuff = p_chara.buffs.FindLast(b => b.GetType() == typeof(Buff_tripledamage)) as Buff_tripledamage;
        if (oldbuff != null)
        {
            //如果层数已满，则什么都不发生
            if (oldbuff.multicount >= maxMultiCount)
            {
                //p_chara.Hit(3);
                //p_chara.buffs.Remove(oldbuff);
                return;
            }

            oldbuff.multicount++;
            oldbuff.damageMulti += attackMulti;

            Player.Instance.spec.gameObject.SetActive(true);
            switch (oldbuff.multicount)
            {
                case 1:
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMax = Player.Instance.spec.MaxColor1;
                    Player.Instance.spec._specDictionary["SpecTripledamage"].colorMin = Player.Instance.spec.MinColor1;


                    break;
                case 2:
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

        base.BuffAdded(p_chara,str);


        multicount = 1;
        damageMulti = attackMulti;
        Debug.Log("双倍伤害叠加：1，3");


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
        Player.Instance.spec.transform.localScale=new Vector3(0,0,0);
    }

    public override void AfterAttacked(Character p_chara)
    {
        base.AfterAttacked(p_chara);

        if (isAttackedCancel)
        {
            p_chara.buffs.Remove(this);
            Player.Instance.spec._specDictionary["SpecTripledamage"].isEnabled = false;
            Player.Instance.spec.transform.localScale = new Vector3(0, 0, 0);
        }

    }

}
