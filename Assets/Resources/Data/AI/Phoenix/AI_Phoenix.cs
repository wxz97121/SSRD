//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AI_Phoenix : AI
//{
//    //记录当前阶段
//    public int phaseID = 0;

//    public QTEScoreData qTEScoreData_1;
//    public QTEScoreData qTEScoreData_2;

//    public OneSongScore qtescore1;
//    public OneSongScore qtescore2;


//    //public List<EnemySkill> SkillSequence;

//    public List<string> SG_P1_idle;
//    public List<string> SG_P0_intro;
//    public List<string> SG_P1_attack;


//    public List<string> SG_P2_attack1;
//    public List<string> SG_P2_idle;
//    public List<string> SG_P2_ready2;
//    public List<string> SG_P2_attack2;
//    public List<string> SG_P2_reborn;
//    public List<string> SG_P2_defend;




//    override protected void Update()
//    {
//        base.Update();

//    }

//    override protected void Start()
//    {
//        base.Start();
//    }

//    public override void Init()
//    {
//        base.Init();
//        skillSequence.Clear();
//        phaseID = -1;
//        actionID = 0;
//        qtescore1 = OneSongScore.ReadQTEScoreData(qTEScoreData_1);
//        qtescore2 = OneSongScore.ReadQTEScoreData(qTEScoreData_2);
//        isUndead = true;

//        //SkillSequence = new List<EnemySkill>();

//        //装载技能组
//        SG_P1_idle = new List<string>
//        {
//            "phase1_idle",
//            "phase1_idle",
//            "phase1_idle",
//            "phase1_idle"
//        };

//        SG_P1_attack = new List<string>
//        {
//            "phase1_idle",
//            "phase1_attackwarn",
//            "phase1_idle",
//            "phase1_attack"
//        };

//        SG_P0_intro = new List<string>
//        {
//            "phase1_idle",
//            "phase1_dmp",
//            "phase1_idle",
//            "phase1_idle"
//        };

//        //P2技能组
//        SG_P2_idle = new List<string>
//        {
//            "phase2_idle",
//            "phase2_idle",
//            "phase2_idle",
//            "phase2_idle"
//        };

//        SG_P2_reborn = new List<string>
//        {
//            "phase2_reborn",
//             "phase2_idle",
//            "phase2_idle",
//            "phase2_idle"
//        };

//        SG_P2_attack1 = new List<string>
//        {
//            "phase2_idle",
//            "phase2_attack1warn",
//            "phase2_idle",
//            "phase2_attack1"
//        };

//        SG_P2_ready2 = new List<string>
//        {
//            "phase2_idle",
//            "phase2_idle",
//            "phase2_ready2",
//            "phase2_ready2"
//        };
//        SG_P2_attack2 = new List<string>
//        {
//            "phase2_ready2",
//            "phase2_ready2",
//            "phase2_attack2",
//            "phase2_idle"
//        };



//        SG_P2_defend = new List<string>
//        {
//            "phase2_idle",
//            "phase2_idle",
//            "phase2_defend",
//            "phase2_idle"
//        };

//    }

//    public override void Action(int beatnum)
//    {
//        base.Action(beatnum);


//        switch (phaseID)
//        {
//            case -1:

//                PhaseIntro();
//                break;


//            case 0:
//                Phase1();
//                break;

//            case 1:
//                PhaseQTE1();
//                break;
//            case 2:

//                Phase2();
//                break;
//            case 3:
//                PhaseQTE2();
//                break;
//            case 4:
//                Phase3();
//                break;
//        }
//    }

//    public override void QTEAction(string actionname)
//    {
//        base.QTEAction(actionname);
//        _skillDictionary[actionname].EffectFunction(this);

//    }

//    private void PhaseIntro()
//    {
//        SoundController.Instance.FMODSetParameter("boss", 1);
//        SoundController.Instance.FMODSetParameter("chorus", 0);
//        SoundController.Instance.FMODSetParameter("verse", 0);
//        SoundController.Instance.FMODSetParameter("breakdown", 0);
//        SoundController.Instance.FMODSetParameter("outro", 0);

//        if (skillSequence.Count == 0)
//        {
//            skillSequence.Clear();
//            skillSequence.AddRange(SG_P0_intro);
//            Debug.Log("skillSequence[0] = "+skillSequence[0]);
//        }
//        Debug.Log("actionID=" + actionID);
//        _skillDictionary[skillSequence[actionID]].EffectFunction(this);

//        actionID++;
//        if (actionID >= skillSequence.Count)
//        {
//            phaseID = 0;
//            actionID = 0;
//            skillSequence.Clear();
//            skillSequence.AddRange(SG_P1_idle);

