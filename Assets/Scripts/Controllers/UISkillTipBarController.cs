using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillTipBarController : MonoBehaviour
{
    //小节列表
    public List<GameObject> currentBarList;

    public GameObject barPos0GO;
    public GameObject barPos1GO;

    [HideInInspector] public Vector3 barPos0;
    [HideInInspector] public Vector3 oneBarSpace;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearSkillTipArea()
    {
        for (int i=0;i<currentBarList.Count;i++)
        {
            Destroy(currentBarList[i]);

        }
        currentBarList.Clear();
        //transform.Find("field").
    }
    public void InitSkillTipBarArea()
    {
        currentBarList = new List<GameObject>();

        Debug.Log("InitSkillTipBarArea");
        //获取位置信息
        barPos0 = barPos0GO.transform.localPosition;
        oneBarSpace = barPos1GO.transform.localPosition - barPos0;

        for (int i = 0; i < Player.Instance.skillSlots.Length; i++)
        {
            if (Player.Instance.skillSlots[i].skill == null) continue;
            GameObject tempSkillTipBar = CreateSkillTipBar(Player.Instance.skillSlots[i].skill);
            //currentBarList.Add(CreateSkillTipBar(Player.Instance.skillSlots[i].skill));

            //currentBarList[i].transform.localPosition = barPos0 + (oneBarSpace * i);
            //currentBarList[i].transform.localScale = new Vector3(1f, 1f, 1f);

            tempSkillTipBar.transform.localPosition = barPos0 + (oneBarSpace * i);
            tempSkillTipBar.transform.localScale = new Vector3(1f, 1f, 1f);
            currentBarList.Add(tempSkillTipBar);
        }
    }

    public GameObject CreateSkillTipBar(Skill skill)
    {
    
        GameObject instSkillTip = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_SkillTipBar", typeof(GameObject)), this.transform);
        instSkillTip.name = skill.m_name;
        instSkillTip.GetComponent<UISkillTipBar>().ReadScoreFromSkill(skill.inputSequence);
        instSkillTip.GetComponent<UISkillTipBar>().beatsThisBar = 4;
        instSkillTip.GetComponent<UISkillTipBar>().skill = skill;

        Debug.Log("skill name = " + instSkillTip.GetComponent<UISkillTipBar>().skill.m_name);

        instSkillTip.GetComponent<UISkillTipBar>().Init();

        return instSkillTip;
    }

    //输入正确的绿色圆圈
    public void AddRightOInBar(string name,int index)
    {
        transform.Find(name).GetComponent<UISkillTipBar>().AddRightO(index);
    }

    //输入错误 移除
    public void RemoveRightO(string skillname)
    {
        transform.Find(skillname).GetComponent<UISkillTipBar>().RemoveRightO();

    }

    public void RemoveAllRightO()
    {
        foreach(var bar in currentBarList)
        {
            bar.GetComponent<UISkillTipBar>().RemoveRightO();
        }
    }

    //发招完成的情况下移除所有绿圈
    public void RemoveAllRightOWhenSuccess()
    {
        foreach (var bar in currentBarList)
        {
            bar.GetComponent<UISkillTipBar>().RemoveRightOWhenSuccess();
        }
    }

    //更新CD时间显示
    public void UpdateCDs()
    {
        foreach (var bar in currentBarList)
        {
            bar.GetComponent<UISkillTipBar>().icon.UpdateCD();
        }
    }
}
