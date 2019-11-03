using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UISkillDesc : MonoBehaviour
{
    public Skill skill;

    public GameObject tipBarGO;

    private UISkillTipBar tipBar;
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Skill s)
    {
        //tipBarGO=
        skill = s;
        InitSkillTipBar();
        text.text = skill.Desc;
        //todo 播放视频

    }


    public void InitSkillTipBar()
    {

        //GameObject instSkillTip = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_SkillTipBar", typeof(GameObject)), this.transform);
        tipBarGO.name = skill.m_name;
        tipBar = tipBarGO.GetComponent<UISkillTipBar>();
        tipBar.Empty();
        tipBar.ReadScoreFromSkill(skill.inputSequence);
        tipBar.beatsThisBar = 4;
        tipBar.skill = skill;

        Debug.Log("skill name = " + tipBar.skill.m_name);

        tipBar.Init();
    }
}
