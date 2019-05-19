using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UIHPArea : MonoBehaviour
{
    public Image image;
    private int maxHP;
    public TextMeshProUGUI hpNum;
    private int HP;
    private float fill;
    public Character chara;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHP == 0)
        {
            
            return;
        }
        //HP发生变化
        if (chara != null) { 
        if (chara.Hp != HP)
        {
            Animate(chara.Hp);
        }
        HP = chara.Hp;
        hpNum.text = HP.ToString();

        }
    }

    public void init()
    {
        if (chara != null)
        {
            maxHP = chara.maxHp;
            HP = chara.Hp;
            image.fillAmount = (float)HP / (float)maxHP;
            hpNum.text = HP.ToString();
        }
    }

    //血量变化动画
    public void Animate(int hp)
    {
        fill = (float)hp / (float)maxHP;
        image.DOFillAmount(fill, 0.2f).SetEase(Ease.InQuad);
    }
}
