using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBar : MonoBehaviour {

    public float length;
    //指针
    public GameObject pin;
    //背景
    public Image bg;

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
    }
}
