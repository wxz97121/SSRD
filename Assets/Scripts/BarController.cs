using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//行动类别
public enum actionType {
    None = 0,
    Charge = 1,
    Hit = 2,
    Defense = 3
}
//拍子类别，目前没用留作拓展
public enum beatType
{
    Normal = 0
}
//节拍总控制
public class BarController : MonoBehaviour {
    static BarController _instance;
    //节拍条
    public GameObject mBar;
    //节拍条长度
    public float mBarLength;

    //歌曲开始时间(用来处理当前节奏)
    [HideInInspector] public float songDspTime;
    //每一拍长度
    [HideInInspector] public float secPerBeat;
    //判定音符(目前有两个，分别从左右向中间靠近)
    public GameObject[] mJudge;
    //开始位置
    private Vector3 noteSpawnPos = new Vector3(380, 0, 0);
    //结束位置
    private Vector3 noteEndPos = new Vector3(0, 0, 0);
    //当前乐谱
    public Score score=new Score();
    //音符计数
    public int nextNoteIndex = 0;
    //提前展示多少拍
    public float beatsShownInAdvance = 4f;
    //记录当前是第几个循环
    public float mCycleCount = 0f;

    //当前拍
    public int mBeatCurrent;
    //拍列表(当前没用)
    private List<Note> mRunningNoteList = new List<Note>();

    //BGM源
    public AudioSource mSong;
    //BPM
    public int mBpm = 120;

    //拍锁(这一拍按过就不能按了)
    public bool mBeatLock = false;
    //中间锁(用在beatCenter里，防止动画重复播放)
    public bool mCenterLock = false;
    //结束锁(当前没用，之前是为了区分拍子完结的两种状态：自然完结和玩家敲击完结)
    public bool mEndLock = false;
    //评价控制(评价控制还没改成全局控制)
    public CommentController commentController = null;

    //敌人复活倒计时(暂时用的，四拍以后招新敌人)
    public int EnemyCountdown = -1;

    //校正(校准因为视觉产生的节拍误差)
    public float songPosOffset = 0.5f;


    private void Awake()
    {
        _instance = this;
    }

