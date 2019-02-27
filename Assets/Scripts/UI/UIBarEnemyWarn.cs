using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBarEnemyWarn : MonoBehaviour
{
    public Sprite spr_atk;
    public Sprite spr_def;
    public Image image;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
      //  image = gameObject.GetComponent<Image>();
      //  text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //切换指针显示类型

    public void SetSprite(string type)
    {
        switch (type)
        {
            case "ATK":
                image.sprite = spr_atk;
                break;
            case "DEF":
                image.sprite = spr_def;
                break;
        }
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void SetAlpha(float alf)
    {
        image.color = new Color(255, 255, 255, alf);
        text.faceColor = new Color(text.faceColor.r, text.faceColor.g, text.faceColor.b, alf);

    }

    public void SetColor(Color color)
    {
        text.faceColor = new Color(color.r, color.g, color.b, text.faceColor.a);

    }
}
