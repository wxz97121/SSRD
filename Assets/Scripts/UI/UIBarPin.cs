using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarPin : MonoBehaviour
{
    public Sprite spr_normal;
    public Sprite spr_locked;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
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
                image.sprite = spr_normal;
                break;
            case 1:
                image.sprite = spr_locked;
                break;
        }
    }
}
