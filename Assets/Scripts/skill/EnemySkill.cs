using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class EnemySkill
{
    public string m_name;
    public string EffectStr;

    public void EffectFunction(AI m_Char)
    {

        CommonEffect(m_Char, EffectStr);
    }

    //发动技能
    public void CommonEffect(AI m_Char,string str)
    {
//        Debug.Log("effect string = " + EffectStr);
        string[] EffectStrSplit = str.Split(',');
        //用开头三个大写字母表示功能，后面参数用下划线分割
        //例如 ATK_3 表示暗黑破坏神3
        //例如 HEL_5 表示HTML5
        //例如 BUF_3_4 表示 3号 Buff 持续4回合
        //WRN_ATK_RED_3 下一小节出现攻击3红色的提示
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
                case ("DATKPRO"):
                    if (!m_Char.isBroken)
                    {
                        ATKPRO(InstancedEff[1], m_Char);
                    }
                    else
                    {
                        m_Char.Broken();
                    }
                    break;
                case ("ATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DATK"):
                    ATK(int.Parse(InstancedEff[1]), m_Char,true);
                    break;
                case ("ANI"):
                    ANI(InstancedEff[1], m_Char);
                    break;
                case ("ANIT"):

                    ANI(InstancedEff[1], m_Char,true);
                    break;
                case ("HEL"):
                    HEL(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DHEL"):
                    //可被打断的治疗
                    if (!m_Char.isBroken)
                    {
                        HEL(int.Parse(InstancedEff[1]), m_Char);
                    }
                    else
                    {
                        m_Char.Broken();
                    }
                    break;
                case ("HL"):
                    HL(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("DEF"):
                    DEF(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("CTR"):
                    Counter(InstancedEff[1], m_Char);
                    break;
                case ("WRN"):
                    WRN(InstancedEff[1], InstancedEff[2], int.Parse(InstancedEff[3]), m_Char, InstancedEff[4]);
                    break;
                case ("DMP"):
                    DMP(int.Parse(InstancedEff[1]), m_Char);
                    break;
                case ("SFX"):
                    SFX(InstancedEff[1], m_Char);
                    break;
                case ("TIP"):
                    TIP(InstancedEff[1], int.Parse(InstancedEff[2]));
                    break;
                    //长时间持续的特效，用BUFF控制
                case ("LVFX"):
                    LVFX(InstancedEff[1], int.Parse(InstancedEff[2]),m_Char);
                    break;
                case ("VFX"):
                    //出现特效
                    Vfx(InstancedEff[1], m_Char);
                    break;
                case ("CAM"):
                    //摄像机
                    CAM(InstancedEff[1]);
                    break;
                case ("CBB"):
                    //可被打断宣言,用在第四拍
                    m_Char.isBreakable=true;
                    SoundController.Instance.PlayAudioEffect("Breakable");
                    break;
                default:
                    break;
            }
        }
    }

    public EnemySkill(string DataDir)
    {

    }
    public EnemySkill(int type,string Eff)
    {

        m_name = "";
        EffectStr = Eff;

    }

    public EnemySkill()
    {

    }



    public void ATKPRO(string effstr, Character Char)
    {

        int dDamage = 0;
        bool noAfterattack = false;
        bool isDefenceToDisable = false;
        bool isDefencePenetrate = false;
        string sfxstr = "SLASH";
        string[] InstancedEffstr = effstr.Split('&');
        foreach (string s in InstancedEffstr)
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
        Char.Hit(dDamage, noAfterattack, isDefenceToDisable, isDefencePenetrate, sfxstr);
    }


    private void ATK(int dDamage, Character Char,bool isCounterable= false)
    {
        Char.Hit(dDamage, false, isCounterable); 
        //        Debug.Log("ATK "+ dDamage);
    }

    private void HEL(int dHeal, Character Char)
    {
        Char.Heal(dHeal);
    }

    private void HL(int dHeal, Character Char)
    {
        Char.HealLife(dHeal);
    }

    private void DEF(int beats,Character Char)
    {
//        Debug.Log("DEF");
        Buff_defend defend = new Buff_defend
        {
            m_name = "defend",
            remainBeats = beats,
            //character = Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        defend.BuffAdded(Char,"");
        //Char.buffs.Add(defend);

    }

    private void LVFX(string str, int beats, Character Char)
    {
        Buff_playVFX buff_PlayVFX = new Buff_playVFX
        {
            m_name = "Lvfx",
            remainBeats = beats,
            //character = Char,
            activateTime = RhythmController.Instance.songPosInBeats
        };
        buff_PlayVFX.BuffAdded(Char, str);

    }

    private void ANI(string aniname, AI Char,bool IsSetTrigger=false)
    {
        //                Debug.Log(aniname);
        //if (aniname == "attack") Debug.Log("Attack?????");
        if (IsSetTrigger)
        {
            for(int i = 0; i < Char.animator.parameterCount; i++)
            {

                Char.animator.ResetTrigger(Char.animator.GetParameter(i).name);

            }

            Char.animator.SetTrigger(aniname);
        }
        else
        {
            Char.animator.Play(aniname, -1);

        }

    }


    //图标提示
    private void WRN(string spr,string col, int num, AI Char,string isNextbar)
    {
        Color color=Color.black;
        if (col == "RED")
        {
            color = Color.red;
        }
        if (isNextbar=="pre") { 
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.gameObject.transform.localScale =new Vector3 (1,1,1);

        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetSprite(spr);
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetText(num.ToString());
        UIBarController.Instance.preBar.GetComponent<UIBar>().enemyWarn.SetColor(color);
        }
        else
        {
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.gameObject.transform.localScale = new Vector3(1, 1, 1);

            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetSprite(spr);
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetText(num.ToString());
            UIBarController.Instance.playingBar.GetComponent<UIBar>().enemyWarn.SetColor(color);
        }

    }

    //吸收能力
    private void DMP(int energy,AI Char)
    {
        if (energy <= 0)
        {
            Char.mTarget.GetComponent<Character>().Mp = 0;

        }
        else
        {
            Char.mTarget.GetComponent<Character>().Mp -= energy;
            if (Char.mTarget.GetComponent<Character>().Mp < 0)
            {
                Char.mTarget.GetComponent<Character>().Mp = 0;

            }
        }
    }

    private void SFX(string name, AI Char)
    {
        SoundController.Instance.PlayAudioEffect(name);
    }

    private void TIP(string str, int type)
    {
        SuperController.Instance.ShowInputTip(str, type);
    }

    private void CAM(string str)
    {
        if (str == "DShade")
        {
            SuperController.Instance.BgMaskTransition();
        }
    }

    //当身技能
    private void Counter(string str,AI Char)
    {
        Buff_counter counter = new Buff_counter
        {
            m_name = "counter",
            remainBeats = 2,
            activateTime = RhythmController.Instance.songPosInBeats
            
        };
        counter.BuffAdded(Char, str);
    }


    public void Vfx(string str,  Character Char)
    {
        string name = "";
        int father = 0;
        Vector3 pos = new Vector3(0,0,0);
        string[] vfxstrs = str.Split('&');
        Vector3 scale = new Vector3(1f, 1f, 1f);

        GameObject VFXGO=null;
        foreach (string s in vfxstrs)
        {
            switch (s.Split(':')[0])
            {
                case ("name"):
                    name = s.Split(':')[1];
                    break;
                case ("father"):
                    father =int.Parse(s.Split(':')[1]);
                    break;
                case ("pos"):
                    string[] posstrs = s.Split(':')[1].Split('/');
                    pos = new Vector3(float.Parse(posstrs[0]), float.Parse(posstrs[1]), float.Parse(posstrs[2]));
                    break;
                case ("scale"):
                    string[] sclstrs = s.Split(':')[1].Split('/');
                    scale = new Vector3(float.Parse(sclstrs[0]), float.Parse(sclstrs[1]), float.Parse(sclstrs[2]));

                    break;
            }

        }
        if (father == 0)
        {
             VFXGO = VFX.ShowVFX(name, pos);

        }
        if(father == 1)
        {
             VFXGO = VFX.ShowVFX(name, pos,Char);

        }
        if (father == 2)
        {
             VFXGO = VFX.ShowVFX(name, pos, Char.mTarget.GetComponent<Character>());

        }
        VFXGO.transform.localScale = new Vector3(VFXGO.transform.localScale.x*scale.x, VFXGO.transform.localScale.y * scale.y, VFXGO.transform.localScale.z * scale.z);
    }
}
