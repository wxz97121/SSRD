using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScriptOption
{
    public string label;
    public NovelScript result;
}

[System.Serializable]
public class ScriptLine
{
    public bool left = true;
    public Sprite sprite;
    public string name;
    public string content;
    public string[] effstring;
    public ScriptOption[] options;
}

[CreateAssetMenu(fileName = "ScriptData", menuName = "Script Data")]
public class NovelScript: ScriptableObject
{
    public ScriptLine[] lines;
}
