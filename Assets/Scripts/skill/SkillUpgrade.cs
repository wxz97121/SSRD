using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillUpgrade
{

    //集中处理技能的升级选择，level是档位，choice是选项序号
    public static void UpgradeSkill(string skillname,int level,int choice,Skill skill)
    {
        Debug.Log(skill.m_name);
        switch (skillname)
        {
            case "attack":
                Attack(level, choice, skill);
                break;
        }
    }





    static void Attack(int level , int choice, Skill skill)
    {
        //1级 A选项
        if (level==1&&choice==1)
        {
            Debug.Log(skill.m_name + " " + skill.EffectStr);

            skill.EffectStr=skill.EffectStr.Replace("ATK_1", "ATK_2");
            skill.upgradeChoice1 = 1;
            Debug.Log("1111111");
            Debug.Log(skill.m_name + " " + skill.EffectStr);
        }

        //2级 A选项
        if (level == 2 && choice == 1)
        {
            skill.EffectStr = skill.EffectStr.Replace("ATK_2", "ATK_3");
            skill.upgradeChoice2 = 1;

        }

        //2级 B选项
        if (level == 2 && choice == 2)
        {
            //todo:加一钱
            skill.upgradeChoice2 = 2;

        }

        //3级 A选项
        if (level == 3 && choice == 1)
        {
            if (skill.EffectStr.Contains("ATK_2"))
            {
                skill.EffectStr = skill.EffectStr.Replace("ATK_2", "ATK_4");

            }
            else
            {
                skill.EffectStr = skill.EffectStr.Replace("ATK_3", "ATK_5");
            }
            skill.upgradeChoice3 = 1;

        }

        //3级 B选项
        if (level == 3 && choice == 2)
        {
            skill.cost -= 4;
            skill.upgradeChoice3 = 2;

        }


    }



}
