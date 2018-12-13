using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//行动类别
public enum actionType {
    None = 0,
    Charge = 1,
    Hit = 2,
    Defense = 3,
    Collect = 4

}
//拍子类别，目前没用留作拓展
public enum beatType
{
    Normal = 0
}

//节拍总控制
public class BarController : MonoBehaviour {

    //单例
    static BarController _instance;
    //节拍条
    public GameObject mBar;
    //节拍条长度
    public float mBarLength;

    //歌曲开始时间(用来处理当前节奏)
    [HideInInspector] public float songDspTime;
    //每一拍长度
    [HideInInspector] public float secPerBeat;

    //开始位置
    private Vector3 noteSpawnPos_main = new Vector3(600, 0, 0);
    //结束位置
    private Vector3 noteEndPos_main = new Vector3(0, 0, 0);
    //当前乐谱
    public Score score_main = new Score();
    //音符计数
    public int nextNoteIndex_main = 0;

    //开始位置
    private Vector3 noteSpawnPos_energy = new Vector3(600, 60, 0);
    //结束位置
    private Vector3 noteEndPos_energy = new Vector3(0, 60, 0);
    //当前乐谱
    public Score score_energy = new Score();
    //音符计数
    public int nextNoteIndex_energy = 0;


    //提前展示多少拍
    public float beatsShownInAdvance = 4f;
    //记录当前是第几个循环
    public float mCycleCount_main = 0f;
    public float mCycleCount_energy = 0f;

    //当前拍
    public int mBeatCurrent;
    //拍列表
    private List<Note> mRunningNoteList_main = new List<Note>();
    //当前拍
    public int mBeatCurrent_energy;
    //拍列表
    private List<Note> mRunningNoteList_energy = new List<Note>();
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

    //拍锁(这一拍按过就不能按了)
    public bool mBeatLock_energy = false;
    //中间锁(用在beatCenter里，防止动画重复播放)
    public bool mCenterLock_energy = false;
    //结束锁(当前没用，之前是为了区分拍子完结的两种状态：自然完结和玩家敲击完结)
    public bool mEndLock_energy = false;
    //评价控制(评价控制还没改成全局控制)
    public CommentController commentController = null;

    //敌人复活倒计时(暂时用的，四拍以后招新敌人)
    public int EnemyCountdown = -1;

    //校正(校准因为视觉产生的节拍误差)
    public float songPosOffset = 0f;

    
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
        score_main.notesPerCycle = 8f;
        for (int i = 0; i < 4; i++)
        {
            Note temp = new Note();
            temp.beat = (float)(i * 2 + 0.5);
            temp.note = (GameObject)Resources.Load("Prefabs/Note");
            temp.type = Note.NoteType.action;
            score_main.notes.Add(temp);
        }

