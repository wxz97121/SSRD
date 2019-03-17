using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum BubbleSprType
{
    empty,
    hp,
    mp

}
public class Bubble : MonoBehaviour
{
    public Sprite spr_hp;
    public Sprite spr_mp;
    public TextMeshPro text;
    public SpriteRenderer sprite;


    public static void AddBubble(BubbleSprType t,string str,Character c)
    {
        GameObject ins=Instantiate(Resources.Load("Prefab/bubble"), c.transform.position, Quaternion.identity) as GameObject;
        var bub = ins.GetComponent<Bubble>();
        bub.sprite = bub.gameObject.GetComponentInChildren<SpriteRenderer>();
        bub.text.text = str;
        switch (t)
        {
            case 0:
                break;
            case BubbleSprType.hp:
                bub.sprite.sprite = bub.spr_hp;
                break;
            case BubbleSprType.mp:
                bub.sprite.sprite = bub.spr_mp;

                break;
        }
    }

    // Update is called once per frame
    void Start()
    {
        transform.DOLocalMove((transform.localPosition+new Vector3(0,1,0)),1,false);
        sprite.DOFade(0, 1);
        text.DOFade(0, 1);
        Destroy(gameObject, 1);
    }
}
