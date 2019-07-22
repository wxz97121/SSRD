using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill Data")]

public class SkillData : ScriptableObject
{
    public SkillType type;
    public Sprite sprite;
    public string _name;
    public int cost;
    public List<Note> inputSequence;
    //public UnityEvent EffectEvent;
    public string Effect;
    public int CD;
    public string Desc;
}

