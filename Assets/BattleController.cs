using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour {

    static BattleController _instance;

    public Transform UICanvas;

    public GameObject enemyUIPrefab;
    public bool resultLock = false;

    public int enemyNum = 0;
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
        AddEnemy();
	}
	
	// Update is called once per frame
	void Update () {
    }

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

}
