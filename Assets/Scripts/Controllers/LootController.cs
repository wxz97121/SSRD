using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    Item,
    Skill,
    Talent
}

public class LootOption
{
    LootType type;
    int id;

    public void Activate()
    {
        switch (type)
        {
            case LootType.Item:
                {
                    var equipment = Resources.Load<Equipment>("Data/Equipment/"+id.ToString());
                    Player.Instance.Equip(equipment);
                }
                break;
            case LootType.Skill:
                {
                    //var skill = Resources.Load<SkillData>("Data/Skill/" + id.ToString());
                    //Player.Instance.Equip(equipment);
                }
                break;
        }
    }

    public LootOption (LootType t, int i)
    {
        type = t;
        id = i;
    }

    public string GetName()
    {
        switch (type)
        {
            case LootType.Item:
                {
                    var equipment = Resources.Load<Equipment>("Data/Equipment/" + id.ToString());
                    return equipment.equipName;
                }
            case LootType.Skill:
                {
                    //var skill = Resources.Load<SkillData>("Data/Skill/" + id.ToString());
                    //Player.Instance.Equip(equipment);
                }
                break;
        }
        return "";
    }

    public string GetDesc()
    {
        switch (type)
        {
            case LootType.Item:
                {
                    var equipment = Resources.Load<Equipment>("Data/Equipment/" + id.ToString());
                    return equipment.equipDesc;
                }
                break;
            case LootType.Skill:
                {
                    //var skill = Resources.Load<SkillData>("Data/Skill/" + id.ToString());
                    //Player.Instance.Equip(equipment);
                }
                break;
        }
        return "";
    }
}

public class LootController : MonoBehaviour
{
    protected static LootController _instance;
    public static LootController Instance
    {
        get
        {
            return _instance;
        }
    }

    public Transform[] optionSlot;
    public GameObject[] optionView;
    private void Awake()
    {
        _instance = this;
    }
    public Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewLoot()
    {
        SuperController.Instance.state = GameState.Loot;

        canvas.gameObject.SetActive(true);
        Debug.Log(optionSlot.Length);
        for (int i = 0; i < optionSlot.Length; i++)
        {
            var opt = new LootOption(LootType.Item, i);
            optionView[i] = Instantiate(Resources.Load<GameObject>("Prefab/LootOption"), optionSlot[i]);
            optionView[i].GetComponent<LootOptionView>().SetOption(opt);
        }
    }

    public void EndLoot()
    {
        for (int i = 0; i < optionView.Length; i++)
        {
            Destroy(optionView[i]);
        }

        canvas.gameObject.SetActive(false);

        StartCoroutine("StateDelay");
    }

    public IEnumerator StateDelay()
    {
        yield return new WaitForSeconds(1f);
        SuperController.Instance.Resume();
        //SuperController.Instance.state = GameState.Start;
    }
}
