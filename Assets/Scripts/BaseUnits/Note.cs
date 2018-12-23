using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Note{

    /*音符类型
     1-能量
     2-大能量
     9-蓄力    
     10-动作
     */
    public enum NoteType
    {
        energy = 1,//能量+！
        energy_double = 2,//能量+2
        charge = 9,//蓄力 能量-1
        action = 10,//行动 能量-1

        inputBassdrum=21,//输入，底鼓
        inputSnare=22,//输入，军鼓
    }

    public NoteType type;
    public GameObject note;
    //副note，用于显示两个小节公用的音符，蓄力专用
    public GameObject subnote;
    //节拍位置
    public float beatInBar;
    public float beatInSong;

}
