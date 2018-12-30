using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    //谱子
    public OneSongScore score;
    //临时技能
    public List<Skill> playerSkills;

    #region 单例
    static LevelData _instance;
    private void Awake()
    {
        _instance = this;

    }
    public static LevelData Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public void ReadSkillDatas()
    {
        Player.Instance.skills = new List<Skill>();

        Player.Instance.skills = playerSkills;
        InputSequenceController.Instance.skills = Player.Instance.skills;
        InputSequenceController.Instance.availableSkills = InputSequenceController.Instance.skills;
        
    }

    // Use this for initialization
   public void ReadScoreDatas()
    {
        score = ReadScoreData("score_1_test");

        playerSkills = new List<Skill>
        {
            new Skill("testSkill_ZZX_SUPERATTACK"),
            new Skill("testSkill_0ZX_DEFEND"),
            new Skill("testSkill_00X_ATTACK"),
            new Skill("testSkill_0Z0ZZX_ULTI")
        };






    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 写入score ReadScoreData(string scorename)
    public OneSongScore ReadScoreData(string scorename)
    {
        Debug.Log("start reading score");

        OneSongScore _score = new OneSongScore
        {
            mainlude = new List<OneBarScore>(),
            prelude = new List<OneBarScore>()
        };

        ScoreData data = Resources.Load("Data/Score/" + scorename) as ScoreData;


        //_score.mainlude = data.mainlude;
        for (int i = 0; i < data.mainlude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.mainlude[i].beatsThisBar,
                notes = new List<Note>()
            };
            for (int j = 0; j < data.mainlude[i].notes.Count; j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.mainlude[i].notes[j].type,
                    beatInBar = data.mainlude[i].notes[j].beatInBar
                });
                //                Debug.Log("added mainlude note ");
            }
            _score.mainlude.Add(_onebarscore);

        }

        //_score.prelude = data.prelude;

        for (int i = 0; i < data.prelude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.prelude[i].beatsThisBar,
                notes = new List<Note>()
            };
            for (int j = 0; j < data.prelude[i].notes.Count; j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.prelude[i].notes[j].type,
                    //_note.note = data.mainlude[j].notes[j].note;
                    beatInBar = data.prelude[i].notes[j].beatInBar
                });
            }
            _score.prelude.Add(_onebarscore);

        }

        _score.bgmusic = data.bgmusic;

        return _score;
    }
    #endregion



    #region 旧的读取skill，弃用 
    //public Skill ReadSkillData(string skillname)
    //{
    //    SkillData data = Resources.Load("Data/Skill/" + skillname) as SkillData;

    //    Skill _skill = new Skill()
    //    {
    //        name = data._name,
    //        inputSequence = new List<Note>(),
    //        EffectEvent = data.EffectEvent
    //        //effects = new List<Effect>(),

    //    };

    //    for (int i = 0; i < data.inputSequence.Count; i++)
    //    {
    //        _skill.inputSequence.Add(new Note
    //        {
    //            type = data.inputSequence[i].type,
    //            beatInBar = data.inputSequence[i].beatInBar
    //        }
    //        );
    //    }


    //    return _skill;
    //}

    #endregion
}
