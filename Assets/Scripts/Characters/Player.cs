using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//技能槽
public struct SkillSlot
{
    public Skill skill;
    public SkillType requiredType;

}

//玩家，角色派生类
public class Player : Character {
    static Player _instance;

    public Animator animator;
    //当前的技能列表
    public SkillSlot[] skillSlots=new SkillSlot[5];

    //自动模式
    public bool automode = false;
    public Queue<string> autoSkills;


    //必杀用的通用Skill
    public Skill commonSkill;

    //携带金钱数
    public int money=10;
    public TextMeshProUGUI UIMoneyNum;


    //当前剩余技能点
    public int NotePower = 0;

    public SpecController spec;

    public Transform UIEquip;

    public List<GameObject> enemyList = new List<GameObject>();

    //装备列表
    public List<Equipment> equipmentList;
    //当前装备着的武器
    public Equipment currentWeapon;
    //当前装备着的护甲
    public Equipment currentCloth;
    //当前装备着的卷轴
    public Equipment currentAmulet;
    public List<Skill> skillList;
 

    private void Awake()
    {
        _instance = this;
    }

    public static Player Instance
    {
        get
        {
            return _instance;
        }
    }
    public void Reset()
    {
        Hp = maxHp;
        Mp = maxMp;
        life = maxLife;
        spec.Start();
        if (currentWeapon != null) currentWeapon.AddBuffs();
        if (currentCloth != null) currentCloth.AddBuffs();
        if (currentAmulet != null) currentAmulet.AddBuffs();
    }
    // Use this for initialization
    override protected void Start () {
        base.Start();
        isPlayer = true;

        skillSlots[0].requiredType = SkillType.attack;
        skillSlots[1].requiredType = SkillType.defend;
        skillSlots[2].requiredType = SkillType.special;
        skillSlots[3].requiredType = SkillType.special;
        skillSlots[4].requiredType = SkillType.ulti;
        skillSlots[0].skill = null;
        skillSlots[1].skill = null;
        skillSlots[2].skill = null;
        skillSlots[3].skill = null;
        skillSlots[4].skill = null;
        animator = GetComponent<Animator>();

        //范用技能，强行发招用
        commonSkill = new Skill();

        autoSkills = new Queue<string>();

        skillList = new List<Skill>();



        //测试
        AddSkill("testSkill_00X_ATTACK");
        EquipSkill(0, skillList[0]);
//        Debug.Log(skillSlots[0].skill);
        AddSkill("testSkill_0ZX_DEFEND");
        EquipSkill(1, skillList[1]);

        //AddSkill("testSkill_0ZX_ACCDEFEND");


        //AddSkill("SUPERSUPERATTACK");
        //AddSkill("PIERCEATTACK1");

        //AddEquip("Jacket");
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        UIMoneyNum.SetText(money.ToString());
        UpdateEquipUI();
    }


    override public void AddMp(int dMp)
    {
        base.AddMp(dMp);
    }


    public void AddMoney(int dM)
    {
        money += dM;
        Bubble.AddBubble(BubbleSprType.money, "+"+dM.ToString(), this, true);
    }


    public override void BattleStart()
    {
        Reset();

    }

    override public void Damage(int dDamage, Character source)
    {
        base.Damage(dDamage, source);
        if (life <= 0) Die();

    }

    override public void Die()
    {
        SuperController.Instance.GameOver();
    }


    #region 更新技能CD 现在没啥用了
    public void UpdateCDs()
    {
        foreach(SkillSlot s in skillSlots)
        {
            if (s.skill != null)
            {
                if (s.skill.Cooldown > 0)
                {
                    s.skill.Cooldown--;
                }
            }
        }
        //更新技能CD的UI显示
        SuperController.Instance.skillTipBarController.UpdateCDs();
    }
    #endregion


    #region 自动模式(蓄力或者硬直专用)
    public void IntoAutoMode(Queue<string> skills)
    {
        automode = true;
        autoSkills.Clear();
        autoSkills = skills;
        UIBarController.Instance.ShowNoInput();
    }

    public void LeaveAutoMode()
    {
        automode = false;
        autoSkills.Clear();
        UIBarController.Instance.HideNoInput();
    }

