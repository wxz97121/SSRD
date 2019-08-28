using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VisualNovelLineView : MonoBehaviour
{
    public Transform LeftAvatar;
    public Transform RightAvatar;
    public Transform Label;
    public Transform OptionA;
    public Transform OptionB;
    public Transform Tip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateViewInfo (NovelScript script, int line)
    {
        var linemsg = script.lines[line];

        Label.GetComponent<TextMeshProUGUI>().text = "";
        OptionA.GetComponent<TextMeshProUGUI>().text = "";
        OptionB.GetComponent<TextMeshProUGUI>().text = "";


        var avatar = LeftAvatar;
        if (linemsg.left)
        {
            LeftAvatar.Find("Avatar").GetComponent<Image>().color = Color.white;
            RightAvatar.Find("Avatar").GetComponent<Image>().color = Color.gray;
            LeftAvatar.Find("Name").transform.localScale = Vector3.one;
            RightAvatar.Find("Name").transform.localScale = Vector3.zero;
        }
        else
        {
            LeftAvatar.Find("Avatar").GetComponent<Image>().color = Color.gray;
            RightAvatar.Find("Avatar").GetComponent<Image>().color = Color.white;
            LeftAvatar.Find("Name").transform.localScale = Vector3.zero;
            RightAvatar.Find("Name").transform.localScale = Vector3.one;
            avatar = RightAvatar;
        }

        avatar.Find("Avatar").GetComponent<Image>().sprite = linemsg.sprite;
        avatar.Find("Name").GetComponent<TextMeshProUGUI>().text = linemsg.name;

        if (linemsg.options.Length == 0)
        {
            Label.GetComponent<TextMeshProUGUI>().text = linemsg.content;

            if (line < script.lines.Length - 1)
            {
                Tip.GetComponent<TextMeshProUGUI>().text = "按X继续对话";
            }
            else
            {
                Tip.GetComponent<TextMeshProUGUI>().text = "按X结束对话";
            }
        }
        else
        {
            OptionA.GetComponent<TextMeshProUGUI>().text = "<color=red>按Z选择:"+linemsg.options[0].label+"</color>";

            OptionB.GetComponent<TextMeshProUGUI>().text = "<color=blue>按X选择:" + linemsg.options[1].label+"</color>";

            Tip.GetComponent<TextMeshProUGUI>().text = "";
        }

    }
}
