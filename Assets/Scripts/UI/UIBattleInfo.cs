using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleInfo : MonoBehaviour
{
    public UIEnergyCells energyCells;


    #region 单例
    static UIBattleInfo _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static UIBattleInfo Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        energyCells.init();
    }

    public void Blink()
    {
        energyCells.Blink();
    }
}
