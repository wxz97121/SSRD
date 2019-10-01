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
        
        mapAreas = new List<MapArea>(mapCanvas.transform.GetComponentsInChildren<MapArea>());
        //mapAreas = mapCanvas.transform.GetComponentsInChildren<MapArea>();
        foreach (MapArea m in mapAreas)
        {
            if (m.m_VisitType == MapState.Current)
            {
                currentMapArea = m;
            }
        }
        UpdateAreaVisitStateAll();
        HideMap();

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

        currentMapArea.m_VisitType = MapState.Current;

        playerIcon.transform.localPosition = new Vector3(-180, 0, 0) + currentMapArea.view.transform.localPosition;

        foreach (MapArea m in mapAreas)
        {
            m.UpdateAreaVisitState();
        }
        foreach (MapArea m in currentMapArea.LinkedMaps)
        {
            if (m.m_VisitType == MapState.Unlocked)
            {
                m.m_VisitType = MapState.Near;
            }
            m.UpdateAreaVisitState();
        }

        //todo:这里要增加解锁和显示隐藏的动画

    }




    public void ShowMap()
    {
        mapCanvas.gameObject.SetActive(true);
        mapCanvas.GetComponent<UIMapWindow>().Init();
        UpdateAreaVisitStateAll();

        mapCanvas.localScale = Vector3.one;
    }
    public void HideMap()
    {
        mapCanvas.localScale = Vector3.zero;
        mapCanvas.gameObject.SetActive(false);

    }

    public void CreateChapterMap()
    {

    }
}
