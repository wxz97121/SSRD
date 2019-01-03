using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill 
{
    public string m_name;
    private string EffectStr;
    public int cost;
    //输入方式
    public List<Note> inputSequence;
    public void EffectFunction(Character m_Char)
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
            switch(InstancedEff[0])
            {
                case ("ATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("ANI"):
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

    public Skill(string DataDir)
    {
        DataDir = "Data/Skill/" + DataDir;
        SkillData data = Resources.Load(DataDir) as SkillData;
        if (!data)
        {
            Debug.Log("这个路径没有SkillData！！");
            Debug.Break();
        }
        m_name = data._name;
        inputSequence = new List<Note>();
        EffectStr = data.Effect;
        cost = data.cost;
        for (int i = 0; i < data.inputSequence.Count; i++)
        {
            inputSequence.Add(new Note
            {
                type = data.inputSequence[i].type,
                beatInBar = data.inputSequence[i].beatInBar
            }
            );
        }

    }
    //public UnityEvent EffectEvent;
    //public List<Effect> effects;

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
            character=Char,
            activateTime= RhythmController.Instance.songPosInBeats
    };
        Char.buffs.Add(defend);
        defend.BuffAdded();

    }
}