    public static BarController Instance
    {
        get
        {
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {

        //测试数据，之后改为读取配置
        score.notesPerCycle = 8f;
        for (int i = 0; i < 4; i++)
        {
            Note temp = new Note();
            temp.beat = (float)(i * 2 + 0.5);
            temp.note = (GameObject)Resources.Load("Prefabs/Note");
            temp.type = Note.noteType.action;
            score.notes.Add(temp);


        } 

    BarReset();
	}

	// Update is called once per frame
	void Update () {
        BeatUpdate();

    }
    //计算bpm
    private void BpmCalc (){
        secPerBeat = 60f / mBpm;
    }
    //重置节拍条(重新计算bpm,归零，播放歌曲，开始游戏等)
    private void BarReset (){
        BpmCalc();

        //获取歌曲开始播放的时间点
        songDspTime = (float)AudioSettings.dspTime;

        mBeatCurrent = 1;
        mSong.Play();
        BeatStart();

        commentController = GameObject.Find("Comment").GetComponent<CommentController>();
    }
    //更新节拍
    private void BeatUpdate() {
        //获得当前歌曲位置
        float songPosition = (float)(AudioSettings.dspTime - songDspTime) + songPosOffset;
        //计算出当前在哪一拍
        float songPosInBeats = songPosition / secPerBeat;
        //mCycleCount = Mathf.Floor((songPosInBeats) / score.notesPerCycle);
        //Debug.Log("mCycleCount=" + mCycleCount);
        //Debug.Log("mCycleCount * score.notesPerCycle=" + mCycleCount * score.notesPerCycle);

        //生成音符
        if (nextNoteIndex < score.notes.Count && ((score.notes[nextNoteIndex].beat + mCycleCount * score.notesPerCycle) < songPosInBeats + beatsShownInAdvance)) {
            Debug.Log("nextNoteIndex=!!!!!!!!" + nextNoteIndex);
            Debug.Log("mCycleCoun=" + mCycleCount);
            Debug.Log("score.notesPerCycle=" + score.notesPerCycle);

            Debug.Log("beat=" + (score.notes[nextNoteIndex].beat + mCycleCount * score.notesPerCycle));
            //Debug.Log("nextNoteIndex=!!!!!!!!" + nextNoteIndex);
            //Debug.Log("nextNoteIndex=!!!!!!!!" + nextNoteIndex);
            Note tempnote = new Note();
            tempnote.beat = score.notes[nextNoteIndex].beat + mCycleCount * score.notesPerCycle;
            tempnote.type = score.notes[nextNoteIndex].type;
            tempnote.note = (GameObject)Instantiate(Resources.Load("Prefab/Note"), this.transform.position + noteSpawnPos, Quaternion.identity, this.transform);
            nextNoteIndex++;
            mRunningNoteList.Add(tempnote);
        }


        if (songPosInBeats + 1 > mCycleCount * score.notesPerCycle + beatsShownInAdvance)
        {
            nextNoteIndex = 0;
            mCycleCount++;
        }

        if (mRunningNoteList.Count > 0) {
        //拍子正中位置，播放所有正常跟节奏的动画，包括敌人的READY，IDLE动画。
        if (songPosInBeats - mRunningNoteList[0].beat > 0f)
        {
            //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
            BeatCenter();
            ;
        }

        //如果超出范围(0.5是表示以节拍中心向后的时间范围)
        if (songPosInBeats - mRunningNoteList[0].beat > 0.5f)
        {
            //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
            BeatEnd();
            EnemyUpdate();
            mBeatCurrent++;
            BeatStart();
        }
        }

        float mBarPercent = songPosInBeats - ((int)songPosInBeats);

        //调整判定音符的位置

        foreach(Note temp in mRunningNoteList)
        {
            temp.note.transform.localPosition = Vector2.Lerp(
            noteSpawnPos,
            noteEndPos,
            (beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance
        );

            //跳转透明度
            temp.note.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f+(beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance);


            //隐藏已经到达中间的音符
            if ((beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance >= 1)
            {
                temp.note.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
            }
        }


        //mJudge[0].transform.localPosition = new Vector3(mBarLength * (mBarPercent), 0, 0);
        //mJudge[1].transform.localPosition = new Vector3(mBarLength * (-mBarPercent), 0, 0);

        ////调整判定音符的透明度(最靠近的两个在后半拍要隐形,造成连续的视觉效果)

        //if (mBeatLock && mBarPercent>0.5f){
        //    mJudge[0].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
        //    mJudge[1].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
        //}
        //else {
        //    mJudge[0].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f + 0.25f * mBarPercent);
        //    mJudge[1].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f + 0.25f * mBarPercent);
        //}

        //for (int i = 1; i < 4; i++)
        //{
        //    mJudge[0].transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f - 0.25f * i + 0.25f * mBarPercent);
        //    mJudge[1].transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f - 0.25f * i + 0.25f * mBarPercent);
        //}
        //return;
    }
    //每一拍的控制
    public void ShowAction(actionType type){
        //如果已经按过就停止
        if (mBeatLock)
        {
            return;
        }
        //如果没敌人了就先不管
        if (Player.Instance.enemyList.Count<=0){
            return;
        }
        //判定评价
        commentController.CallCommentUpdate(BeatComment());

        switch (type){
            case actionType.None :
                {
                }
                break;
            case actionType.Charge:
                {
                    BeatDone();
                    if (Player.Instance.getHit)
                    {

                    }
                    else {
                        Player.Instance.Charge();
                    }
                }
                break;
            case actionType.Hit:
                {
                    //如果出玩家攻击，就先结算敌人行动再处理玩家行动(敌人攻击会打断玩家)
                    BeatDone();
                    if (Player.Instance.getHit){
                        //Player.Instance.HitFail();
                    }
                    else{
                        if (BeatComment() < 2) {
                            Player.Instance.Hit();
                            Player.Instance.AddMp(2 - BeatComment());
                            Player.Instance.addSoulPoint(2 - BeatComment());
                        }
                        else if (BeatComment() == 3) {
                            Player.Instance.HitFail();
                            Player.Instance.decreaseSoulLevel();
                        }
                    }
                    BeatDone();

                }
                break;
            case actionType.Defense:
                {
                    //如果出玩家防守，就先结算玩家行动再处理敌人行动(玩家加完盾，敌人攻击就会失败)
                    if (BeatComment() < 2) {
                        Player.Instance.Defense();
                        Player.Instance.AddMp(2 - BeatComment());
                        Player.Instance.addSoulPoint(2 - BeatComment());

                    }
                    else if (BeatComment() == 3) {
                        Player.Instance.HitFail();
                        Player.Instance.decreaseSoulLevel();

                    }

                    BeatDone();
                }
                break;
        }



    }

    //暂时没用，不用管这个
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
    //计算当前拍子的位置，影响判定
    private int BeatComment (){
        if (mRunningNoteList.Count==0)
        {
            return 3;
        }
        float songPosition = (float)(AudioSettings.dspTime - songDspTime) + songPosOffset;
        float songPosInBeats = songPosition / secPerBeat;
        float mBarPercent = Mathf.Abs(songPosInBeats - mRunningNoteList[0].beat);

        if (mBarPercent<=0.1f) {
            return 0;
        }
        else if (mBarPercent<=0.25f){
            return 1;
        }
        else {
            return 3;
        }
    }

    //节拍结算(和下面的节拍结束差不多，但是它是由玩家主动操作调用的，下面是回合结束自然调用的)
    public void BeatCenter()
    {
        if (!mCenterLock)
        {

            //Debug.Log("now is the center");
            foreach (GameObject inst in Player.Instance.enemyList)
            {
               // Debug.Log("actionid="+ inst.GetComponent<AI>().actionID);
                if (inst.GetComponent<AI>().actionID !=-1)
                {
                    if (inst.GetComponent<AI>().actionSequence[inst.GetComponent<AI>().actionID] != 2)
                    {
                        inst.GetComponent<AI>().Action();
                    }
                }
                else
                {
                    inst.GetComponent<AI>().Action();

                }
            }
        }
        mCenterLock = true;

    }

    //节拍结算(和下面的节拍结束差不多，但是它是由玩家主动操作调用的，下面是回合结束自然调用的)
    //这里只有攻击动画
    public void BeatDone (){
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

        mBeatLock = true;
    }
    //节拍结束
    public void BeatEnd (){
        if (EnemyCountdown>=0){
            //commentController.CallCommentUpdate(2);
            return;
        }

        if (!mBeatLock){
            commentController.CallCommentUpdate(2);
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
        else {
            mBeatLock = false;
        }
        updateEnemyAction();
        Destroy(mRunningNoteList[0].note.gameObject);
        mRunningNoteList.RemoveAt(0);
        mCenterLock = false;
    }

    //集中更新AI的ActionID
    public void updateEnemyAction()
    {
        foreach (GameObject inst in Player.Instance.enemyList)
        {
            inst.GetComponent<AI>().actionID = (inst.GetComponent<AI>().actionID + 1) % inst.GetComponent<AI>().actionSequence.Length;
        }
    }
    //先不用
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
        Player.Instance.Initialize();
        foreach (GameObject inst in Player.Instance.enemyList)
        {
            inst.GetComponent<AI>().Initialize();
        }
    }

    public void EnemyUpdate (){
        if (EnemyCountdown > 0){
            Debug.Log("Enemy Reborn: "+EnemyCountdown);
            EnemyCountdown--;
        }
        else if (EnemyCountdown == 0){
            BattleController.Instance.AddEnemy();
            EnemyCountdown--;
        }
        else{
            if (Player.Instance.enemyList.Count <= 0)
            {
                Debug.Log("No Enemy");
                EnemyCountdown = 4;
            }
        }

    }

}
