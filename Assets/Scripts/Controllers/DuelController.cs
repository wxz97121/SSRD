using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗逻辑
public class DuelController : MonoBehaviour {


    //敌人复活倒计时(暂时用的，四拍以后招新敌人)
    public int EnemyCountdown = -1;

    //记录玩家在行动拍是否输入过，其实是个锁
    public bool isInputedAtActionBeat = false;



    #region 单例
    static DuelController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static DuelController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAction(actionType actionType)
    {



        switch (actionType)
        {
            case actionType.Collect:
                {
                    Debug.Log("------------------------");

                    Debug.Log("START TRYING COLLECT");
                    Debug.Log("currentEnergyNotes.Count:" + UIBarController.Instance.currentEnergyNotes.Count);
                    Debug.Log("comment value:" + RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes));

                    if (RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes)<2)
                    {
                        Player.Instance.AddMp(1);
                        SoundController.Instance.PlayAudioEffect("HIHAT");

                        UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                        UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

                 //      Debug.Log("2");

                    }
                    //
                }
                break;
            
            default:
                break;
        }
    }


    public void EnemyRespawn()
    {

        if(Player.Instance.enemyList.Count==0)
            BattleController.Instance.AddEnemy();

    }


    #region 第三拍结束时敌人行动
    public void BeatEndAction()
    {
        if (EnemyCountdown >= 0)
        {
            return;
        }

        if (!isInputedAtActionBeat)
        {
            SuperController.Instance.commentController.CallCommentUpdate(2);
            foreach (GameObject inst in Player.Instance.enemyList)
            {
                if (inst.GetComponent<AI>().actionID != -1)
                {
                    if (inst.GetComponent<AI>().actionSequence[inst.GetComponent<AI>().actionID] == 2)
                    {
                        inst.GetComponent<AI>().Action();
                    }
                }
            }
        }
        else
        {
            isInputedAtActionBeat = false;
        }
        updateEnemyAction();

        Player.Instance.OnTurnEnd();
    }
    #endregion


    //集中更新AI的ActionID
    public void updateEnemyAction()
    {
        foreach (GameObject inst in Player.Instance.enemyList)
        {
            inst.GetComponent<AI>().actionID = (inst.GetComponent<AI>().actionID + 1) % inst.GetComponent<AI>().actionSequence.Length;
        }
    }
}
