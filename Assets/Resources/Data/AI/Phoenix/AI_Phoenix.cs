

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Phoenix : AI
{
    public QTEScoreData qTEScoreData_1;
    public OneSongScore qtescore1;
    public QTEScoreData qTEScoreData_2;
    public OneSongScore qtescore2;
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
        qtescore2 = OneSongScore.ReadQTEScoreData(qTEScoreData_2);

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
            case 4:
                PhaseQTE2(beatnum);
                break;
            case 5:
                Phase3Action(beatnum);
                break;
        }




    }

    private void Phase1Action(int beatnum)
    {
        if (beatnum == 1)
        {
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {

                //进入QTE状态
                RhythmController.Instance.QTEStart(qtescore1);
                actionID = 0;
                phaseID = 2;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 1);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);
                skillGroupSeq.Clear();
                CastSkill("ANI_qte1-idle");
            }
            else
            {
                CastSkill(GetNextSkill(beatnum));

            }
        }

        if (beatnum == 3 && life > 0)
        {
            if ((skillGroupSeq.Count == 0))
            {
                float P = Random.value;
                if (P > 0.8)
                {
                    if (lastSkill == "form1-attack1" || lastSkill == "form1-attack2")
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
                    if (lastSkill == "form1-attack1" || lastSkill == "form1-attack2")
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

        }

        if (beatnum == 4)
        {
            //转换阶段至QTE1
            if (life <= 0)
            {
                CastSkill("ANI_qte1-idle");
                skillGroupSeq.Clear();
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

        if (beatnum == 2)
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
            CastSkill("HEAL_30,HL_30,VFX_name:HealBig&father:1&scale:2/2/2,ANI_form2-idle");
            isUndead = true;
            SGSAdd("form2-idle");
        }
    }

    private void Phase2Action(int beatnum)
    {
        Debug.Log("111111");
        if (beatnum == 1)
        {
            if (SoundController.Instance.GetLastMarker() == "minibridge" || SoundController.Instance.GetLastMarker() == "breakdown")
            {

                //进入QTE2状态
                RhythmController.Instance.QTEStart(qtescore2);
                phaseID = 4;
                //    Debug.Log("change qte mode complete");

                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 1);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 0);
                skillGroupSeq.Clear();
                CastSkill("ANI_qte1-idle");
            }
            else
            {
                CastSkill(GetNextSkill(beatnum));

            }

        }
        if (beatnum == 2)
        {
            CastSkill(GetNextSkill(beatnum));

        }


        if (beatnum == 3)
        {
            Debug.Log("2222222");

            if (skillGroupSeq.Count == 0)
            {
                float P = Random.value;
                if (P > 0.7)
                {
                    if (lastSkill == "form2-attack1" || lastSkill == "form2-attack2")
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
                    if (lastSkill == "form2-attack1" || lastSkill == "form2-attack2")
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

        }

        if (beatnum == 4)
        {
            //转换阶段至QTE2
            if (life <= 0)
            {
                Debug.Log("3333");

                CastSkill("ANI_qte2-idle");
                skillGroupSeq.Clear();
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
    }


    private void PhaseQTE2(int beatnum)
    {
        if (UIBarController.Instance.QTEbarIndex < 0 && SoundController.Instance.timelineInfo.currentMusicBeat >= 4)
        {
            //           Debug.Log("back to start");
            //SuperController.Instance.state = GameState.Start;
            phaseID = 5;
            CastSkill("HEAL_30,HL_30,VFX_name:HealBig&father:1&scale:2/2/2,ANI_form2-idle");
            isUndead = false;
            SGSAdd("form2-idle");
        }
    }


    private void Phase3Action(int beatnum)
    {
        if (beatnum == 1)
        {

            CastSkill(GetNextSkill(beatnum));

        }
        if (beatnum == 2)
        {
            CastSkill(GetNextSkill(beatnum));

        }


        if (beatnum == 3)
        {
            if (skillGroupSeq.Count == 0)
            {
                float P = Random.value;
                if (P > 0.7)
                {

                        SGSAdd("form3-heal");


                }
                else if (P > 0.35f)
                {



                        SGSAdd("form3-pierce");




                }else
                {
                    SGSAdd("form2-idle");
                }
            }

        }

        if (beatnum == 4)
        {
            
            CastSkill(GetNextSkill(beatnum));
            if (life <= 0)
            {
                skillGroupSeq.Clear();
                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 0);
                SoundController.Instance.FMODSetParameter("outro", 1);
            }
        }
    }

}