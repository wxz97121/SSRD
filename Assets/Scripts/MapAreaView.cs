using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapAreaView : MonoBehaviour
{
    public MapArea area = null;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        text.text = area.AreaName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        area.Activate();
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
