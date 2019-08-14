

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
            if ((skillGroupSeq.Count ==  0))
            {
                float P = Random.value;
                if (P > 0.8)
                {
                    if(lastSkill== "form1-attack1" || lastSkill == "form1-attack2")
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
        }
    }

    private void Phase2Action(int beatnum)
    {
        if (beatnum == 3)
        {
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
        else
        {
            CastSkill(GetNextSkill(beatnum));

        }
    }


}