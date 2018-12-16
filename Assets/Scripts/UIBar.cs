using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBar : MonoBehaviour {

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

    public List<Note> noteList_energy;
    private void Awake()
    {
    // 小节线
    linelist = new List<GameObject>();
    //音符
    noteList_main = new List<Note>();

    noteList_energy = new List<Note>();
    startPos = startPosGO.transform.localPosition;
    oneBeatSpace = onebeatspaceGO.transform.localPosition-startPos;
    }


    //初始化小节块
    public void Init()
    {
        InitLines();
        InitNotes();
    }


    //将音符收进两个轨道
    public void ReadScore(List<Note> notes)
    {
        foreach (Note note in notes)
        {
            if ((int)note.type <= 8)
            {

                noteList_energy.Add(new Note
                {
                    type = note.type,
                    beat = note.beat
                }
                );
            }

        }
    }

    //画节拍线
    public void InitLines()
    {
        int _linecount = (int)(beatsThisBar);
        for (int i=0; i<_linecount;i++)
        {
            GameObject line = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Line", typeof(GameObject)), transform);
            line.transform.localPosition = startPos + oneBeatSpace * (i);
            //第三拍变红提示
            if (i == 2) { line.GetComponent<Image>().color = Color.red; }
            linelist.Add(line);
        }
    }

    //画原有音符
    public void InitNotes()
    {
        foreach (Note note in noteList_energy)
        {
            GameObject _note= Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note", typeof(GameObject)), transform);
            note.note = _note;
            note.note.transform.localPosition = startPos + (oneBeatSpace * ( note.beat));

        }

    }

    //指针移动
    public void PinMoving(float barposinbeat)
    {
        Debug.Log("pinmove");
        Debug.Log("barpos="+ barposinbeat);
        Debug.Log("beatsThisBar=" + beatsThisBar);

        pin.transform.localPosition = Vector2.Lerp
        (
            startPos,
            startPos+beatsThisBar* oneBeatSpace,
            barposinbeat/beatsThisBar

        );
    }


    //设置显示透明度
    public void SetAlpha(float alpha)
    {

        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alpha);
        pin.GetComponent<Image>().color = new Color(pin.GetComponent<Image>().color.r, pin.GetComponent<Image>().color.g, pin.GetComponent<Image>().color.b, alpha);
        foreach(GameObject line in linelist)
        {
            line.GetComponent<Image>().color = new Color(line.GetComponent<Image>().color.r, line.GetComponent<Image>().color.g, line.GetComponent<Image>().color.b, alpha);
        }

        foreach (Note n in noteList_energy)
        {
            n.note.GetComponent<Image>().color = new Color(n.note.GetComponent<Image>().color.r, n.note.GetComponent<Image>().color.g, n.note.GetComponent<Image>().color.b, alpha);
        }
    }

    //重置
    public void Empty()
    {
        int tempcount = noteList_energy.Count;
        for(int i=0; i < tempcount; i++)
        {
            Destroy(noteList_energy[0].note);
            noteList_energy.RemoveAt(0);
        }
        pin.transform.localPosition = startPos;
       
    }
}
