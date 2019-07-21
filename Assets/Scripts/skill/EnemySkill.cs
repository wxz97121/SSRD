using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class EnemySkill
{
    public string m_name;
    public string EffectStr;

    public void EffectFunction(AI m_Char)
    {

        CommonEffect(m_Char, EffectStr);
    }

    //发动技能
    public void CommonEffect(AI m_Char,string str)
    {
//        Debug.Log("effect string = " + EffectStr);
        string[] EffectStrSplit = str.Split(',');
        //用开头三个大写字母表示功能，后面参数用下划线分割
        //例如 ATK_3 表示暗黑破坏神3
        //例如 HEL_5 表示HTML5
        //例如 BUF_3_4 表示 3号 Buff 持续4回合
        //WRN_ATK_RED_3 下一小节出现攻击3红色的提示
        //若干个这样的字符串，用逗号分开，表示一个技能的效果
        foreach (string s in EffectStrSplit)
        {
            string[] InstancedEff = s.Split('_');
            switch (InstancedEff[0])
            {
                case ("EPT"):
                    break;
                case ("ATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char,true);
                    break;
                case ("ANI"):
                    ANI(InstancedEff[1], m_Char);
                    break;
                case ("ANIT"):

                    ANI(InstancedEff[1], m_Char,true);
                    break;
                case ("HEL"):
                    HEL(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DEF"):
                    DEF(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("WRN"):
                    WRN(InstancedEff[1], InstancedEff[2], int.Parse(InstancedEff[3]), m_Char, InstancedEff[4]);
                    break;
                case ("DMP"):
                    DMP(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("SFX"):
                    SFX(InstancedEff[1], m_Char);
                    break;
                default:
                    break;
            }
        }
    }

    public EnemySkill(string DataDir)
    {

    }
    public EnemySkill(int type,string Eff)
    {

        m_name = "";
        EffectStr = Eff;

    }

    public EnemySkill()
    {

    }

    private void ATK(int dDamage, Character Char,bool isCounterable= false)
    {
        Char.Hit(dDamage, false, isCounterable); 
        //        Debug.Log("ATK "+ dDamage);
    }

    private void HEL(int dHeal, Character Char)
    {
        Char.Heal(dHeal);
    }

    private void DEF(int beats,Character Char)
    {
//        Debug.Log("DEF");
        Buff_defend defend = new Buff_defend
        {
            m_name = "defend",
            remainBeats = beats,
            //character = Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        defend.BuffAdded(Char);
        //Char.buffs.Add(defend);

    }

    private void ANI(string aniname, AI Char,bool IsSetTrigger=false)
    {
        //                Debug.Log(aniname);
        //if (aniname == "attack") Debug.Log("Attack?????");
        if (IsSetTrigger)
        {
            for(int i = 0; i < Char.animator.parameterCount; i++)
            {
                Char.animator.ResetTrigger(i);

            }

            Char.animator.SetTrigger(aniname);
        }
        else
        {
            Char.animator.Play(aniname, -1);

        }

    }


    //图标提示
    private void WRN(string spr,string col, int num, AI Char,string isNextbar)
    {
        Color color=Color.black;
        if (col == "RED")
        {
            color = Color.red;
        }
        if (isNextbar=="pre") { 
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.gameObject.transform.localScale =new Vector3 (1,1,1);

        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetSprite(spr);
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetText(num.ToString());
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetColor(color);
        }
        else
        {
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.gameObject.transform.localScale = new Vector3(1, 1, 1);

            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetSprite(spr);
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetText(num.ToString());
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetColor(color);
        }

    }

    //吸收能力
    private void DMP(int energy,AI Char)
    {
        if (energy <= 0)
        {
            Char.mTarget.GetComponent<Character>().Mp = 0;

        }
        else
        {
            Char.mTarget.GetComponent<Character>().Mp -= energy;
            if (Char.mTarget.GetComponent<Character>().Mp < 0)
            {
                Char.mTarget.GetComponent<Character>().Mp = 0;

            }
        }
    }

    private void SFX(string name,AI Char)
    {
        SoundController.Instance.PlayAudioEffect(name);
    }
}
