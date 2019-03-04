using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkillTipBar : MonoBehaviour {

    public Text testtext;
    public Text costtext;

    public bool active;
    //开始拍
    public float startBeat;
    //本小节拍子数
    public float beatsThisBar;
    //指针
    public GameObject pin;
    //背景
    public Image bg;

    public GameObject startPosGO;
    public GameObject onebeatspaceGO;
    public Vector3 startPos;
    public Vector3 oneBeatSpace;

    //小节线
    public List<GameObject> linelist;
    //音符
    public List<Note> noteList_main;

    public List<Note> noteList_input;
    private void Awake()
    {
    // 小节线
    linelist = new List<GameObject>();
    //音符
    noteList_main = new List<Note>();

    noteList_input = new List<Note>();
    startPos = startPosGO.transform.localPosition;
    oneBeatSpace = onebeatspaceGO.transform.localPosition-startPos;
    }


    //初始化小节块
    public void Init()
    {
       InitLines();
        InitNotes();
    }


    //将音符收进轨道
    public void ReadScoreFromSkill(List<Note> notes)
    {
        foreach (Note note in notes)
        {
            if ((int)note.type <= 99)
            {

                noteList_input.Add(new Note
                {
                    type = note.type,
                    beatInBar = note.beatInBar,
                }
                );
            }

        }
    }

    //画节拍线
    public void InitLines()
    {
        //        int _linecount = (int)(beatsThisBar);
        for (int i=0; i<=2;i++)
        {
            GameObject line = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Line", typeof(GameObject)), transform);
            line.transform.localPosition = startPos + (oneBeatSpace * i);
            line.transform.localScale = new Vector3(1,0.6f,1);
            line.GetComponent<Image>().color = Color.black;
            //第三拍变红提示
            if (i == 2) { line.GetComponent<Image>().color = Color.red; }
            linelist.Add(line);
        }

    }

    //画原有音符
    public void InitNotes()
    {
        foreach (Note note in noteList_input)
        {
          //  GameObject _note = new GameObject();
            if (note.type == Note.NoteType.inputBassdrum)
            {
                GameObject _note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Bassdrum", typeof(GameObject)), transform);
                note.note = _note;

            }
            else
            {
                GameObject _note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Snare", typeof(GameObject)), transform);
                note.note = _note;

            }
            note.note.transform.localPosition = startPos + (oneBeatSpace * ( note.beatInBar))+new Vector3(0,0,0);

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
        testtext.text = "barposinbeat" + barposinbeat.ToString("F2")+"/"+ (beatsThisBar);

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
        Color tempcolor = pin.GetComponent<Image>().color;
        pin.GetComponent<Image>().color = new Color(tempcolor.r,tempcolor.g,tempcolor.b,a);
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

        foreach (Note n in noteList_input)
        {
            if(n.note)
            {
                n.note.GetComponent<Image>().color = new Color(n.note.GetComponent<Image>().color.r, n.note.GetComponent<Image>().color.g, n.note.GetComponent<Image>().color.b, alpha);
            }
        }
    }

    //重置
    public void Empty()
    {
        int tempcount = noteList_input.Count;
        for (int i = 0; i < tempcount; i++)
        {
            Destroy(noteList_input[0].note);
            noteList_input.RemoveAt(0);
        }
        int tempcountline = linelist.Count;
        for (int i = 0; i < tempcountline; i++)
        {
            Destroy(linelist[0].gameObject);
            linelist.RemoveAt(0);
        }
        pin.transform.localPosition = startPos;
       
    }

    public void AddRightO(int index)
    {
       Instantiate((GameObject)Resources.Load("VFX/RightO", typeof(GameObject)), noteList_input[index].note.transform);

    }

    public void RemoveRightO()
    {
        foreach (var note in noteList_input)
        {

            if (note.note.transform.Find("RightO(Clone)") != null)
                Destroy(note.note.transform.Find("RightO(Clone)").gameObject);
        }
    }

    public void RemoveRightOWhenSuccess()
    {
        foreach (var note in noteList_input)
        {

            if (note.note.transform.Find("RightO(Clone)") != null)
            {
                note.note.transform.Find("RightO(Clone)").GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                note.note.transform.Find("RightO(Clone)").gameObject.name = "fadingRightO";
            }
        }
    }
}
