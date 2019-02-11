using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySkill
{
    public string m_name;
    private string EffectStr;

    //发动技能
    public void EffectFunction(AI m_Char)
    {

        string[] EffectStrSplit = EffectStr.Split(',');
        //用开头三个大写字母表示功能，后面参数用下划线分割
        //例如 ATK_3 表示暗黑破坏神3
        //例如 HEL_5 表示HTML5
        //例如 BUF_3_4 表示 3号 Buff 持续4回合
        //若干个这样的字符串，用逗号分开，表示一个技能的效果
        foreach (string s in EffectStrSplit)
        {
            string[] InstancedEff = s.Split('_');
            switch (InstancedEff[0])
            {
                case ("ATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("ANI"):
                    ANI(InstancedEff[1], m_Char);
                    break;
                case ("HEL"):
                    HEL(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DEF"):
                    DEF(m_Char);
                    break;
                default:
                    break;
            }
        }
    }

    public EnemySkill(string DataDir)
    {
        EnemySkillData data = Resources.Load(DataDir) as EnemySkillData;
        if (!data)
        {
            Debug.Log(DataDir);
            Debug.Log("这个路径没有EnemySkillData！！");
            Debug.Break();
        }
        m_name = data._name;
        EffectStr = data.Effect;

    }


    private void ATK(int dDamage, Character Char)
    {
        Char.Hit(dDamage);
        //        Debug.Log("ATK "+ dDamage);
    }

    private void HEL(int dHeal, Character Char)
    {
        Char.Heal(dHeal);
    }

    private void DEF(Character Char)
    {
        Debug.Log("DEF");
        Buff_defend defend = new Buff_defend
        {
            m_name = "DEF",
            remainBeats = 2,
            character = Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        Char.buffs.Add(defend);
        defend.BuffAdded();
    }

    private void ANI(string aniname, AI Char)
    {
//        Debug.Log(aniname);

        Char.animator.Play(aniname, 0);
    }
}
