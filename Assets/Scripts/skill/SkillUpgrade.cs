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
            Debug.Log("1111111");
            Debug.Log(skill.m_name + " " + skill.EffectStr);
        }

        //2级 A选项 加一攻击力
        if (choice == 21)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_2", "ATK_3");
            skill.upgradeChoice2 = 1;

        }

        //2级 B选项 还是加一攻击力
        if (choice == 22)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_2", "ATK_3");
            skill.upgradeChoice2 = 2;

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



}
