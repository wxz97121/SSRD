using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UISoulArea : MonoBehaviour
{
    public Image image;
    public int maxSoul;
    public int Soul;
    public float fill;
    public Character chara;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxSoul == 0|| chara == null)
        {
            
            return;
        }
        //魂发生变化

        if (chara.soulPoint != Soul)
        {
            Animate(chara.soulPoint);
        }
        Soul = chara.soulPoint;

        
    }

    public void init(Character character)
    {
        chara = character;
        if (chara != null)
        {
            maxSoul = chara.soulMaxPoint;
            gameObject.SetActive(maxSoul != 0);

            Soul = chara.soulPoint;
            image.fillAmount = (float)Soul / (float)maxSoul;
        }
    }

    //血量变化动画
    public void Animate(int soul)
    {
        fill = (float)soul / (float)maxSoul;
        image.DOFillAmount(fill, 0.2f).SetEase(Ease.InQuad);
    }

    //已经满了的能量格子闪烁
    public void Blink()
    {
        if (Soul >= maxSoul && Soul > 0)
        {
            transform.GetChild(2).GetComponent<Image>().color = new Color(1,1, 1, 1);
            transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1, 1);


            transform.GetChild(2).GetComponent<Image>().DOFade(0, 0.4f).SetEase(Ease.InQuad);
            transform.GetChild(3).GetComponent<Image>().DOFade(0, 0.4f).SetEase(Ease.InQuad);

        }
    }
}
