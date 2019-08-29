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
public enum VisitType
{
    CanVisit,
    Visited,
    Locked
}
public class MapArea
{
    public MapAreaView view = null;
    public LevelData levelData;
    //public AreaType type;
    public Vector2 pos;
    public string AreaName;
    public VisitType m_VisitType;
    /*
    public bool canVisit = false;
    public bool visited = false;
    */
    public MapArea(int i,int j)
    {
        pos = new Vector2(i, j);
        m_VisitType = VisitType.Locked;
        //type = AreaType.Default;
    }
    public void SetLevelData(LevelData newLeveldata)
    {
        levelData = newLeveldata;
        AreaName = levelData.AreaName;
    }
    public void InitView(GameObject inst)
    {
        view = inst.GetComponent<MapAreaView>();
        view.area = this;
    }

    public void Activate()
    {
        Debug.Log("Activate");
        switch (levelData.LevelType)
        {
            case AreaType.Default:
                SuperController.Instance.ReadLevelDatas(levelData);
                if (levelData.PreStory != null)
                {
                    VisualNovelController.Instance.InitScript(levelData.PreStory);
                }
                else
                {
                    SuperController.Instance.SkillSelectUI();
                }
                if (levelData.AfterStory != null)
                {
                    SuperController.Instance.AfterStory = levelData.AfterStory;
                }
                else
                {
                    SuperController.Instance.AfterStory = null;
                }
                break;
            case AreaType.Fight:
                break;
            case AreaType.Shop:
                break;
            case AreaType.Story:
                break;
            case AreaType.Boss:
                break;
            default:
                break;
        }

        m_VisitType = VisitType.Visited;
        MapController.Instance.currentChapter.UnlockAround((int)pos.x, (int)pos.y);

        MapController.Instance.HideMap();
        
    }


}

public class MapChapter
{
    public int size = 1;
    public MapArea[,] map;

    public MapChapter (int s)
    {
        size = s;
    }

    public void GenerateMap()
    {
        map = new MapArea[size, size];
        //LevelData = new MapArea[size]
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                map[i, j] = new MapArea(i,j);
                if (MapController.Instance.m_LevelData.Length > i * 3 + j)
                    map[i, j].SetLevelData(MapController.Instance.m_LevelData[i * 3 + j]);
                else map[i, j].SetLevelData(Resources.Load<LevelData>("Data/Level/testLevel"));
            }
        }
    }

    public void UnlockAround(int x, int y)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i, j].m_VisitType == VisitType.CanVisit)
                    map[i, j].m_VisitType = VisitType.Locked;

            }
        }
        //if (i > 0) map[i - 1, j].canVisit = true;
        if (x < size - 1) map[x + 1, y].m_VisitType = VisitType.CanVisit;
        //if (j > 0) map[i, j - 1].canVisit = true;
        if (y < size - 1) map[x, y + 1].m_VisitType = VisitType.CanVisit;
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
    //将来需要把这个LevelData数组处理一下
    //做成每个map一个的Data
    public LevelData[] m_LevelData;

    public Transform mapCanvas;
    public MapChapter currentChapter;
    public void ShowMap()
    {
        mapCanvas.localScale = Vector3.one;
    }
    public void HideMap()
    {
        mapCanvas.localScale = Vector3.zero;
    }

    public void CreateChapterMap()
    {
        if (currentChapter != null)
        {
            for (int i = 0; i < currentChapter.size; i++)
            {
                for (int j = 0; j < currentChapter.size; j++)
                {
                    Destroy(currentChapter.map[i, j].view);
                }
            }
        }

        currentChapter = new MapChapter(3);
        currentChapter.GenerateMap();

        InitMap();
    }
    public void InitMap()
    {
        for(int i = 0; i < currentChapter.size; i++)
        {
            for (int j = 0; j < currentChapter.size; j++)
            {
                var inst = Instantiate(Resources.Load<GameObject>("Prefab/MapAreaView"), mapCanvas.Find("Grid"));
                inst.GetComponent<RectTransform>().localPosition = new Vector2((i - currentChapter.size / 2) * 144f, (j - currentChapter.size / 2) * 144f);
                currentChapter.map[i, j].InitView(inst);
            }
        }

        currentChapter.map[0, 0].m_VisitType = VisitType.CanVisit;
    }

}
