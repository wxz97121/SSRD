using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum actionType {
    None = 0,
    Charge = 1,
    Hit = 2,
    Defense = 3
}

public enum beatType
{
    Normal = 0
}

public class BarController : MonoBehaviour {

    public GameObject mBar;
    public float mBarLength;
    [HideInInspector] public float songDspTime;
    [HideInInspector] public float secPerBeat;
    public GameObject[] mJudge;

    public int mBeatCurrent;
    private List<GameObject> mBeatList = new List<GameObject>();

    public AudioSource mSong;
    public int mBpm = 120;

    public bool mBeatLock = false;
    public Character player;

    public CommentController commentController = null;

    // Use this for initialization
    void Start () {
        BarReset();
	}

	// Update is called once per frame
	void Update () {
        BeatUpdate();

    }

    private void BpmCalc (){
        secPerBeat = 60f / mBpm;
    }
    private void BarReset (){
        BpmCalc();
        songDspTime = (float)AudioSettings.dspTime;
        mBeatCurrent = 0;
        mSong.Play();
        BeatStart();

        commentController = GameObject.Find("Comment").GetComponent<CommentController>();
    }

    private void BeatUpdate (){
        float songPosition = (float)(AudioSettings.dspTime - songDspTime);
        float songPosInBeats = songPosition / secPerBeat;
        if ((int)songPosInBeats > mBeatCurrent)
        {
            BeatEnd();
            mBeatCurrent++;
        }

        float mBarPercent = songPosInBeats - ((int)songPosInBeats);

        mJudge[0].transform.localPosition = new Vector3(mBarLength * (mBarPercent), 0, 0);
        mJudge[1].transform.localPosition = new Vector3(mBarLength * (-mBarPercent), 0, 0);
        for (int i = 0; i < 4; i++)
        {
            mJudge[0].transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f - 0.25f * i + 0.25f * mBarPercent);
            mJudge[1].transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f - 0.25f * i + 0.25f * mBarPercent);
        }
        return;
    }

    public void ShowAction(actionType type){
        if (mBeatLock)
        {
            return;
        }

        commentController.CallCommentUpdate(BeatComment());

        switch (type){
            case actionType.None :
                {
                }
                break;
            case actionType.Charge:
                {
                }
                break;
            case actionType.Hit:
                {
                    player.Hit();
                }
                break;
            case actionType.Defense:
                {
                }
                break;
        }

        mBeatLock = true;
    }

    public float NumDiscretize (float origin, float interval){
        float num = 0f;
        while (num < origin){
            num += interval;
        }
        if (num - origin < interval / 2){
            return num;
        }
        else {
            return (num - interval);
        }

    }

    private int BeatComment (){
        float songPosition = (float)(AudioSettings.dspTime - songDspTime);
        float songPosInBeats = songPosition / secPerBeat;
        float mBarPercent = songPosInBeats - ((int)songPosInBeats);

        if (mBarPercent<0.5f) {
            return 0;
        }
        else if (mBarPercent <0.8f){
            return 1;
        }
        else {
            return 3;
        }
    }

    public void BeatEnd (){
        if (!mBeatLock){
            commentController.CallCommentUpdate(2);
        }
        else {
            mBeatLock = false;
        }
       
    }

    public void ActionEffect (){
        /*switch (mBeatActionCurrent)
        {
            case actionType.None:
                {
                }
                break;
            case actionType.Charge:
                {
                    //player.Charge();
                }
                break;
            case actionType.Hit:
                {
                    //player.Hit();
                }
                break;
            case actionType.Defense:
                {
                    //player.Defense();
                }
                break;
        }*/
    }

    public void BeatStart (){

    }
}
