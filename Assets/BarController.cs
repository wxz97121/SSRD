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

public enum beatPhase {
    Wait = 0,
    During = 1,
}

public class BarController : MonoBehaviour {

    public GameObject mBar;
    public float mBarLength;
    [HideInInspector] public float songDspTime;
    [HideInInspector] public float secPerBeat;
    public GameObject mJudge;
    public float mJudgeDir = 1.0f;

    public GameObject mBeatAction;
    public actionType mBeatActionCurrent;
    public Sprite[] mBeatActionIcons;
    public int mBeatCurrent;
    public beatPhase mBeatPhaseCurrent = beatPhase.Wait;

    public AudioSource mSong;
    public int mBpm = 120;

    public bool mBeatLock = false;
    public Character player;

    // Use this for initialization
    void Start () {
        BarReset();
	}

	// Update is called once per frame
	void Update () {
        float songPosition = (float)(AudioSettings.dspTime - songDspTime);
        float songPosInBeats = songPosition / secPerBeat;
        if (NumDiscretize(songPosInBeats / 4.0f - ((int)songPosInBeats / 4), 1f / 8f) > 7f/8f)
        {
            BeatEnd();
        }
        if ((int)songPosInBeats >= mBeatCurrent + 4)
        {
            mBeatCurrent += 4;
            BeatStart();
        }

        float mBarPercent = songPosInBeats / 4.0f - ((int)songPosInBeats / 4);
        mJudge.transform.localPosition = new Vector3(mBarLength * (mBarPercent - 0.5f) * mJudgeDir, -16f, 0);
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
    }

    public void ShowAction(actionType type){
        if (mBeatLock){
            return;
        }

        mBeatActionCurrent = type;

        float songPosition = (float)(AudioSettings.dspTime - songDspTime);
        float songPosInBeats = songPosition / secPerBeat;
        float mBarPercent = NumDiscretize(songPosInBeats / 4.0f - ((int)songPosInBeats / 4), 1f / 8f);
        mBeatAction.transform.localPosition = new Vector3(mBarLength * (mBarPercent - 0.5f) * mJudgeDir, 30f, 0);

        switch (type){
            case actionType.None :
                {
                    mBeatAction.GetComponent<Image>().sprite = mBeatActionIcons[0];
                }
                break;
            case actionType.Charge:
                {
                    mBeatAction.GetComponent<Image>().sprite = mBeatActionIcons[1];
                }
                break;
            case actionType.Hit:
                {
                    mBeatAction.GetComponent<Image>().sprite = mBeatActionIcons[2];
                }
                break;
            case actionType.Defense:
                {
                    mBeatAction.GetComponent<Image>().sprite = mBeatActionIcons[3];
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

    public void BeatEnd (){
        if (mBeatPhaseCurrent == beatPhase.During)
        {
            mBeatLock = true;
            ActionEffect();
            mBeatPhaseCurrent = beatPhase.Wait;
        }
    }

    public void ActionEffect (){
        switch (mBeatActionCurrent)
        {
            case actionType.None:
                {
                }
                break;
            case actionType.Charge:
                {
                    player.Charge();
                }
                break;
            case actionType.Hit:
                {
                    player.Hit();
                }
                break;
            case actionType.Defense:
                {
                    player.Defense();
                }
                break;
        }
    }

    public void BeatStart (){
        if (mBeatPhaseCurrent == beatPhase.Wait){
            mBeatLock = false;
            mBeatAction.GetComponent<Image>().sprite = mBeatActionIcons[0];
            mBeatActionCurrent = actionType.None;
            mBeatPhaseCurrent = beatPhase.During;
        }

    }
}
