using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemySkillData", menuName = "Enemy Skill Data")]

public class EnemySkillData : ScriptableObject
{
    public string _name;
    public string Effect;
}

