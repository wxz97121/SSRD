﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SkillType
{
    normal=1,
    attack=2,
    defend=3,
    special=4,
    ulti=5,
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
    //CD
    public int CooldownMax;
    public int Cooldown;
    public string Desc;



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
                case ("ATKPRO"):
                    ATKPRO(InstancedEff[1], m_Char);
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
                    //防御成功带技能
                    if (InstancedEff.Length > 1)
                    {
                        DEF(m_Char, InstancedEff[1]);
                    }
                    else
                    {
                        DEF(m_Char, "");
                    }
                    break;
                case ("-CD"):
                    MCD(int.Parse(InstancedEff[1]), InstancedEff[2], m_Char as Player);
                    break;
                //加能量
                case ("ENG"):
                    ENG(int.Parse(InstancedEff[1]), m_Char as Player);
                    break;
                //三倍蓄力
                case ("TBD"):
                    TBD(int.Parse(InstancedEff[1]),m_Char);
                    break;
                //全力一击
                case ("ALLMPATK"):
                    ALLMPATK(m_Char);
                    break;
                //QTE必杀
                case ("ULTI"):
                    ULTI(m_Char, InstancedEff[1]);
                    break;

                case ("CBB"):
                    //可被打断宣言
                    m_Char.isBreakable = true;
                    break;
                case ("VFX"):
                    //出现特效
                    Vfx(InstancedEff[1], InstancedEff[2],m_Char);
                    break;
                case ("AUTO"):
                    //开始自动操作
                    AUTO(InstancedEff[1], m_Char as Player);
                    break;
                default:
                    break;
            }
        }
    }


    public Skill(int mode,string Str)
    {
        if (mode == 0)
        {
            Str = "Data/Player/Skill/" + Str;
            SkillData data = Resources.Load(Str) as SkillData;
            if (!data)
            {
                Debug.Log("这个路径没有SkillData！！" + Str);
                Debug.Break();
            }
            m_name = data._name;
            inputSequence = new List<Note>();
            EffectStr = data.Effect;
            cost = data.cost;
            Icon = data.sprite;
            type = data.type;
            CooldownMax = data.CD;
            Cooldown = 0;
            Desc = data.Desc;
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

        if (mode == 1)
        {
            m_name = "";
            EffectStr = Str;
        }

    }


    public Skill()
    {

    }
    //public UnityEvent EffectEvent;
    //public List<Effect> effects;


    public void ATKPRO(string effstr, Character Char)
    {

        int dDamage = 0;
        bool noAfterattack = false;
        bool isDefenceToDisable = false;
        bool isDefencePenetrate = false;
        string sfxstr = "SLASH";
        string[] InstancedEffstr = effstr.Split('&');
        foreach(string s in InstancedEffstr)
        {
            switch (s.Split(':')[0])
            {
                case ("dmg"):
                    dDamage = int.Parse(s.Split(':')[1]);
                    break;
                case ("NAA"):
                    noAfterattack = true;
                    break;
                case ("IDTD"):
                    isDefenceToDisable = true;
                    break;
                case ("IDP"):
                    isDefencePenetrate = true;
                    break;
                case ("sfx"):
                    sfxstr = s.Split(':')[1];
                    break;

            }
        }
        Char.Hit( dDamage,  noAfterattack ,  isDefenceToDisable,  isDefencePenetrate,  sfxstr);
    }




    public void ATK(int dDamage, Character Char,bool isDefenceToDisable = false)
    {
        Char.Hit(dDamage, isDefenceToDisable);
        //        Debug.Log("ATK "+ dDamage);
    }

    public void ATKmini(int dDamage, Character Char)
    {
        Char.Hit(dDamage,true);
        //        Debug.Log("ATK "+ dDamage);
    }

    public void ATKpene(int dDamage, Character Char)
    {
        Char.Hit(dDamage,false,false,true);
        //        Debug.Log("ATK "+ dDamage);
    }

    public void HEL(int dHeal, Character Char)
    {
        Char.Heal(dHeal);
    }

    public void DEF(Character Char,string str)
    {
//        Debug.Log("DEF");
        Buff_defend defend = new Buff_defend
        {
            m_name = "defend",
            remainBeats = 2,
            //character=Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        //Char.buffs.Add(defend);
        defend.BuffAdded(Char,str);

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
        Char.Hit((Char.Mp/4)*2);
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

    //减CD的技能，skilltype：   
    //attack=2,
    //defend=3,
    //special=4,
    //ulti=5,
    public void MCD(int dCD,string skillTypes,Player player)
    {

        foreach (SkillSlot s in player.skillSlots)
        {
            if (s.skill != null && s.skill.Cooldown > 0)
            {
                if(skillTypes.Contains(((int)(s.skill.type)).ToString()))
                s.skill.Cooldown -= dCD;
                if (s.skill.Cooldown < 0)
                    s.skill.Cooldown = 0;
            }
        }
    }


    //加能量 
    public void ENG(int dMP, Player player)
    {


        player.AddMp(dMP);
    }

    //开始自动行动
    public void AUTO(string str, Player player)
    {
        //>用来分隔每个技能组
        //;代表, 用来分隔组里的每个技能
        //=替代_
        str = str.Replace(';', ',');
        str = str.Replace('=', '_');

        string[] StrSplit = str.Split('>');
        Queue<string> skills = new Queue<string>();
        foreach(string s in StrSplit)
        {
            skills.Enqueue(s);
        }

        player.IntoAutoMode(skills);

    }


    public void Vfx(string str,string vecstr,Character Char)
    {
        string[] vecstrs= vecstr.Split('/');
        Vector3 vector = new Vector3(float.Parse(vecstrs[0]), float.Parse(vecstrs[1]), float.Parse(vecstrs[2]));
        VFX.ShowVFX(str,vector+Char.transform.localPosition);

    }
}
