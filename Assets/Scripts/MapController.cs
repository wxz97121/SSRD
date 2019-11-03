using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AreaType
{
    Default,
    Fight,
    Shop,
    Story,
    Boss
}
public enum MapState
{
    Unlocked,
    Locked,
    Hide,
    Near,
    Current
}


public class MapChapter
{
    public int size = 1;
    public MapArea[,] map;

    public MapChapter (int s)
    {
        size = s;
    }




}

public class MapController : MonoBehaviour
{
    protected static MapController _instance;
    public void Awake()
    {
        _instance = this;
    }
    public static MapController Instance
    {
        get
        {
            return _instance;
        }
    }

    void Start()
    {
        UIWindowController.Instance.mapWindow.transform.localScale = Vector3.zero;

        UIWindowController.Instance.mapWindow.gameObject.SetActive(true);

        mapAreas = new List<MapArea>(mapCanvas.transform.GetComponentsInChildren<MapArea>());
        //mapAreas = mapCanvas.transform.GetComponentsInChildren<MapArea>();
        foreach (MapArea m in mapAreas)
        {
            m.Init();
            if (m.m_VisitType == MapState.Current)
            {
                currentMapArea = m;
                Debug.Log("cur = " + m.AreaName);
            }
        }
        UpdateAreaVisitStateAll();
        UIWindowController.Instance.mapWindow.Close();



    }

    public List<MapArea> mapAreas;

    public Transform mapCanvas;

    public Image playerIcon;
    public MapChapter currentChapter;

    public MapArea currentMapArea;

    IEnumerator UnlockMaps(string[] mapNames)
    {
        yield return 0;
    }

    public void UpdateAreaVisitStateAll()
    {
        UIWindowController.Instance.mapWindow.UpdateAreaVisitStateAll();


    }




    public void ShowMap()
    {

        StartCoroutine("ShowMapCR");

    }
    public void HideMap()
    {
        StartCoroutine("HideMapCR");

    }

    public IEnumerator ShowMapCR()
    {
        yield return StartCoroutine(UIWindowController.Instance.BlackIn());

        //特殊处理，为了延迟关闭各种弹窗
        UIWindowController.Instance.CloseAllExcept(UIWindowController.Instance.mapWindow);

        UIWindowController.Instance.mapWindow.Open();


        UpdateAreaVisitStateAll();

        yield return StartCoroutine(UIWindowController.Instance.BlackOut());

    }

    public IEnumerator HideMapCR()
    {
        yield return StartCoroutine(UIWindowController.Instance.BlackIn());

        UIWindowController.Instance.mapWindow.Close();
        yield return StartCoroutine(UIWindowController.Instance.BlackOut());



    }
}
