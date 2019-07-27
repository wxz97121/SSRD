using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_SkillIcon : MonoBehaviour
{
    //UI元件
    public Image icon;
    public TextMeshProUGUI cdtext;
    public TextMeshProUGUI costtext;
    public Image mask;
    

    //技能
    public Skill skill;

    // Start is called before the first frame update
    public void Init()
    {
        Debug.Log("skill name = " + skill.m_name);
        icon.sprite = skill.Icon;
        if (skill.cost == -1)
        {
            costtext.text = "ALL";
        }
        else
        {
            costtext.text = skill.cost.ToString();
        }
        UpdateCD();
    }

    // Update is called once per frame
    public void UpdateCD()
    {
        //这里技能的CD是1的时候，实际上是没有CD的
        if (skill.Cooldown == 0)
        {
            mask.gameObject.SetActive(false);
        }else{
            mask.gameObject.SetActive(true);
            cdtext.gameObject.SetActive(true);

            cdtext.text = (skill.Cooldown).ToString();
        }
    }
}
