using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能组，4个一组，用来存敌人每回合的动作
[System.Serializable]

public struct EnemySkillGroup
{
    public string name;
    public List<string> enemySkills;
}
