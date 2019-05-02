using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//玩家，角色派生类
public class Player : Character {
    static Player _instance;

    public Animator animator;
    //当前的技能列表
    public List<Skill> skills = new List<Skill>();
    //魂点数 魂等级
    public int soulPoint = 0;
    public int soulLevel = 0;
    //魂到达多少升级
    public int soulMaxPoint = 10;
    //魂的UI元素
    public Image soulPointProgress;
    public Image soulLevelLetter;

    //必杀用的通用Skill
    public Skill UltiSkill;

    //携带金钱数
    public int money=10;
    public TextMeshProUGUI UIMoneyNum;

    public SpecController spec;

    public Transform UIEquip;

    public List<GameObject> enemyList = new List<GameObject>();

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
    }
    // Use this for initialization
    override protected void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        UltiSkill = new Skill();

    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        UpdateSoulUI();
        UIMoneyNum.SetText(money.ToString());
        UpdateEquipUI();
    }
    //控制玩家的输入
    protected override void UpdateInput()
    {
        base.UpdateInput();

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    BarController.Instance.ShowAction_energy(actionType.Collect);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    BarController.Instance.ShowAction_main(actionType.Hit);
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    BarController.Instance.ShowAction_main(actionType.Charge);
        //}
    }

    override public void AddMp(int dMp)
    {
        base.AddMp(dMp);
    }
    //玩家具体的控制
    override public bool Hit(int dDamage)
    {
        //animator.Play("player_slash",0);
        return base.Hit(dDamage);
    }



    public override void BattleStart()
    {
        if (currentWeapon != null) currentWeapon.AddBuffs();
        if (currentArmor != null) currentArmor.AddBuffs();
        if (currentScroll != null) currentScroll.AddBuffs();
    }

    override public void Damage(int dDamage, Character source)
    {
        base.Damage(dDamage, source);
        if(Hp<=0) Die();

        animator.Play("player-damaged", 0);

    }

    override public void Die()
    {
        SuperController.Instance.GameOver();
    }

    public void addSoulPoint(int dSoulPoint){
        soulPoint += dSoulPoint;
        if (soulPoint >= soulMaxPoint)
        {
            if(soulLevel>=2){
                soulPoint = soulMaxPoint;
            }else{
                soulLevel++;
                soulPoint = 0;
            }
        }
    }



    public void decreaseSoulLevel(){
        if(soulLevel>0){
            soulLevel--;
        }
        soulPoint = 0;
    }

    public void UpdateSoulUI(){
        soulPointProgress.fillAmount =((float)soulPoint / (float)soulMaxPoint);
        switch(soulLevel){
            case 0:
                soulLevelLetter.color = Color.black;
                soulPointProgress.color = Color.black;

                break;
            case 1:
                soulLevelLetter.color = Color.yellow;
                soulPointProgress.color = Color.yellow;


                break;
            case 2:
                soulLevelLetter.color = Color.red;
                soulPointProgress.color = Color.red;

                break;
        }
    }

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
        if (currentArmor != null)
        {
            armorUI.Find("Name").GetComponent<TextMeshProUGUI>().text = currentArmor.equipName;
            armorUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = currentArmor.equipDesc;
        }
        else
        {
            armorUI.Find("Name").GetComponent<TextMeshProUGUI>().text = "Armor";
            armorUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = "Description";
        }
        if (currentScroll != null)
        {
            scrollUI.Find("Name").GetComponent<TextMeshProUGUI>().text = currentScroll.equipName;
            scrollUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = currentScroll.equipDesc;
        }
        else
        {
            scrollUI.Find("Name").GetComponent<TextMeshProUGUI>().text = "Scroll";
            scrollUI.Find("Desc").GetComponent<TextMeshProUGUI>().text = "Description";
        }
    }

    public void Equip (Equipment equipment)
    {
        switch (equipment.type)
        {
            case equipType.Armor:
                {
                    currentArmor = equipment;
                }
                break;
            case equipType.Weapon:
                {
                    currentWeapon = equipment;
                }
                break;
            case equipType.Scroll:
                {

                }
                break;
        }
        equipment.OnEquip();
    }

    public void UltiAction(string effstr)
    {
        UltiSkill.CommonEffect(this, effstr);
    }
}