//        }


//    }

//    private void Phase1()
//    {


//        _skillDictionary[skillSequence[actionID]].EffectFunction(this);


//        actionID++;
//        if (actionID >= skillSequence.Count)
//        {
//            System.Random rng = new System.Random();
//            float randomvalue = rng.Next(0, 10);
//            Debug.Log("random" + randomvalue);
//            if (randomvalue >= 7)
//            {
//                skillSequence.Clear();
//                skillSequence.AddRange(SG_P1_attack);
//            }
//            else
//            {
//                skillSequence.Clear();
//                skillSequence.AddRange(SG_P1_idle);
//            }
//            actionID = 0;
//        }



//        //转换阶段至QTE1
//        if (Hp <= 0)
//        {
//            SoundController.Instance.FMODSetParameter("boss", 0);
//            SoundController.Instance.FMODSetParameter("chorus", 0);
//            SoundController.Instance.FMODSetParameter("verse", 0);
//            SoundController.Instance.FMODSetParameter("breakdown", 1);
//            SoundController.Instance.FMODSetParameter("outro", 0);
//            //Debug.Log(SoundController.Instance.GetLastMarker());
//            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
//            {

//                //进入QTE状态
//                RhythmController.Instance.QTEStart(qtescore1);
//                actionID = 0;
//                phaseID = 1;
//                //    Debug.Log("change qte mode complete");

//                SoundController.Instance.FMODSetParameter("boss", 0);
//                SoundController.Instance.FMODSetParameter("chorus", 0);
//                SoundController.Instance.FMODSetParameter("verse", 1);
//                SoundController.Instance.FMODSetParameter("breakdown", 0);
//                SoundController.Instance.FMODSetParameter("outro", 0);

//                _skillDictionary["QTE1_turn"].EffectFunction(this);
//                skillSequence.Clear();
//            }

//        }
//    }


//    private void PhaseQTE1()
//    {
//        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
//        {
//            //           Debug.Log("back to start");
//            //SuperController.Instance.state = GameState.Start;
//            phaseID = 2;
//            skillSequence.Clear();
//            skillSequence.AddRange(SG_P2_reborn);
//        }
//    }

//    private void Phase2()
//    {

//        _skillDictionary[skillSequence[actionID]].EffectFunction(this);

//        isUndead = true;
//        actionID++;
//        if (actionID >= skillSequence.Count)
//        {
//            System.Random rng = new System.Random();
//            float randomvalue = rng.Next(0, 10);
//            Debug.Log("value" + randomvalue);
//            if (randomvalue >= 6)
//            {
//                Debug.Log(">=6");
//                skillSequence.Clear();
//                skillSequence.AddRange(SG_P2_attack1);
//            }
//            else if (randomvalue >= 3)
//            {
//                Debug.Log(">=3");

//                skillSequence.Clear();
//                skillSequence.AddRange(SG_P2_ready2);
//                skillSequence.AddRange(SG_P2_attack2);
//            }
//            //else if (randomvalue >= 2)
//            //{
//            //    SkillSequence = SG_P2_defend;
//            //}
//            else
//            {
//                Debug.Log("else");
//                skillSequence.Clear();
//                skillSequence.AddRange(SG_P2_idle);
//            }
//            actionID = 0;
//        }



//        //转换阶段至QTE2
//        if (Hp <= 0)
//        {
//            SoundController.Instance.FMODSetParameter("boss", 0);
//            SoundController.Instance.FMODSetParameter("chorus", 0);
//            SoundController.Instance.FMODSetParameter("verse", 0);
//            SoundController.Instance.FMODSetParameter("breakdown", 1);
//            SoundController.Instance.FMODSetParameter("outro", 0);
//            //Debug.Log(SoundController.Instance.GetLastMarker());
//            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
//            {
//                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore2.QTEscore[0].notes);
//                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore2.QTEscore[1].notes);
//                SuperController.Instance.state = GameState.QTE;
//                UIBarController.Instance.QTEscore = qtescore2;
//                UIBarController.Instance.QTEbarIndex = 1;
//                actionID = 0;
//                phaseID = 3;
//                //    Debug.Log("change qte mode complete");

//                SoundController.Instance.FMODSetParameter("boss", 0);
//                SoundController.Instance.FMODSetParameter("chorus", 1);
//                SoundController.Instance.FMODSetParameter("verse", 0);
//                SoundController.Instance.FMODSetParameter("breakdown", 0);
//                SoundController.Instance.FMODSetParameter("outro", 0);

