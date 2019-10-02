using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowController : MonoBehaviour
{
    public UIMainMenu mainMenu;
    public UIMapWindow mapWindow;
    public UIWinWindow winWindow;
    //public 





    #region 单例
    static UIWindowController _instance;
    public static UIWindowController Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
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
}
