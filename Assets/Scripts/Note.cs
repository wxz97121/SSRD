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
        energy = 1,
        energy_double = 2,
        charge = 9,
        action = 10,
    }

    public NoteType type;public GameObject note;

    //节拍位置
    public float beat;
}
