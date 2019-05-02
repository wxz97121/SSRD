﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Phoenix : AI
{
    //记录当前阶段
    public int phaseID = 0;

    public QTEScoreData qTEScoreData_1;
    public QTEScoreData qTEScoreData_2;

    public OneSongScore qtescore1;
    public OneSongScore qtescore2;


    //public List<EnemySkill> SkillSequence;

    public List<string> SG_P1_idle;
    public List<string> SG_P0_intro;
    public List<string> SG_P1_attack;


    public List<string> SG_P2_attack1;
    public List<string> SG_P2_idle;
    public List<string> SG_P2_ready2;
    public List<string> SG_P2_attack2;
    public List<string> SG_P2_reborn;
    public List<string> SG_P2_defend;




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
        phaseID = -1;
        actionID = 0;
        qtescore1 = OneSongScore.ReadQTEScoreData(qTEScoreData_1);
        qtescore2 = OneSongScore.ReadQTEScoreData(qTEScoreData_2);
        isUndead = true;

        //SkillSequence = new List<EnemySkill>();

        //装载技能组
        SG_P1_idle = new List<string>
        {
            "phase1_idle",
            "phase1_idle",
            "phase1_idle",
            "phase1_idle"
        };

        SG_P1_attack = new List<string>
        {
            "phase1_idle",
            "phase1_attackwarn",
            "phase1_idle",
            "phase1_attack"
        };

        SG_P0_intro = new List<string>
        {
            "phase1_idle",
            "phase1_dmp",
            "phase1_idle",
            "phase1_idle"
        };

        //P2技能组
        SG_P2_idle = new List<string>
        {
            "phase2_idle",
            "phase2_idle",
            "phase2_idle",
            "phase2_idle"
        };

        SG_P2_reborn = new List<string>
        {
            "phase2_reborn",
             "phase2_idle",
            "phase2_idle",
            "phase2_idle"
        };

        SG_P2_attack1 = new List<string>
        {
            "phase2_idle",
            "phase2_attack1warn",
            "phase2_idle",
            "phase2_attack1"
        };

        SG_P2_ready2 = new List<string>
        {
            "phase2_idle",
            "phase2_idle",
            "phase2_ready2",
            "phase2_ready2"
        };
        SG_P2_attack2 = new List<string>
        {
            "phase2_ready2",
            "phase2_ready2",
            "phase2_attack2",
            "phase2_idle"
        };



        SG_P2_defend = new List<string>
        {
            "phase2_idle",
            "phase2_idle",
            "phase2_defend",
            "phase2_idle"
        };

    }

    public override void Action()
    {
        base.Action();


        switch (phaseID)
        {
            case -1:

                PhaseIntro();
                break;


            case 0:
                Phase1();
                break;

            case 1:
                PhaseQTE1();
                break;
            case 2:

                Phase2();
                break;
            case 3:
                PhaseQTE2();
                break;
            case 4:
                Phase3();
                break;
        }
    }

    public override void QTEAction(string actionname)
    {
        base.QTEAction(actionname);
        _skillDictionary[actionname].EffectFunction(this);

    }

    private void PhaseIntro()
    {
        SoundController.Instance.FMODSetParameter("boss", 1);
        SoundController.Instance.FMODSetParameter("chorus", 0);
        SoundController.Instance.FMODSetParameter("verse", 0);
        SoundController.Instance.FMODSetParameter("breakdown", 0);
        SoundController.Instance.FMODSetParameter("outro", 0);

        if (skillSequence.Count == 0)
        {
            skillSequence.Clear();
            skillSequence.AddRange(SG_P0_intro);
        }
        Debug.Log("actionID=" + actionID);
        _skillDictionary[skillSequence[actionID]].EffectFunction(this);

        actionID++;
        if (actionID >= skillSequence.Count)
        {
            phaseID = 0;
            actionID = 0;
            skillSequence.Clear();
            skillSequence.AddRange(SG_P1_idle);

        }


    }

    private void Phase1()
    {


        _skillDictionary[skillSequence[actionID]].EffectFunction(this);


        actionID++;
        if (actionID >= skillSequence.Count)
        {
            System.Random rng = new System.Random();
            float randomvalue = rng.Next(0, 10);
            Debug.Log("random" + randomvalue);
            if (randomvalue >= 7)
            {
                skillSequence.Clear();
                skillSequence.AddRange(SG_P1_attack);
            }
            else
            {
                skillSequence.Clear();
                skillSequence.AddRange(SG_P1_idle);
            }
            actionID = 0;
        }



        //转换阶段至QTE1
        if (Hp <= 0)
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 1);
            SoundController.Instance.FMODSetParameter("outro", 0);
            //Debug.Log(SoundController.Instance.GetLastMarker());
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {

                //进入QTE状态
                RhythmController.Instance.QTEStart(qtescore1);
                actionID = 0;
                phaseID = 1;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 1);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);

                _skillDictionary["QTE1_turn"].EffectFunction(this);
                skillSequence.Clear();
            }

        }
    }


    private void PhaseQTE1()
    {
        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
        {
            //           Debug.Log("back to start");
            //SuperController.Instance.state = GameState.Start;
            phaseID = 2;
            skillSequence.Clear();
            skillSequence.AddRange(SG_P2_reborn);
        }
    }

    private void Phase2()
    {

        _skillDictionary[skillSequence[actionID]].EffectFunction(this);

        isUndead = true;
        actionID++;
        if (actionID >= skillSequence.Count)
        {
            System.Random rng = new System.Random();
            float randomvalue = rng.Next(0, 10);
            Debug.Log("value" + randomvalue);
            if (randomvalue >= 6)
            {
                Debug.Log(">=6");
                skillSequence.Clear();
                skillSequence.AddRange(SG_P2_attack1);
            }
            else if (randomvalue >= 3)
            {
                Debug.Log(">=3");

                skillSequence.Clear();
                skillSequence.AddRange(SG_P2_ready2);
                skillSequence.AddRange(SG_P2_attack2);
            }
            //else if (randomvalue >= 2)
            //{
            //    SkillSequence = SG_P2_defend;
            //}
            else
            {
                Debug.Log("else");
                skillSequence.Clear();
                skillSequence.AddRange(SG_P2_idle);
            }
            actionID = 0;
        }



        //转换阶段至QTE2
        if (Hp <= 0)
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 1);
            SoundController.Instance.FMODSetParameter("outro", 0);
            //Debug.Log(SoundController.Instance.GetLastMarker());
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {
                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore2.QTEscore[0].notes);
                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore2.QTEscore[1].notes);
                SuperController.Instance.state = GameState.QTE;
                UIBarController.Instance.QTEscore = qtescore2;
                UIBarController.Instance.QTEbarIndex = 1;
                actionID = 0;
                phaseID = 3;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 1);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);

                _skillDictionary["QTE1_turn"].EffectFunction(this);

            }

        }
    }

    private void PhaseQTE2()
    {
        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
        {
            //           Debug.Log("back to start");
            SuperController.Instance.state = GameState.Start;
            phaseID = 4;
            isUndead = false;

        }
    }

    private void Phase3()
    {
        isUndead = false;
    }
}