    public void CastAutoSkill()
    {

        CastSkill(autoSkills.Dequeue());
        if (autoSkills.Count == 0)
        {
            LeaveAutoMode();
        }

    }
    #endregion

    public void UpdateEquipUI()
    {
        var weaponUI = UIEquip.Find("Weapon");
        var armorUI = UIEquip.Find("Armor");
        var scrollUI = UIEquip.Find("Scroll");
        if (currentWeapon != null)
        {
            weaponUI.Find("Name").GetComponent<TextMeshProUGUI>().text = currentWeapon.equipName;
            weaponUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = currentWeapon.equipDesc;
        }
        else
        {
            weaponUI.Find("Name").GetComponent<TextMeshProUGUI>().text = "Weapon";
            weaponUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = "Description";
        }
        if (currentCloth != null)
        {
            armorUI.Find("Name").GetComponent<TextMeshProUGUI>().text = currentCloth.equipName;
            armorUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = currentCloth.equipDesc;
        }
        else
        {
            armorUI.Find("Name").GetComponent<TextMeshProUGUI>().text = "Armor";
            armorUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = "Description";
        }
        if (currentAmulet != null)
        {
            scrollUI.Find("Name").GetComponent<TextMeshProUGUI>().text = currentAmulet.equipName;
            scrollUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = currentAmulet.equipDesc;
        }
        else
        {
            scrollUI.Find("Name").GetComponent<TextMeshProUGUI>().text = "Scroll";
            scrollUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = "Description";
        }
    }

    public void EquipEquipment (Equipment equipment)
    {
        switch (equipment.type)
        {
            case equipType.Weapon:
                {
                    if (currentWeapon != null)
                    {
                        currentWeapon.isEquiped = false;
                    }
                    currentWeapon = equipment;
                }
                break;
            case equipType.Cloth:
                {
                    if (currentCloth != null)
                    {
                        currentCloth.isEquiped = false;
                    }
                    currentCloth = equipment;
                }
                break;
            case equipType.Amulet:
                {
                    if (currentAmulet != null)
                    {
                        currentAmulet.isEquiped = false;
                    }
                    currentAmulet = equipment;
                }
                break;
        }
        equipment.isEquiped = true;
    }

   override public void CastSkill(string str)
    {
        base.CastSkill(str);
        commonSkill.CommonEffect(this, str);
    }

    public void UltiAction(string effstr)
    {
        commonSkill.CommonEffect(this, effstr);
    }

    public override int getCurrentATK()
    {
        int mATK = ATK;
        if (currentWeapon)
            mATK += currentWeapon.ATK;
        if (currentCloth)
            mATK += currentCloth.ATK;
        if (currentAmulet)
            mATK += currentAmulet.ATK;
        return mATK;
    }

    public override int getCurrentDEF()
    {
        int mDEF = DEF;
        if (currentWeapon)
            mDEF += currentWeapon.DEF;
        if (currentCloth)
            mDEF += currentCloth.DEF;
        if (currentAmulet)
            mDEF += currentAmulet.DEF;
        return mDEF;
    }
    public void AddSkill(SkillData newSkillData)
    {
        Skill skill = new Skill(newSkillData);
        skillList.Add(skill);
    }
    public void AddSkill(string newSkillData)
    {
        Skill skill = new Skill(0,newSkillData);
        skillList.Add(skill);
    }

    public void AddEquip(string equipdata)
    {
        Equipment equip = Resources.Load<Equipment>("Data/Equipment/"+equipdata);
        equipmentList.Add(equip);
    }

    public void AddEquip(Equipment equipdata)
    {
        equipmentList.Add(equipdata);
    }

    public void EquipSkill(int slotID,Skill skill)
    {
        //如果是已装备的技能，先卸下来
        if (skill.isEquiped)
        {
            for(int i = 0; i < skillSlots.Length; i++)
            {
                if (skillSlots[i].skill == skill)
                {
                    skillSlots[i].skill = null;
                }
            }
        }


        if (skillSlots[slotID].skill == null)
        {

            skillSlots[slotID].skill = skill;
            skill.isEquiped = true;
//            Debug.Log("new : "+ skill.m_name);
        }
        else
        {

            skillSlots[slotID].skill.isEquiped = false;
            skillSlots[slotID].skill = skill;
            skill.isEquiped = true;
        }
    }


}







