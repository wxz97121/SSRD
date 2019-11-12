using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum BubbleSprType
{
    empty,
    hp,
    mp,
    life,
    money

}
public class Bubble : MonoBehaviour
{
    public Sprite spr_hp;
    public Sprite spr_mp;
    public Sprite spr_life;
    public Sprite spr_money;


    public TextMeshPro text;
    public SpriteRenderer sprite;


    public static void AddBubble(BubbleSprType t,string str,Character c,bool add=false)
    {
        GameObject ins=Instantiate(Resources.Load("Prefab/bubble"), c.transform.position, Quaternion.identity) as GameObject;
        var bub = ins.GetComponent<Bubble>();
        bub.sprite = bub.gameObject.GetComponentInChildren<SpriteRenderer>();
        bub.text.text = str;
        if (add)
        {
            bub.text.fontMaterial= Resources.Load("Fonts & Materials/LiberationSans SDF - Bubble - green", typeof(Material)) as Material;
            bub.text.colorGradient = new VertexGradient(Color.white, Color.white, new Color(254f/255f, 173f/255f, 166f/255f,1f), new Color(254f / 255f, 173f / 255f, 166f / 255f, 1f));
        }
        else
        {
            bub.text.fontMaterial = Resources.Load("Fonts & Materials/LiberationSans SDF - Bubble - red", typeof(Material)) as Material;

            bub.text.colorGradient = new VertexGradient(Color.white, Color.white, new Color(60f/255f, 168f / 255f, 146f / 255f, 1f), new Color(60f / 255f, 168f / 255f, 146f / 255f, 1f));

        }
        switch (t)
        {
            case 0:
                break;
            case BubbleSprType.hp:
                bub.sprite.sprite = bub.spr_hp;
                bub.sprite.color = new Color(0f/255f, 190f/255f, 76f/255f,1f);
                break;
            case BubbleSprType.mp:
                bub.sprite.sprite = bub.spr_mp;
                bub.sprite.color = new Color(238f / 255f, 172f / 255f, 87f / 255f,1f);


                break;
            case BubbleSprType.life:
                bub.sprite.sprite = bub.spr_life;
                bub.sprite.color = new Color(255f / 255f, 76f / 255f, 76f / 255f,1f);


                break;
            case BubbleSprType.money:
                bub.sprite.sprite = bub.spr_money;
                bub.sprite.color = Color.white;


                break;
        }
    }

    // Update is called once per frame
    void Start()
    {
        System.Random rng = new System.Random(GetHashCode());
        transform.DOLocalMove((transform.localPosition+new Vector3(0,-10,0)),1,false).SetEase(Ease.InBack);
        transform.DOLocalMoveX(transform.localPosition.x+(float)(rng.Next(0,10)-5),1f).SetEase(Ease.InOutQuad);
       //sprite.DOFade(0, 0.5).SetEase(Ease.InOutQuad);
       //text.DOFade(0, 0.5).SetEase(Ease.InOutQuad);
        Destroy(gameObject, 1);
        //test
    }
}
