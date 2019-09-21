using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]

public class LevelData : ScriptableObject
{
    public string AreaName = "安贞西里";
    public ScoreData scoreData;
    public List<string> enemyList;
    public string BGMPath;
    public AreaType LevelType;
    public SkillData AwardSkill;
    public Equipment AwardEquip;
    public int AwardMoney;
    public NovelScript PreStory;
	public NovelScript AfterStory;
    public string NextStoryStep;
}