        score_energy.notesPerCycle = 8f;
        for (int i = 0; i < 16; i++)
        {
            Note temp = new Note();
            temp.beat = (float)(i*0.5);
            temp.note = (GameObject)Resources.Load("Prefabs/Note");
            temp.type = Note.NoteType.action;
            score_energy.notes.Add(temp);
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


    // 重置节拍条(重新计算bpm,归零，播放歌曲，开始游戏等)
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

        //生成音符_主轨道
        if (nextNoteIndex_main < score_main.notes.Count && ((score_main.notes[nextNoteIndex_main].beat + mCycleCount_main * score_main.notesPerCycle) < songPosInBeats + beatsShownInAdvance)) {

            Note tempnote = new Note();
            tempnote.beat = score_main.notes[nextNoteIndex_main].beat + mCycleCount_main * score_main.notesPerCycle;
            tempnote.type = score_main.notes[nextNoteIndex_main].type;
            tempnote.note = (GameObject)Instantiate(Resources.Load("Prefab/Note"), this.transform.position + noteSpawnPos_main, Quaternion.identity, this.transform);
            nextNoteIndex_main++;
            mRunningNoteList_main.Add(tempnote);
        }
        if (songPosInBeats + beatsShownInAdvance > (mCycleCount_main + 1) * score_main.notesPerCycle)
        {
            nextNoteIndex_main = 0;
            mCycleCount_main++;
        }
        //生成音符_能量轨道
        if (nextNoteIndex_energy < score_energy.notes.Count && ((score_energy.notes[nextNoteIndex_energy].beat + mCycleCount_energy * score_energy.notesPerCycle) < songPosInBeats + beatsShownInAdvance))
        {

            Note tempnote = new Note();
            tempnote.beat = score_energy.notes[nextNoteIndex_energy].beat + mCycleCount_energy * score_energy.notesPerCycle;
            tempnote.type = score_energy.notes[nextNoteIndex_energy].type;
            tempnote.note = (GameObject)Instantiate(Resources.Load("Prefab/Note"), this.transform.position + noteSpawnPos_energy, Quaternion.identity, this.transform);
            nextNoteIndex_energy++;
            mRunningNoteList_energy.Add(tempnote);
        }
        if (songPosInBeats + beatsShownInAdvance > (mCycleCount_energy+1 )* score_energy.notesPerCycle)
        {
            nextNoteIndex_energy = 0;
            mCycleCount_energy++;
        }


        //判定音符是否到位，触发相应函数
        if (mRunningNoteList_main.Count > 0) {
        //拍子正中位置，播放所有正常跟节奏的动画，包括敌人的READY，IDLE动画。
            if (songPosInBeats - mRunningNoteList_main[0].beat > 0f)
            {
                //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
                BeatCenter();
                
            }

            //如果超出范围(0.5是表示以节拍中心向后的时间范围)
            if (songPosInBeats - mRunningNoteList_main[0].beat > 0.5f)
            {
                //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
                BeatEnd();
                EnemyUpdate();
                mBeatCurrent++;
                BeatStart();
            }
        }

        if (mRunningNoteList_energy.Count > 0)
        {

            //如果超出范围(0.2是表示以节拍中心向后的时间范围)
            if (songPosInBeats - mRunningNoteList_energy[0].beat > 0.2f)
            {
                //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
               BeatEnd_energy();
            }
        }


        //调整判定音符的位置
        //主轨道
        foreach (Note temp in mRunningNoteList_main)
        {
            temp.note.transform.localPosition = Vector2.Lerp(
            noteSpawnPos_main,
            noteEndPos_main,
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
        //副轨道
        foreach (Note temp in mRunningNoteList_energy)
        {
            temp.note.transform.localPosition = Vector2.Lerp(
            noteSpawnPos_energy,
            noteEndPos_energy,
            (beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance
        );

            //跳转透明度
            temp.note.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f + (beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance);


            //隐藏已经到达中间的音符
            if ((beatsShownInAdvance - (temp.beat - songPosInBeats)) / beatsShownInAdvance >= 1)
            {
                temp.note.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
            }
        }


    }

    //每一拍的控制-主轨道
    public void ShowAction_main(actionType type){
        Debug.Log("action LOCK:"+ mBeatLock.ToString());
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
        commentController.CallCommentUpdate(BeatComment(mRunningNoteList_main));

        switch (type){
            case actionType.None :
                {
                }
                break;
            case actionType.Charge:
                {
                    BeatDone_EnemyAct();
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
                    //BeatDone_EnemyAct();
                    if (Player.Instance.getHit){
                        //Player.Instance.HitFail();
                    }
                    else{
                        if (BeatComment(mRunningNoteList_main) < 2) {
                            Player.Instance.Hit();
                            Player.Instance.addSoulPoint(2 - BeatComment(mRunningNoteList_main));
                        }
                        else if (BeatComment(mRunningNoteList_main) == 3) {
                            bad(0);

                        }
                    }
                    BeatDone_EnemyAct();

                    mBeatLock = true;

                }
                break;
            case actionType.Defense:
                {
                    //如果出玩家防守，就先结算玩家行动再处理敌人行动(玩家加完盾，敌人攻击就会失败)
                    if (BeatComment(mRunningNoteList_main) < 2) {
                        Player.Instance.Defense();
                        Player.Instance.addSoulPoint(2 - BeatComment(mRunningNoteList_main));

                    }
                    else if (BeatComment(mRunningNoteList_main) == 3) {
                        bad(0);
                    }

                    BeatDone_EnemyAct();
                }
                break;
        }

    

    }
    public void ShowAction_energy(actionType type)
    {
        Debug.Log("ENERGY");
        //如果已经按过就停止
        if (mBeatLock_energy)
        {
            return;
        }


        //如果没敌人了就先不管
        if (Player.Instance.enemyList.Count <= 0)
        {
            return;
        }
        switch (type)
        {

            case actionType.Collect:
                {
                    if (BeatComment(mRunningNoteList_energy) < 2)
                    {
                        Player.Instance.AddMp(1);

                    }
                    else if (BeatComment(mRunningNoteList_energy) == 3)
                    {
                        bad(1);
                    }
                    mBeatLock_energy = true;
                }
                break;
            
        }



    }

    //计算当前拍子的位置，影响判定
    //0为main
    //1为energy
    private int BeatComment (List<Note> notelist){
        if (notelist.Count==0)
        {
            return 3;
        }
        float songPosition = (float)(AudioSettings.dspTime - songDspTime) + songPosOffset;
        float songPosInBeats = songPosition / secPerBeat;
        float mBarPercent = Mathf.Abs(songPosInBeats - notelist[0].beat);

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
    public void BeatDone_EnemyAct (){
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
        Destroy(mRunningNoteList_main[0].note.gameObject);
        mRunningNoteList_main.RemoveAt(0);
        mCenterLock = false;

        Player.Instance.OnTurnEnd();
    }

    public void BeatEnd_energy()
    {
        //清除已经过去的NOTE
        mBeatLock_energy = false;

        Destroy(mRunningNoteList_energy[0].note.gameObject);
        mRunningNoteList_energy.RemoveAt(0);
    }


    //集中更新AI的ActionID
    public void updateEnemyAction()
    {
        foreach (GameObject inst in Player.Instance.enemyList)
        {
            inst.GetComponent<AI>().actionID = (inst.GetComponent<AI>().actionID + 1) % inst.GetComponent<AI>().actionSequence.Length;
        }
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

    //输入错误
    //0为Main
    //1为Energy
    public void bad(int type)
    {
        switch(type){

        case 0:
        Player.Instance.HitFail();
        Player.Instance.decreaseSoulLevel();
        mRunningNoteList_main[0].note.transform.localScale = Vector3.zero;
            break;
        case 1:
            Player.Instance.decreaseSoulLevel();
            mRunningNoteList_energy[0].note.transform.localScale = Vector3.zero;
                Debug.Log("bad");
            break;
        }

    }


}
