using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SkillUpgrade
{

    public Sprite sprite;
    public string name;
    public string desc;

    //集中处理技能的升级选择，level是档位，choice是选项序号
    public static void UpgradeSkill(int choice,Skill skill)
    {
        Debug.Log(skill.m_name);
        switch (skill.m_name)
        {
            case "ATTACK":
                Attack(choice, skill);
                break;
            case "DEFEND":
                Defend(choice, skill);
                break;
            case "HEAVY_ATTACK":
                HEAVYATTACK(choice, skill);
                break;
            case "CHARGE_ATTACK":
                CHARGEATTACK(choice, skill);
                break;
            case "STAB":
                STAB(choice, skill);
                break;

            case "CONCENTRATE":
                CONCENTRATE(choice, skill);
                break;
        }
    }





    public static void Attack(int choice, Skill skill)
    {
        //1级 A选项 加一攻击力
        if (choice==11)
        {
            Debug.Log(skill.m_name + " " + skill.EffectStr);

            skill.EffectStr=skill.EffectStr.Replace("ATK_1", "ATK_2");
            skill.upgradeChoice1 = 1;

//            Debug.Log(skill.m_name + " " + skill.EffectStr);
        }

        //2级 A选项 加一攻击力
        if (choice == 21)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_2", "ATK_3");
            skill.upgradeChoice2 = 1;

        }




        //3级 A选项 加2攻击力
        if (choice == 31)
        {
            skill.EffectStr = skill.EffectStr += ",AMWK";
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 减少一点费用
        if (choice == 32)
        {
            skill.cost -= 4;
            skill.upgradeChoice3 = 2;

        }
        

    }

    public static void Defend(int choice, Skill skill)
    {
        //1级 A选项 防住返1费
        if (choice == 11)
        {
            Debug.Log(skill.m_name + " " + skill.EffectStr);

            skill.EffectStr = skill.EffectStr.Replace("DEF", "DEF_ENG$4");
            skill.upgradeChoice1 = 1;

        }

        //2级 A选项 防住返1费
        if (choice == 21)
        {
            skill.EffectStr = skill.EffectStr.Replace("DEF_ENG$4", "DEF_ENG$8");
            skill.upgradeChoice2 = 1;

        }




        //3级 A选项 防住则反弹2伤害
        if (choice == 31)
        {
            skill.EffectStr = skill.EffectStr.Replace("DEF_ENG$8", "DEF_ENG$8;ATK$2");
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 减1费
        if (choice == 32)
        {
            skill.cost -= 4;
            skill.upgradeChoice3 = 2;

        }


    }

    public static void HEAVYATTACK(int choice, Skill skill)
    {
        //1级 A选项 加2攻
        if (choice == 11)
        {
            Debug.Log(skill.m_name + " " + skill.EffectStr);

            skill.EffectStr = skill.EffectStr.Replace("ATK_5", "ATK_7");
            skill.upgradeChoice1 = 1;

        }

        //2级 A选项 减1费
        if (choice == 21)
        {
            skill.cost-=4;
            skill.upgradeChoice2 = 1;

        }


        //3级 A选项 额外攻击2
        if (choice == 31)
        {
            skill.EffectStr += ",ATK_3";
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 变成全力一击
        if (choice == 32)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_7", "ALLMPATK");
            skill.upgradeChoice3 = 2;
        }
    }


    public static void CHARGEATTACK(int choice, Skill skill)
    {
        //1级 A选项 加2攻
        if (choice == 11)
        {

            skill.cost -= 4;
            skill.upgradeChoice1 = 1;
        }

        //2级 A选项 减1费
        if (choice == 21)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK=6", "ATK=10");
            skill.upgradeChoice2 = 1;
        }


        //3级 A选项 蓄力时无敌
        if (choice == 31)
        {
            skill.EffectStr = skill.EffectStr.Replace(",ANI_player-readytoslash", ",ANI_player-readytoslash,DEF");


            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 击晕对手1回合
        if (choice == 32)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_10", "ATK_10,STUN_4");

            skill.upgradeChoice3 = 2;
        }
    }


    public static void STAB(int choice, Skill skill)
    {
        //1级 A选项 加1攻
        if (choice == 11)
        {
            Debug.Log(skill.m_name + " " + skill.EffectStr);

            skill.EffectStr = skill.EffectStr.Replace("ATK_3", "ATK_4");
            skill.upgradeChoice1 = 1;

        }

        //2级 A选项 穿防
        if (choice == 21)
        {
            skill.EffectStr = "STAB_4_0,ANI_player-attack";
            skill.upgradeChoice2 = 1;

        }


        //3级 A选项 连续使用+2攻击
        if (choice == 31)
        {
            skill.EffectStr = "STAB_4_1,ANI_player-attack";
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 破防+5攻击
        if (choice == 32)
        {
            skill.EffectStr = "STAB_4_2,ANI_player-attack";
            skill.upgradeChoice3 = 2;
        }
    }


    public static void CONCENTRATE(int choice, Skill skill)
    {
        //1级 A选项 最多3层
        if (choice == 11)
        {


            skill.EffectStr = "TBD_count:3&multi:2&cancel";
            skill.upgradeChoice1 = 1;

        }

        //2级 A选项 减1费
        if (choice == 21)
        {
            skill.cost-=4;
            skill.upgradeChoice2 = 1;

        }


        //3级 A选项 变成3倍
        if (choice == 31)
        {
            skill.EffectStr = "TBD_count:3&multi:3&cancel";
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项 不会被打断
        if (choice == 32)
        {
            skill.EffectStr = "TBD_count:3&multi:2";
            skill.upgradeChoice3 = 2;
        }
    }
}
