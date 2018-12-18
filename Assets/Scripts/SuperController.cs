using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//统一控制局内战斗
public class SuperController : MonoBehaviour {

    #region 单例
    static SuperController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static SuperController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateInput();
	}

    protected void UpdateInput()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            DuelController.Instance.ShowAction(actionType.Collect);
        }


    }
}
