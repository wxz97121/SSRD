using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InputTip : MonoBehaviour
{
    public TextMeshProUGUI uppertext;
    public TextMeshProUGUI colortext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string text,int type=0)
    {
        uppertext.text = text;
        colortext.text = text;
        if (type == 1)
        {
            colortext.colorGradient = new VertexGradient(Color.green, Color.green, new Color(0 / 255, 135 / 255, 25 / 255), new Color(0 / 255, 135 / 255, 25 / 255));
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
