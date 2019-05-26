﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能组，4个一组，用来存敌人每回合的动作
public struct EnemySkillGroup
{
    string name;
    string[] enemySkills;
}

//敌人AI，角色派生类
public class AI : Character
{
    public string m_name;
    public AIData data;

    public Dictionary<string, EnemySkill> _skillDictionary;

    //敌人当前的动作序号(在序列的哪一拍)
    public int actionID = 0;

    //当前的技能列表
    public List<string> skillSequence = new List<string>();
    private List<EnemySkillGroup> skillGroups; 
    public Animator animator;



    //敌人的初始位置(基本不用动，多人可能有用)
    public Vector3 originPosition = new Vector3(0, 0, 0);

    //死亡后掉落金钱数
    public int lootMoney = 1;



    // Use this for initialization
    override protected void Start()
    {
        //默认初始化
        base.Start();
    }

    public virtual void Init()
    {
        _skillDictionary = new Dictionary<string, EnemySkill>();

        EnemySkillData[] skillArray = Resources.LoadAll<EnemySkillData>("Data/AI/"+m_name+"/Skill");

        //读取文件夹中的所有技能，存入字典
        foreach (EnemySkillData item in skillArray)
        {
            EnemySkill _skill = new EnemySkill("Data/AI/" + m_name + "/Skill/" + item.name);
            _skillDictionary.Add(_skill.m_name, _skill);
            Debug.Log("add skill, name:" + _skill.m_name);
        }

        //读取技能顺序表
        skillSequence = data.actionSequence;
//        Debug.Log("skillSequence:"+ skillSequence);

        //初始化动作序号,下一拍到0
        actionID = 0;
        //初始化敌人目标，默认是玩家
        mTarget = GameObject.Find("Player");


        animator = GetComponent<Animator>();


        //初始化敌人模型位置
        originPosition = transform.position;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    //敌人死亡的时候从玩家的目标列表中去除，然后销毁模型和ui
    override public void Die()
    {

        gameObject.AddComponent<VFX>();
        VFX vfx = gameObject.GetComponent<VFX>();
//        Destroy(UIHpNum.transform.parent.gameObject);

        Player.Instance.enemyList.Remove(gameObject);
        vfx.StartCoroutine("FadeOut");
        Player.Instance.money += lootMoney;
        UIBarController.Instance.ClearBarWarn();

        Debug.Log("Enemy Die");

    }



   
    override public void Damage(int dDamage, Character source)
    {
        base.Damage(dDamage, source);
    }


    virtual public void Action(int beatnum)
    {
        //怪物的死亡放在第一拍
        if (life <= 0)
        {
            if (!isUndead)
            {
                if (SoundController.Instance.timelineInfo.currentMusicBeat == 1)
                {
                    Die();
                }
                else
                {
                    //如果不是第一拍且死了，就是暂时不动
                    return;
                }
            }
        }
    }

    virtual public void Action()
    {
        //怪物的死亡放在第一拍
        if (life<=0)
        {
            if (!isUndead)
            {
                if (SoundController.Instance.timelineInfo.currentMusicBeat == 1)
                {
                    Die();
                }
                else
                {
                    //如果不是第一拍且死了，就是暂时不动
                    return;
                }
            }
        }
    }

    virtual public void QTEAction(string actionname)
    {

    }




    public string GetNextSkill()
    {
        if (skillSequence.Count == 0)
        {
            return "EMT";
        }
//        Debug.Log("skill name =" + skillSequence[actionID]);
        return _skillDictionary[skillSequence[actionID]].EffectStr;
    }






}
