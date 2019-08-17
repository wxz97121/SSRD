using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapAreaView : MonoBehaviour
{
    public MapArea area = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (area != null)
        {
            transform.Find("Label").GetComponent<TextMeshProUGUI>().text = area.AreaName;

            if (area.m_VisitType==VisitType.Visited)
            {
                GetComponent<Image>().color = Color.gray;
                GetComponent<Button>().interactable = false;
            }
            else if (area.m_VisitType == VisitType.CanVisit)
            {
                GetComponent<Image>().color = Color.green;
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Image>().color = Color.red;
                GetComponent<Button>().interactable = false;
            }
        }
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
