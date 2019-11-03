using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIWindowController : MonoBehaviour
{
    public List<UIWindow> windows;

    public GameObject nowselect;
    public GameObject lastselect;

    public UIWindow focusWindow;


    public UIMainMenu mainMenu;
    public UIMapWindow mapWindow;
    public UIWinWindow winWindow;
    public UIMapMenu mapMenu;
    public UIPrepareWindow prepareWindow;
    public UINoticeWindow noticeWindow;
    public UIItemSelect itemSelect;
    public UIShopWindow shopWindow;
    public UISkillUpgradeSelectSkill SkillUpgradeSelectSkill;
    public UIUpgradeSkill upgradeSkill;
    public UIConfirmWindow confirmWindow;
    public Image arrow;

    public Image blackCurtain;


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
        windows = new List<UIWindow>
        {
            mainMenu,
            mapWindow,
            winWindow,
            mapMenu,
            prepareWindow,
            noticeWindow,
            itemSelect,
            shopWindow,
            SkillUpgradeSelectSkill,
            upgradeSkill,
            confirmWindow
        };


    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(nowselect);
        }
        else
        {
            if (nowselect != EventSystem.current.currentSelectedGameObject)
            {
                lastselect = nowselect;
                nowselect = EventSystem.current.currentSelectedGameObject;

                StopCoroutine("ArrowMove");

                StartCoroutine("ArrowMove");
            }


            nowselect = EventSystem.current.currentSelectedGameObject;
        }
    }


    public void ClearFocus()
    {
        foreach(UIWindow w in windows)
        {
            w.isFocus = false;
        }
    }

    public void CloseAllExcept(UIWindow win)
    {
        foreach (UIWindow w in windows)
        {
            if (w != win)
            {
                w.Close();
            }
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
       // EventSystem.current.sendNavigationEvents = false;
        float time = 0.2f;
        float timecount = 0f;
        float a = 0f;
        //todo 每个按钮要增加箭头指示位置


        Vector3 startpos;


        startpos = arrow.transform.position;
        GameObject GO = EventSystem.current.currentSelectedGameObject;


        Vector3 gopos = GO.transform.position;




        while (timecount <= time)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }


            arrow.transform.position= Vector3.Lerp(
                    startpos,
                    gopos,
                    a
            );

        }
      //  EventSystem.current.sendNavigationEvents = true;

    }

    public IEnumerator BlackIn()
    {
        Debug.Log("start black flash 2");

        float time = 0.2f;
        float timecount = 0f;
        float a;
        Debug.Log("start black flash 3");

        while (timecount <= time/2)
        {
                    Debug.Log("start black flash 3");

            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }


            blackCurtain.color = new Color(0f, 0f, 0f, a);
            Debug.Log(blackCurtain.color);
        }
    }

    public IEnumerator BlackOut()
    {
        Debug.Log("start black flash 2");

        float time = 0.2f;
        float timecount = 0f;
        float a;
        Debug.Log("start black flash 3");

        while (timecount <= time / 2)
        {
            Debug.Log("start black flash 3");

            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }


            blackCurtain.color = new Color(0f, 0f, 0f, 1-a);
            Debug.Log(blackCurtain.color);
        }

    }


}
