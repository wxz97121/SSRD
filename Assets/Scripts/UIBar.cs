using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBar : MonoBehaviour {

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

    //画节拍线
    public void InitLines()
    {
        int _linecount = (int)(beatsThisBar);
        for (int i=0; i<_linecount;i++)
        {
            GameObject line = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Line", typeof(GameObject)), transform);
            line.transform.localPosition = startPos + oneBeatSpace * (i + 1);
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
            note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note", typeof(GameObject)), transform);
            note.note.transform.localPosition = startPos + (oneBeatSpace * (1f + note.beat));

        }

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
    }
}
