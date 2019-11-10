using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapAreaView : MonoBehaviour
{
    public MapArea area = null;
    public Text text;
    public Image newAreaTag;
    public Image shopTag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        RefreshInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshInfo()
    {
        text.text = area.AreaName;

        if (area.isVisited == false&&area.m_VisitType!=MapState.Locked)
        {
            newAreaTag.transform.localScale = Vector3.one;
        }
        else
        {
            newAreaTag.transform.localScale = Vector3.zero;
        }
        if (area.levelData.shopData != null)
        {
            shopTag.transform.localScale = Vector3.one;
        }
        else
        {
            shopTag.transform.localScale = Vector3.zero;
        }
    }



    public void ShowDesc()
    {
        string lootequip="";
        string lootskill="";
        if (area.levelData.AwardEquip != null)
        {
            lootequip = area.levelData.AwardEquip.equipName;
        }
        if (area.levelData.AwardSkill != null)
        {
            lootskill = area.levelData.AwardSkill._name;
        }
        GameObject.Find("AreaDescText").GetComponent<Text>().text = area.levelData.AreaName + " , \n 掉落：" + lootequip + "  " + lootskill;
    }
    public void HideDesc()
    {
        GameObject.Find("AreaDescText").GetComponent<Text>().text = "";
    }
}
