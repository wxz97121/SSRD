using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHPArea : MonoBehaviour
{
    public Image image;
    private int maxHP;
    private int HP;
    private float fill;

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
        if (Player.Instance.Hp != HP)
        {
            Animate(Player.Instance.Hp);

        }
        HP = Player.Instance.Hp;
    }

    public void init()
    {
        maxHP = Player.Instance.maxHp;
        HP = Player.Instance.Hp;
        image.fillAmount= (float)HP / (float)maxHP;
    }

    //血量变化动画
    public void Animate(int hp)
    {
        fill = (float)hp / (float)maxHP;
        image.DOFillAmount(fill, 0.2f).SetEase(Ease.InQuad);
    }
}
