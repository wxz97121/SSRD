using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SkillType
{
    normal=1,
    heavy=2,
    defend=3,
    special=4,
    ultra=5,
    free=6

}


public class Skill
{
    public SkillType type;

    public string m_name;
    public string EffectStr;
    public int cost;
    public Sprite Icon;
    //输入方式
    public List<Note> inputSequence;
    public void EffectFunction(Character m_Char)
    {

        CommonEffect(m_Char, EffectStr);
    }

    public void CommonEffect(Character m_Char,string str)
    {
        string[] EffectStrSplit = str.Split(',');
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
                case ("EPT"):
                    break;
                case ("ATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("ATKmini"):
                    ATKmini(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("ANI"):
                    ANI(InstancedEff[1], m_Char as Player);

                    break;
                case ("HEL"):
                    HEL(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DEF"):
                    DEF(m_Char);
                    break;
                //三倍蓄力
                case ("TBD"):
                    TBD(int.Parse(InstancedEff[1]),m_Char);
                    break;
                //全力一击
                case ("ALLMPATK"):
                    ALLMPATK(m_Char);
                    break;
                //必杀
                case ("ULTI"):
                    ULTI(m_Char, InstancedEff[1]);
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
        Icon = data.sprite;
        type = data.type;
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

    public Skill(int type,string eff)
    {
        m_name = "";
        EffectStr = eff;
    }

    public Skill()
    {

    }
    //public UnityEvent EffectEvent;
    //public List<Effect> effects;

    public void ATK(int dDamage, Character Char)
    {
        Char.Hit(dDamage);
        //        Debug.Log("ATK "+ dDamage);
    }

    public void ATKmini(int dDamage, Character Char)
    {
        Char.Hit(dDamage,true);
        //        Debug.Log("ATK "+ dDamage);
    }

    public void HEL(int dHeal, Character Char)
    {
        Char.Heal(dHeal);
    }

    public void DEF(Character Char)
    {
        Debug.Log("DEF");
        Buff_defend defend = new Buff_defend
        {
            m_name = "defend",
            remainBeats = 2,
            //character=Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        //Char.buffs.Add(defend);
        defend.BuffAdded(Char);

    }

    public void ANI(string aniname, Player Char)
    {
        //        Debug.Log(aniname);

        Char.animator.Play(aniname, 0);
    }

    //三倍伤害
    public void TBD(int c, Character Char)
    {
//        Debug.Log("TBD");

        Buff_tripledamage buff =new Buff_tripledamage();
        buff.BuffAdded(Char);

    }

    public void ALLMPATK(Character Char)
    {
        Char.Hit(Char.Mp/2);
        Char.Mp = 0;
        Char.AddSoul(4);
    }

    public void ULTI(Character Char,string scorePath)
    {
        var score = new OneSongScore();
        string path = "Data/Skill/UltiScores/" + scorePath;
        Debug.Log("path = "+path);
        score = OneSongScore.ReadQTEScoreData(Resources.Load(path) as QTEScoreData);
        RhythmController.Instance.UltiQTEStart(score);
        Char.ClearSoul();

    }
}
