using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//战斗总控制
public class BattleController : MonoBehaviour {

    static BattleController _instance;
    //ui控制
    public Transform UICanvas;

    //敌人的ui模板
    public GameObject enemyUIPrefab;


    //简易的敌人生成系统
    //敌人总数
    public int enemyNum = 0;
    //当前敌人序号
    public int enemyIndex = 0;
    private void Awake()
    {
        _instance = this;
    }

    public static BattleController Instance {
        get{
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {
        UICanvas = GameObject.Find("Canvas").transform;
        //AddEnemy();
	}
	
	// Update is called once per frame
	void Update () {
    }

    //增加一个新敌人，配置位置和ui等
    public void AddEnemy (){
        GameObject instEnemy = Instantiate((GameObject)Resources.Load("Prefab/Enemy/"+enemyIndex.ToString(),typeof(GameObject)), new Vector3(5, 0, 0), Quaternion.identity);
        enemyIndex = (enemyIndex + 1) % enemyNum;
        GameObject instEnemyUI = Instantiate(enemyUIPrefab, UICanvas);
        instEnemyUI.transform.localPosition += new Vector3(Screen.width / 4, Screen.height / 4, 0);
        instEnemy.GetComponent<AI>().UIHpNum = instEnemyUI.transform.Find("HpNum").GetComponent<TextMeshProUGUI>();
        instEnemy.GetComponent<AI>().UIMpNum = instEnemyUI.transform.Find("MpNum").GetComponent<TextMeshProUGUI>();
        Player.Instance.enemyList.Add(instEnemy);
        Player.Instance.mTarget = instEnemy;
    }

    //开始一个新战斗
    public void NewBattle()
    {
        AddEnemy();
    }

}
