using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScoreData", menuName = "Score Data")]

public class ScoreData : ScriptableObject {
    public AudioClip bgmusic;
    public List<OneBarScore> prelude;
    public List<OneBarScore> mainlude;

}
