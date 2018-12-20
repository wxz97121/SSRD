using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗逻辑
public class DuelController : MonoBehaviour {


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
}
