using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//战斗逻辑
public class DuelController : MonoBehaviour {

    public List<string> enemyList;
    //敌人总数
    public int enemyNum = 0;
    //当前敌人序号
    public int enemyIndex = 0;

    public AI nowAI = null;

    //记录玩家在行动拍是否输入过，其实是个锁
    public bool isActedAt3rdBeat = false;

    //ui控制
    public Transform UICanvas;

    //敌人的ui模板
    public GameObject enemyUIPrefab;

    //第一顺序技能效果
    private List<string> s1;

    private List<string> s2;

    private List<string> s3;

    private Skill playerSkill;



    private EnemySkill enemySkill;


    #region 单例
    static DuelController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static DuelController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
        UICanvas = GameObject.Find("Canvas").transform;
        s1 = new List<string> { "DEF", "ULTI","-CD","AUTO","CTR"};
        s2 = new List<string> { "ATK", "HEL", "DMP","ATKmini","ALLMPATK","HL","ATKPRO" };
        s3 = new List<string> { "DHEL", "TBD",  "ANI", "ANIT", "SFX" ,"VFX","LVFX","DATK","CBB","DATKPRO"};
        enemySkill = new EnemySkill();
        playerSkill = new Skill();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public AI GetCurAI()
    {
        if (Player.Instance.mTarget)
            nowAI = (Player.Instance.mTarget.GetComponent<AI>()) as AI;
        return nowAI;
    }


    public void EnemyRespawn()
    {
        if (Player.Instance.enemyList.Count == 0)
        {
            if (enemyIndex >= enemyList.Count)
            {
                SuperController.Instance.Win();
                return;
            }
            Player.Instance.Heal(Player.Instance.maxHp- Player.Instance.Hp);
            AddEnemy(enemyIndex);
            enemyIndex++;
        }

    }


    #region 增加敌人
    public void AddEnemy(int enemyIndex)
    {

        GameObject instEnemy = Instantiate((GameObject)Resources.Load("Data/AI/" + enemyList[enemyIndex] + "/Prefab", typeof(GameObject)), GameObject.Find("EnemyGroup").transform);


        SuperController.Instance.enemyBattleInfo.init(instEnemy.GetComponent<AI>() as Character);
        instEnemy.GetComponent<AI>().m_name = enemyList[enemyIndex];
        instEnemy.GetComponent<AI>().Init();

        Player.Instance.enemyList.Add(instEnemy);
        Player.Instance.mTarget = instEnemy;
        //enemyIndex = (enemyIndex + 1) % enemyList.Count;
    }


    #endregion 增加敌人


    #region 清除敌人
    public void ClearEnemy()
    {
        enemyIndex = 0;
        for (int i = Player.Instance.enemyList.Count - 1; i >= 0; i--)
        {
            var enemy = Player.Instance.enemyList[i];
            enemy.GetComponent<AI>().Die();
        }
    }
    #endregion

    public void SkillJudge(string playerSkillstring,string enemySkillstring)
    {
        nowAI = GetCurAI();

        //分解敌人技能STRING
        string[] sEnemyEffStrSplit = enemySkillstring.Split(',');
        //分解玩家技能STRING
        string[] sPlayerEffStrSplit = playerSkillstring.Split(',');

        string E1 = "";
        string E2 = "";
        string E3 = "";
        string P1 = "";
        string P2 = "";
        string P3 = "";


        //把各种效果的STRING分类
        foreach (string s in s1) 
        {
            foreach(string e in sEnemyEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    E1 += e;
                    E1 += ",";
                }
            }


            foreach (string e in sPlayerEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    P1 += e;
                    P1 += ",";
                }
            }

        }

        foreach (string s in s2)
        {
            foreach (string e in sEnemyEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    E2 += e;
                    E2 += ",";
                }
            }

            foreach (string e in sPlayerEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    P2 += e;
                    P2 += ",";
                }
            }

        }

        foreach (string s in s3)
        {
            foreach (string e in sEnemyEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    E3 += e;
                    E3 += ",";
                }
            }

            foreach (string e in sPlayerEffStrSplit)
            {
                string[] InstancedEff = e.Split('_');

                if (InstancedEff[0] == s)
                {
                    P3 += e;
                    P3 += ",";
                }
            }

        }
        //        Debug.Log("E1 = " + E1 + "E2 = " + E2 + "E3 = " + E3);

        //        Debug.Log("p1 = " + P1+ "p2 = " + P2+ "p3 = " + P3);


        //依次触发技能效果
        if (nowAI != null)
        {
            if(!nowAI.isBroken)enemySkill.CommonEffect(nowAI, E1);
            if (!Player.Instance.isBroken) playerSkill.CommonEffect(Player.Instance, P1);
            if (!nowAI.isBroken) enemySkill.CommonEffect(nowAI, E2);
            if (!Player.Instance.isBroken) playerSkill.CommonEffect(Player.Instance, P2);
            if (!nowAI.isBroken) enemySkill.CommonEffect(nowAI, E3);
            if (!Player.Instance.isBroken) playerSkill.CommonEffect(Player.Instance, P3);
        }

        //更新技能CD
        //Player.Instance.UpdateCDs();
        SuperController.Instance.skillTipBarController.UpdateCDs();
        //本小节行动过，上锁
        isActedAt3rdBeat = true;

        //重置双方打断状态
        Player.Instance.isBreakable = false;
        Player.Instance.isBroken = false;
        nowAI.isBreakable = false;
        nowAI.isBroken = false;

        //Buff Effect
        if (playerSkillstring == "")
        {
            foreach (var buff in Player.Instance.buffs)
            {
                buff.AfterNoAction();
            }
        }

    }
}
