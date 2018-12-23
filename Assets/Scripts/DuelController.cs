using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗逻辑
public class DuelController : MonoBehaviour {


    //敌人复活倒计时(暂时用的，四拍以后招新敌人)
    public int EnemyCountdown = -1;



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
        //如果没敌人了就先不管
        if (Player.Instance.enemyList.Count <= 0)
        {
            return;
        }

        switch (actionType)
        {
            case actionType.Collect:
                {
                    //Debug.Log("1");

                    if (RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes)<2)
                    {
                        Player.Instance.AddMp(1);
                        UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("UINoteFadeOut");
                        UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

                        Debug.Log("2");

                    }

                }
                break;
            case actionType.None:
                break;
            case actionType.Charge:
                break;
            case actionType.Hit:
                break;
            case actionType.Defense:
                break;
            default:
                break;
        }
    }


    public void EnemyRespawn()
    {
        if (EnemyCountdown > 0)
        {
            Debug.Log("Enemy Reborn: " + EnemyCountdown);
            EnemyCountdown--;
        }
        else if (EnemyCountdown == 0)
        {
            BattleController.Instance.AddEnemy();
            EnemyCountdown--;
        }
        else
        {
            if (Player.Instance.enemyList.Count <= 0)
            {
                Debug.Log("No Enemy");
                EnemyCountdown = 4;
            }
        }

    }
}
