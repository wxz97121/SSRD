using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarPin : MonoBehaviour
{
    public Color normalcolor;
    public Color lockedcolor;

    //public Sprite spr_normal;
    //public Sprite spr_locked;
    public Image imageup;
    public Image imagedown;

    private Color curColor;

    // Start is called before the first frame update
    void Start()
    {
        curColor = normalcolor; 
   }

    // Update is called once per frame
    void Update()
    {
        
    }


    //切换指针显示类型
    //0=默认
    //1=锁定
    public void SetSprite(int type)
    {
        switch (type)
        {
            case 0:
                imageup.color = normalcolor;
                imagedown.color = normalcolor;
                curColor = normalcolor;

                break;
            case 1:
                imageup.color = lockedcolor;
                imagedown.color = lockedcolor;
                curColor = lockedcolor;
                break;
        }
    }

    public void SetAlpha(float alpha)
    {
        imageup.color = new Color(curColor.r, curColor.g, curColor.b,  alpha);
        imagedown.color = new Color(curColor.r, curColor.g, curColor.b,  alpha);

    }

}
