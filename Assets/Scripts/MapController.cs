using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType
{
    Default,
    Fight,
    Shop,
    Story,
    Boss
}

public class MapArea
{
    public MapAreaView view = null;
    public LevelData levelData;
    public AreaType type;
    public Vector2 pos;

    public bool canVisit = false;
    public bool visited = false;

    public MapArea(int i,int j)
    {
        pos = new Vector2(i, j);
    }

    public void InitView(GameObject inst)
    {
        view = inst.GetComponent<MapAreaView>();
        view.area = this;
    }

    public void Activate()
    {
        Debug.Log("Activate");
        MapController.Instance.currentChapter.UnlockAround((int)pos.x, (int)pos.y);
        MapController.Instance.HideMap();
        SuperController.Instance.SkillSelectUI();
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

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                map[i, j] = new MapArea(i,j);
            }
        }
    }

    public void UnlockAround(int i, int j)
    {
        //if (i > 0) map[i - 1, j].canVisit = true;
        if (i < size - 1) map[i + 1, j].canVisit = true;
        //if (j > 0) map[i, j - 1].canVisit = true;
        if (j < size - 1) map[i, j + 1].canVisit = true;
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
    public Transform mapCanvas;
    public MapChapter currentChapter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        currentChapter.map[0, 0].canVisit = true;
    }
}
