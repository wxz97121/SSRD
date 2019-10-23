using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UILifeArea : MonoBehaviour
{
    public Image image;
    public int maxLife;
    public TextMeshProUGUI lifeNum;
    public int Life;
    public float fill;
    public Character chara;
    public Image delayimage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxLife == 0)
        {
            
            return;
        }
        //HP发生变化
        if (chara != null) { 
        if (chara.life != Life)
        {
            Animate(chara.life);
                StartCoroutine(IDelayAnimate(chara.life/(float)chara.maxLife));
        }
        Life = chara.life;
        lifeNum.text = Life.ToString() + "/" + maxLife.ToString();

        }
    }

    public void init(Character character)
    {
        chara = character;
        if (chara != null)
        {
            maxLife = chara.maxLife;
            Life = chara.life;
            image.fillAmount = Life / (float)maxLife;
            lifeNum.text = Life.ToString()+"/"+maxLife.ToString();
        }
    }

    //血量变化动画
    public void Animate(int hp)
    {
        fill = (float)hp / (float)maxLife;
        image.DOFillAmount(fill, 0.2f).SetEase(Ease.InQuad);
    }

    IEnumerator IDelayAnimate(float _fill)
    {
        yield return new WaitForSeconds(.5f);
        delayimage.DOFillAmount(_fill, 0.2f).SetEase(Ease.InQuad);
    }
}
