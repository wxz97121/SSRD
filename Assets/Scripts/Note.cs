using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note{

    /*音符类型
     1-能量
     2-大能量
     10-动作
     */
    public enum noteType
    {
        energy = 1,
        bigenergy = 2,
        action = 10,
    }

    public noteType type;
    public GameObject note;

    //节拍位置
    public float beat;
}
