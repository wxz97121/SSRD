using System.Collections;
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


    public List<EnemySkill> SkillSequence;

    public List<EnemySkill> SG_P1_idle;
    public List<EnemySkill> SG_P0_intro;
    public List<EnemySkill> SG_P1_attack;


    public List<EnemySkill> SG_P2_attack1;
    public List<EnemySkill> SG_P2_idle;
    public List<EnemySkill> SG_P2_ready2;
    public List<EnemySkill> SG_P2_attack2;
    public List<EnemySkill> SG_P2_reborn;
    public List<EnemySkill> SG_P2_defend;




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

        SkillSequence = new List<EnemySkill>();

        //装载技能组
        SG_P1_idle = new List<EnemySkill>
        {
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_idle"]
        };

        SG_P1_attack = new List<EnemySkill>
        {
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_attackwarn"],
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_attack"]
        };

        SG_P0_intro = new List<EnemySkill>
        {
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_dmp"],
            _skillDictionary["phase1_idle"],
            _skillDictionary["phase1_idle"]
        };

        //P2技能组
        SG_P2_idle = new List<EnemySkill>
        {
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"]
        };

        SG_P2_reborn = new List<EnemySkill>
        {
            _skillDictionary["phase2_reborn"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"]
        };

        SG_P2_attack1 = new List<EnemySkill>
        {
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_attack1warn"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_attack1"]
        };

        SG_P2_ready2 = new List<EnemySkill>
        {
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_attack2warn"],
            _skillDictionary["phase2_ready2"]
        };
        SG_P2_attack2 = new List<EnemySkill>
        {
            _skillDictionary["phase2_ready2"],
            _skillDictionary["phase2_ready2"],
            _skillDictionary["phase2_ready2"],
            _skillDictionary["phase2_attack2"]
        };



        SG_P2_defend = new List<EnemySkill>
        {
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_idle"],
            _skillDictionary["phase2_defend"]
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

        if (SkillSequence.Count == 0)
        {
            SkillSequence.Clear();
            SkillSequence.AddRange(SG_P0_intro);
        }
        Debug.Log("actionID=" + actionID);
        SkillSequence[actionID].EffectFunction(this);
        actionID++;
        if (actionID >= SkillSequence.Count)
        {
            phaseID = 0;
            actionID = 0;
            SkillSequence.Clear();
            SkillSequence.AddRange(SG_P1_idle);

        }


    }

    private void Phase1()
    {


        SkillSequence[actionID].EffectFunction(this);


        actionID++;
        if (actionID >= SkillSequence.Count)
        {
            System.Random rng = new System.Random();
            float randomvalue = rng.Next(0, 10);
            Debug.Log("random" + randomvalue);
            if (randomvalue >= 7)
            {
                SkillSequence.Clear();
                SkillSequence.AddRange(SG_P1_attack);
            }
            else
            {
                SkillSequence.Clear();
                SkillSequence.AddRange(SG_P1_idle);
            }
            actionID = 0;
        }



        //转换阶段至QTE1
        if (Hp <= 0)
        {
            InputSequenceController.Instance.CleanInputSequence();
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 1);
            SoundController.Instance.FMODSetParameter("outro", 0);
            //Debug.Log(SoundController.Instance.GetLastMarker());
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {
                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore1.QTEscore[0].notes);
                UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore1.QTEscore[1].notes);
                SuperController.Instance.state = GameState.QTE;
                UIBarController.Instance.QTEscore = qtescore1;
                UIBarController.Instance.QTEbarIndex = 1;
                actionID = 0;
                phaseID = 1;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 1);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);

                _skillDictionary["QTE1_turn"].EffectFunction(this);
                SkillSequence.Clear();
            }

        }
    }


    private void PhaseQTE1()
    {
        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
        {
            //           Debug.Log("back to start");
            SuperController.Instance.state = GameState.Start;
            phaseID = 2;
            SkillSequence.Clear();
            SkillSequence.AddRange(SG_P2_reborn);
        }
    }

    private void Phase2()
    {

        SkillSequence[actionID].EffectFunction(this);

        isUndead = false;
        actionID++;
        if (actionID >= SkillSequence.Count)
        {
            System.Random rng = new System.Random();
            float randomvalue = rng.Next(0, 10);
            Debug.Log("value" + randomvalue);
            if (randomvalue >= 6)
            {
                Debug.Log(">=6");
                SkillSequence.Clear();
                SkillSequence.AddRange(SG_P2_attack1);
            }
            else if (randomvalue >= 3)
            {
                Debug.Log(">=3");

                SkillSequence.Clear();
                SkillSequence.AddRange(SG_P2_ready2);
                SkillSequence.AddRange(SG_P2_attack2);
            }
            //else if (randomvalue >= 2)
            //{
            //    SkillSequence = SG_P2_defend;
            //}
            else
            {
                Debug.Log("else");
                SkillSequence.Clear();
                SkillSequence.AddRange(SG_P2_idle);
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
        }
    }

    private void Phase3()
    {
        isUndead = false;
    }
}
