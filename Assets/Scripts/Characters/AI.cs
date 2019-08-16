using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//敌人AI，角色派生类
public class AI : Character
{
    public string m_name;
    public AIData data;
    public string lastSkill;

    //技能组字典
    public Dictionary<string, EnemySkillGroup> _skillGroupDict;

    //敌人当前的动作序号
    public int actionID = 0;

    //当前的技能列表
    public List<EnemySkillGroup> skillGroupSeq;
    //技能实例
    protected EnemySkill enemySkill;
    public Animator animator;



    //敌人的初始位置(基本不用动，多人可能有用)
    public Vector3 originPosition = new Vector3(0, 0, 0);

    //死亡后掉落金钱数
    public int lootMoney = 1;
    //已经死亡，等着死亡动画，不再行动
    public bool isReadyToDie=false;



    // Use this for initialization
    override protected void Start()
    {
        //默认初始化
        base.Start();

    }

    public virtual void Init()
    {
        isPlayer = false;
        _skillGroupDict = new Dictionary<string, EnemySkillGroup>();
        //读取技能组表,并存入字典
        //skillGroups = data.enemySkillGroups;
        foreach (EnemySkillGroup item in data.enemySkillGroups)
        {
            _skillGroupDict.Add(item.name, item);
        }

        //初始化动作序号,下一拍到0
        actionID = 0;
        //初始化敌人目标，默认是玩家
        mTarget = GameObject.Find("Player");
        enemySkill = new EnemySkill();

        animator = GetComponent<Animator>();

        skillGroupSeq = new List<EnemySkillGroup>();
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
                    isReadyToDie = true;
                    return;
                }
            }
        }
    }


    virtual public void QTEAction(string effectstr)
    {
        enemySkill.CommonEffect(this, effectstr);
    }



    //从技能组表中读技能
    public string GetNextSkill(int beatnum)
    {
        if (skillGroupSeq.Count == 0)
        {
            return "EMT";
        }
        if (beatnum == 4)
        {
            return skillGroupSeq[0].enemySkills[0];

        }
        else if(beatnum==3)
        {
            string str= skillGroupSeq[0].enemySkills[beatnum];
            lastSkill = skillGroupSeq[0].name;
            skillGroupSeq.RemoveAt(0);
            return str;

        }
        else
        {
            return skillGroupSeq[0].enemySkills[beatnum];
        }
        //4-0  1-2  2-3  3-4
    }


    //发动技能
    override public  void CastSkill(string str)
    {
        base.CastSkill(str);
        enemySkill.CommonEffect(this, str);
    }

    //在队列中增加一个技能组
    public void SGSAdd(string skillgroupname)
    {
        skillGroupSeq.Add(_skillGroupDict[skillgroupname]);
    }

    //在队列中插入一个技能组
    public void SGSInsert(string skillgroupname)
    {
        skillGroupSeq.Insert(0,_skillGroupDict[skillgroupname]);
    }

    //在队列中删除一个技能组
    public void SGSDelete(int index)
    {
        skillGroupSeq.RemoveAt(index);
    }

}
