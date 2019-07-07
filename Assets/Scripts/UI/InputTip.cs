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

    public void Init(string text)
    {
        uppertext.text = text;
        colortext.text = text;

    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
