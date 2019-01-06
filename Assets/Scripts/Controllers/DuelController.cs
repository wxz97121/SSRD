using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//战斗逻辑
public class DuelController : MonoBehaviour {

    public List<string> enemyList;
    //敌人总数
    public int enemyNum = 0;
    //当前敌人序号
    public int enemyIndex = 0;


    //记录玩家在行动拍是否输入过，其实是个锁
    public bool isInputedAtActionBeat = false;

    //ui控制
    public Transform UICanvas;

    //敌人的ui模板
    public GameObject enemyUIPrefab;

    #region 单例
    static DuelController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static DuelController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
        UICanvas = GameObject.Find("Canvas").transform;

    }

    // Update is called once per frame
    void Update () {
		
	}




    public void EnemyRespawn()
    {

        if(Player.Instance.enemyList.Count==0)
            AddEnemy();

    }

    //public void AddEnemy()
    //{
    //    GameObject instEnemy = Instantiate((GameObject)Resources.Load("Prefab/Enemy/" + enemyIndex.ToString(), typeof(GameObject)), new Vector3(5, 0, 0), Quaternion.identity);
    //    enemyIndex = (enemyIndex + 1) % enemyNum;
    //    GameObject instEnemyUI = Instantiate(enemyUIPrefab, UICanvas);
    //    instEnemyUI.transform.localPosition += new Vector3(Screen.width / 4, Screen.height / 4, 0);
    //    instEnemy.GetComponent<AI>().UIHpNum = instEnemyUI.transform.Find("HpNum").GetComponent<TextMeshProUGUI>();
    //    instEnemy.GetComponent<AI>().UIMpNum = instEnemyUI.transform.Find("MpNum").GetComponent<TextMeshProUGUI>();
    //    instEnemy.GetComponent<AI>().Init();

    //    Player.Instance.enemyList.Add(instEnemy);
    //    Player.Instance.mTarget = instEnemy;
    //}

    public void AddEnemy()
    {
        GameObject instEnemy = Instantiate((GameObject)Resources.Load("Data/AI/" + enemyList[enemyIndex] + "/Prefab", typeof(GameObject)), new Vector3(5, 0, 0), Quaternion.identity);
        GameObject instEnemyUI = Instantiate(enemyUIPrefab, UICanvas);
        instEnemyUI.transform.localPosition += new Vector3(Screen.width / 4, Screen.height / 4, 0);
        instEnemy.GetComponent<AI>().UIHpNum = instEnemyUI.transform.Find("HpNum").GetComponent<TextMeshProUGUI>();
        instEnemy.GetComponent<AI>().UIMpNum = instEnemyUI.transform.Find("MpNum").GetComponent<TextMeshProUGUI>();
        instEnemy.GetComponent<AI>().m_name = enemyList[enemyIndex];
        instEnemy.GetComponent<AI>().Init();

        Player.Instance.enemyList.Add(instEnemy);
        Player.Instance.mTarget = instEnemy;
        enemyIndex = (enemyIndex + 1) % enemyList.Count;

    }



}
