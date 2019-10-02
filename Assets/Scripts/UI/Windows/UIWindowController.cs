using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class UIWindowController : MonoBehaviour
{
    public List<UIWindow> windows;

    public UIMainMenu mainMenu;
    public UIMapWindow mapWindow;
    public UIWinWindow winWindow;
    public UIMapMenu mapMenu;
    //public 

    public Image arrow;




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


    public void Init()
    {
        windows = new List<UIWindow>();
        windows.Add(mainMenu);
        windows.Add(mapWindow);
        windows.Add(winWindow);
        windows.Add(mapMenu);

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ClearFocus()
    {
        foreach(UIWindow w in windows)
        {
            w.isFocus = false;
        }
    }

    IEnumerator OpenMapMenu()
    {
        mapMenu.gameObject.SetActive(true);
        mapWindow.AllButtonDisconnect();

        mapMenu.Init();
        float posx = mapMenu.buttons[0].transform.localPosition.x;
        float posy = mapMenu.buttons[0].transform.localPosition.y;
        float posz = mapMenu.buttons[0].transform.localPosition.z;

        foreach (Button b in mapMenu.buttons)
        {
            b.transform.localPosition = new Vector3(posx + 500f, b.transform.localPosition.y, b.transform.localPosition.z);
        }
        float time = 0.2f;
        float timecount = 0f;
        float a = 0f;
        //Vector3 startpos = arrow.transform.localPosition;
        while (timecount <= time)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }
            foreach (Button b in mapMenu.buttons)
            {
                b.transform.localPosition = Vector3.Lerp(
                    new Vector3(posx + 500f, b.transform.localPosition.y, posz),
                    new Vector3(posx, b.transform.localPosition.y, posz),
                    a
                );
            }


        }
    }


    IEnumerator ArrowMove()
    {
        EventSystem.current.sendNavigationEvents = false;
        float time = 0.2f;
        float timecount = 0f;
        float a = 0f;
        Vector3 startpos = arrow.transform.localPosition;
        while (timecount <= time)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }

            arrow.transform.localPosition = Vector3.Lerp(
                    startpos,
                    EventSystem.current.currentSelectedGameObject.transform.localPosition + new Vector3(-150f, -100f, 0),
                    a
            );
        }
        EventSystem.current.sendNavigationEvents = true;

    }
}
