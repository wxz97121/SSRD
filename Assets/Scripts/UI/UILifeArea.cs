using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UILifeArea : MonoBehaviour
{
    public Image container;
    public Image image;
    public int maxLife;
    public TextMeshProUGUI lifeNum;
    public int Life;
    float sizePerLife = 60f - 44.84f;

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
            float sizeX = container.GetComponent<RectTransform>().sizeDelta.x;
            float sizeY = container.GetComponent<RectTransform>().sizeDelta.y;


            container.GetComponent<RectTransform>().sizeDelta = maxLife < 50 ? new Vector2(sizeX + (sizePerLife * (maxLife - 1)), sizeY) : new Vector2(sizeX + (sizePerLife * (50 - 1)), sizeY);

            image.fillAmount = Life / (float)maxLife;
            delayimage.fillAmount= Life / (float)maxLife;
            lifeNum.text = Life.ToString()+"/"+maxLife.ToString();
        }
    }

    //血量变化动画
    public void Animate(int lf)
    {
        fill = (float)lf / (float)maxLife;
        image.DOFillAmount(fill, 0.1f).SetEase(Ease.InQuad);
    }

    IEnumerator IDelayAnimate(float _fill)
    {
        yield return new WaitForSeconds(.5f);
        delayimage.DOFillAmount(_fill, 0.2f).SetEase(Ease.InQuad);
    }
}
