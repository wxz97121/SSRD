using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBar : MonoBehaviour {

    public enum barType
    {
        inputBar=1,
        QTEBar=2,
        ULTIBar=3,
    }
    public barType type = barType.inputBar;
    public Text testtext;
    public bool active;
    //开始拍
    public float startBeat;
    //本小节拍子数
    public float beatsThisBar;
    //指针
    public GameObject pin;
    //背景
    public Image bg;
    public Sprite bgNORMAL;
    public Sprite bgQTE;
    public Sprite bgULTI;

    public GameObject startPosGO;
    public GameObject onebeatspaceGO;
    public Vector3 startPos;
    public Vector3 oneBeatSpace;

    public UIBarEnemyWarn enemyWarn;

    //小节线
    public List<GameObject> linelist;
    //音符
    public List<Note> noteList_main;

    public List<Note> noteList_energy;
    public List<Note> noteList_QTE;
    private void Awake()
    {
    // 小节线
    linelist = new List<GameObject>();
    //音符
    noteList_main = new List<Note>();

    noteList_energy = new List<Note>();
    noteList_QTE = new List<Note>();

    startPos = startPosGO.transform.localPosition;
    oneBeatSpace = onebeatspaceGO.transform.localPosition-startPos;
    }


    //初始化小节块
    public void Init()
    {
        if (type == barType.QTEBar)
        {
            bg.sprite = bgQTE;
        }
        if (type == barType.inputBar)
        {
            //bg.color = Color.blue;
            bg.sprite=bgNORMAL;
        }
        
        InitLines();
        InitNotes();
        enemyWarn.transform.localScale = new Vector3(0, 0, 0);
    }


    //将音符收进两个轨道
    public void ReadScore(List<Note> notes)
    {
      // Debug.Log("start readscore in a bar:"+notes.Count);
        foreach (Note note in notes)
        {
//            Debug.Log("notetype="+note.type);
            //能量音符
            if ((int)note.type <= 8)
            {

                noteList_energy.Add(new Note
                {
                    type = note.type,
                    beatInBar = note.beatInBar,
                    beatInSong = note.beatInBar + startBeat,
                }
                );
            }

            //QTE音符
            if ((int)note.type >= 30&& (int)note.type<40)
            {

                noteList_QTE.Add(new Note
                {
                    type = note.type,
                    beatInBar = note.beatInBar,
                    //beatInSong = note.beatInBar + UIBarController.Instance.occupiedBeats,
                    beatInSong = note.beatInBar + startBeat,

                    SuccessSkill = note.SuccessSkill,
                    MissSkill=note.MissSkill,
                    BadSkill=note.BadSkill
                }
                );
            }

        }
    }

    //画节拍线
    public void InitLines()
    {
//        Debug.Log("init lines");
        int _linecount = (int)(beatsThisBar);
        for (int i=0; i<=_linecount;i++)
        {
            GameObject line = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Line", typeof(GameObject)), transform);
            line.transform.localPosition = startPos + (oneBeatSpace * i);
            //line.GetComponent<Image>().color = Color.black;
            //第三拍变红提示
            if (i == 2&&type==barType.inputBar) { line.GetComponent<Image>().color = new Color(0.5f,0,0); }
            //QTE变白
            //if (type == barType.QTEBar) { line.GetComponent<Image>().color = Color.white; }
            linelist.Add(line);
        }

    }

    //画原有音符
    public void InitNotes()
    {
//        Debug.Log("init notes");

        foreach (Note note in noteList_energy)
        {
            GameObject _note= Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Energy", typeof(GameObject)), transform);
            note.note = _note;
            note.note.transform.localPosition = startPos + (oneBeatSpace * ( note.beatInBar))+new Vector3(0,10,0);
        }


        //QTE音符
        foreach (Note note in noteList_QTE)
        {
            string path;
            switch (note.type)
            {
                case Note.NoteType.QTEHihat:
                    path = "Prefab/UI/Bar/UI_Bar_QTE_Note_Energy";
                    break;
                case Note.NoteType.QTESnare:
                    path = "Prefab/UI/Bar/UI_Bar_QTE_Note_Snare";
                    break;
                case Note.NoteType.QTEBassdrum:
                    path= "Prefab/UI/Bar/UI_Bar_QTE_Note_Bassdrum";
                    break;
                default:
                    path = "error path";
                    break;
            }
//            Debug.Log("init note path ="+path);
            GameObject _note = Instantiate((GameObject)Resources.Load(path, typeof(GameObject)), transform);
            note.note = _note;
            note.note.transform.localPosition = startPos + (oneBeatSpace * (note.beatInBar)) + new Vector3(0, 10, 0);
        }
    }

    //指针移动
    public void PinMoving(float barposinbeat)
    {
        // Debug.Log("barpos="+ barposinbeat);
        // Debug.Log("beatsThisBar=" + beatsThisBar);
        if (!active)
        {
            return;
        }
        testtext.text = "starttime" + startBeat;

        //处理位置
        pin.transform.localPosition = Vector2.Lerp
        (
            startPos- (oneBeatSpace),
            startPos+ (oneBeatSpace *(beatsThisBar+0.3f)),
            (barposinbeat+ 1) / (beatsThisBar+ 1.3f)
            
        );


        //处理透明度 两头渐隐
        if (barposinbeat<-1)
        {
            SetPinAlpha(0);

        }
        else if(barposinbeat<0)
        {
            float a = Mathf.Lerp(
            1,
            0,
            -barposinbeat / 1
            );
            SetPinAlpha(a);
        }else if(barposinbeat> beatsThisBar)
        {
            float a = Mathf.Lerp(
            1,
            0,
            (barposinbeat-beatsThisBar) / 0.3f
            );
            SetPinAlpha(a);
        }
        else
        {

            SetPinAlpha(1);

        }
    }

    public void SetPinAlpha(float a)
    {
        pin.GetComponent<UIBarPin>().SetAlpha(a);
        //Color tempcolor = pin.GetComponent<Image>().color;
        //pin.GetComponent<Image>().color = new Color(tempcolor.r,tempcolor.g,tempcolor.b,a);
    }


    //设置显示透明度
    public void SetAlpha(float alpha)
    {
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alpha);
        //SetPinAlpha(alpha);
        foreach(GameObject line in linelist)
        {
            line.GetComponent<Image>().color = new Color(line.GetComponent<Image>().color.r, line.GetComponent<Image>().color.g, line.GetComponent<Image>().color.b, alpha);
        }

        foreach (Note n in noteList_energy)
        {
            if(n.note)
            {
                n.note.GetComponent<Image>().color = new Color(n.note.GetComponent<Image>().color.r, n.note.GetComponent<Image>().color.g, n.note.GetComponent<Image>().color.b, alpha);
            }
        }

        foreach (Note n in noteList_QTE)
        {
            if (n.note)
            {
                n.note.GetComponent<Image>().color = new Color(n.note.GetComponent<Image>().color.r, n.note.GetComponent<Image>().color.g, n.note.GetComponent<Image>().color.b, alpha);
            }
        }

        enemyWarn.SetAlpha(alpha);
    }

    //重置
    public void Empty()
    {
        int tempcount = noteList_energy.Count;
        for (int i = 0; i < tempcount; i++)
        {
            Destroy(noteList_energy[0].note);
            noteList_energy.RemoveAt(0);
        }

        int tempcountqte = noteList_QTE.Count;
        for (int i = 0; i < tempcountqte; i++)
        {
            Destroy(noteList_QTE[0].note);
            noteList_QTE.RemoveAt(0);
        }

        int tempcountline = linelist.Count;
        for (int i = 0; i < tempcountline; i++)
        {
            Destroy(linelist[0].gameObject);
            linelist.RemoveAt(0);
        }
        pin.transform.localPosition = startPos;
       
    }


    //创建已输入音符，要自动贴合到最近的节拍上（最小刻度 16分音符）
    public Note AddInputNote(Note.NoteType inputType,float beat)
    {
        Note note = new Note
        {
            type = inputType,
            beatInBar = beat,
        };
        if (inputType == Note.NoteType.inputBassdrum)
        {
            SoundController.Instance.PlayAudioEffect("KICK");
            //SoundController.Instance.PlayOneShot("event:/instruments/bassdrum");

            note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Bassdrum", typeof(GameObject)), transform);

        }
        else
        {

            //SoundController.Instance.PlayOneShot("event:/instruments/snare");
            SoundController.Instance.PlayAudioEffect("SNARE");

            note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Snare", typeof(GameObject)), transform);
        }
        float beatmodpos = (beat % 0.5f < 0.25f) ? beat - beat % 0.5f : beat - beat % 0.5f + 0.5f;
        note.note.transform.localPosition = startPos + (oneBeatSpace * beatmodpos) + new Vector3(0, -10, 0);
        noteList_main.Add(note);
        return note;
    }
}