//                _skillDictionary["QTE1_turn"].EffectFunction(this);

//            }

//        }
//    }

//    private void PhaseQTE2()
//    {
//        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
//        {
//            //           Debug.Log("back to start");
//            SuperController.Instance.state = GameState.Start;
//            phaseID = 4;
//            isUndead = false;

//        }
//    }

//    private void Phase3()
//    {
//        isUndead = false;
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Phoenix : AI
{
    public QTEScoreData qTEScoreData_1; 
    public OneSongScore qtescore1;
    public int phaseID = 1;

    override protected void Update()
    {
        base.Update();
    }

    override protected void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();
        phaseID = 1;
        isUndead = true;
        qtescore1 = OneSongScore.ReadQTEScoreData(qTEScoreData_1);

        SGSAdd("form1-idle");
        SGSAdd("form1-idle");

        SoundController.Instance.FMODSetParameter("boss", 1);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);
    }


    public override void Action(int beatnum)
    {
        base.Action(beatnum);

        switch (phaseID)
        {
            case 1:
                Phase1Action(beatnum);
                break;
            case 2:
                PhaseQTE1(beatnum);
                break;
            case 3:
                Phase2Action(beatnum);
                break;
        }




    }

    private void Phase1Action(int beatnum)
    {
        if(beatnum == 1)
        {
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {

                //进入QTE状态
                RhythmController.Instance.QTEStart(qtescore1);
                actionID = 0;
                phaseID = 2;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 1);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);

                CastSkill("ANI_qte1-idle");
            }
            else
            {
                CastSkill(GetNextSkill(beatnum));

            }
        }

        if (beatnum == 3)
        {
            if ((skillGroupSeq.Count == actionID + 1))
            {
                float P = Random.value;
                if (P > 0.8)
                {
                    if(skillGroupSeq[actionID].name== "form1-attack1" || skillGroupSeq[actionID].name == "form1-attack2")
                    {
                        SGSAdd("form1-idle");
                    }
                    else
                    {
                        SGSAdd("form1-attack1");

                    }

                }
                else if (P > 0.5)
                {
                    if (skillGroupSeq[actionID].name == "form1-attack1" || skillGroupSeq[actionID].name == "form1-attack2")
                    {
                        SGSAdd("form1-idle");
                    }
                    else
                    {
                        SGSAdd("form1-attack2");

                    }


                }
                else if (P > 0.2)
                {
                    SGSAdd("form1-defend");

                }
                else
                {
                    SGSAdd("form1-idle");
                }
            }
            actionID++;

        }

        if (beatnum==4)
        {
            //转换阶段至QTE1
            if (life <= 0)
            {
                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 1);
                SoundController.Instance.FMODSetParameter("outro", 0);
                //Debug.Log(SoundController.Instance.GetLastMarker());


            }
            //Debug.Log("cast skill="+GetNextSkill(beatnum));
            CastSkill(GetNextSkill(beatnum));
        }

        if(beatnum ==2)
        {
            CastSkill(GetNextSkill(beatnum));

        }



    }
    private void PhaseQTE1(int beatnum)
    {
        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
        {
            //           Debug.Log("back to start");
            //SuperController.Instance.state = GameState.Start;
            phaseID = 3;
            CastSkill("HEAL_30,HL_30,ANI_form2-heal");
            isUndead = false;
            SGSAdd("form2-idle");
            actionID = skillGroupSeq.Count - 1;
        }
    }

    private void Phase2Action(int beatnum)
    {
        if (beatnum == 3)
        {
            if (skillGroupSeq.Count == actionID + 1)
            {
                float P = Random.value;
                if (P > 0.7)
                {
                    if (skillGroupSeq[actionID - 1].name == "form2-attack1" || skillGroupSeq[actionID - 1].name == "form2-attack2")
                    {
                        SGSAdd("form2-idle");
                    }
                    else
                    {
                        SGSAdd("form2-attack1");

                    }

                }
                else if (P > 0.35f)
                {
                    if (skillGroupSeq[actionID - 1].name == "form2-attack1" || skillGroupSeq[actionID - 1].name == "form2-attack2")
                    {
                        SGSAdd("form2-idle");
                    }
                    else
                    {
                        SGSAdd("form2-attack2");

                    }


                }
                else if (P > 0.1f)
                {
                    SGSAdd("form2-heal");
                    SGSAdd("form2-idle");


                }
                else
                {
                    SGSAdd("form2-idle");
                }
            }
            actionID++;

        }
        else
        {
            CastSkill(GetNextSkill(beatnum));

        }
    }


}