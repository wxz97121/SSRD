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

    public void InitSkillTipBarArea()
    {
        currentBarList = new List<GameObject>();

        Debug.Log("InitSkillTipBarArea");
        //获取位置信息
        barPos0 = barPos0GO.transform.localPosition;
        oneBarSpace = barPos1GO.transform.localPosition - barPos0;

        for (int i = 0; i < Player.Instance.skills.Count; i++)
        {
            currentBarList.Add(CreateSkillTipBar(Player.Instance.skills[i]));
            currentBarList[i].transform.localPosition = barPos0 + (oneBarSpace * i);
            currentBarList[i].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public GameObject CreateSkillTipBar(Skill skill)
    {

        Debug.Log("CreateSkillTipBar");

        GameObject instSkillTip = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_SkillTipBar", typeof(GameObject)), transform);
        instSkillTip.name = skill.m_name;
        instSkillTip.GetComponent<UISkillTipBar>().ReadScoreFromSkill(skill.inputSequence);
        instSkillTip.GetComponent<UISkillTipBar>().beatsThisBar = 4;
        instSkillTip.GetComponent<UISkillTipBar>().testtext.text = skill.m_name;
        instSkillTip.GetComponent<UISkillTipBar>().costtext.text = skill.cost.ToString();
        instSkillTip.GetComponent<UISkillTipBar>().Init();

        return instSkillTip;
    }

    public void AddRightOInBar(string name,int index)
    {
        transform.Find(name).GetComponent<UISkillTipBar>().AddRightO(index);
    }

    public void RemoveAllRightO()
    {
        foreach(var bar in currentBarList)
        {
            bar.GetComponent<UISkillTipBar>().RemoveRightO();
        }
    }

    public void RemoveAllRightOWhenSuccess()
    {
        foreach (var bar in currentBarList)
        {
            bar.GetComponent<UISkillTipBar>().RemoveRightOWhenSuccess();
        }
    }
}
