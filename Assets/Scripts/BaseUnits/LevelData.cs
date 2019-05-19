using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]

public class LevelData : ScriptableObject
{
    public ScoreData scoreData;
    public List<string> enemyList;
    public string BGMPath;
}
