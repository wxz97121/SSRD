using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill Data")]

public class SkillData : ScriptableObject
{
    public string _name;
    public List<Note> inputSequence;
    //public UnityEvent EffectEvent;
    public string Effect;
}

