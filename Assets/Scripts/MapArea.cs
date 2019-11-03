using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapArea : MonoBehaviour
{
    public MapAreaView view;
    public LevelData levelData;
    public Shop shop;
    public string AreaName;
    public MapState m_VisitType;
    public List<MapArea> LinkedMaps;
    public bool isVisited=false;

    public void Init()
    {
        view = this.GetComponent<MapAreaView>();
//        Debug.Log("get view");
        view.area = this;
        view.Init();
        InitShop();

    }

    public void InitShop()
    {
        if (levelData.shopData != null)
        {
            shop = new Shop()
            {
                data = levelData.shopData
            };
            shop.Init();
            Debug.Log("init shop , goods: " + shop.goods.Count);
        }

        //shop.Init();
    }



    public void InitMap(MapState visitType)
    {
        m_VisitType = visitType;
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
        if (isVisited)
        {
            Debug.Log("show menu!!");
            UIWindowController.Instance.mapMenu.OpenMapMenu(this);
        }
        else
        {

            //UIWindowController.Instance.arrow.transform.position =new Vector3(10000,0, 90.0f);

            SuperController.Instance.ReadLevelDatas(levelData);
            if (levelData.PreStory != null)
            {
                VisualNovelController.Instance.InitScript(levelData.PreStory);
            }
            else
            {
                SuperController.Instance.BattleStart();
            }
            if (levelData.AfterStory != null)
            {
                SuperController.Instance.AfterStory = levelData.AfterStory;
            }
            else
            {
                SuperController.Instance.AfterStory = null;
            }
            MapController.Instance.currentMapArea.m_VisitType = MapState.Unlocked;
            MapController.Instance.currentMapArea = this;

            //isVisited =true;


            MapController.Instance.HideMap();
        }
    }

    //更新地块状态
    public void UpdateAreaVisitState()
    {
        if (view == null)
        {
            InitView(this.gameObject);
        }


        if (isVisited && m_VisitType == MapState.Current)
        {
            view.GetComponent<Image>().color = Color.white;
            view.GetComponent<Button>().interactable = true;
        }

        if (isVisited && m_VisitType == MapState.Near)
        {
            view.GetComponent<Image>().color = Color.white;
            view.GetComponent<Button>().interactable = true;
        }

        if (isVisited == false && m_VisitType == MapState.Current)
        {
            view.GetComponent<Image>().color = Color.white;
            view.GetComponent<Button>().interactable = true;
        }

        if (isVisited == false && m_VisitType == MapState.Unlocked)
        {
            view.GetComponent<Image>().color = Color.gray;
            view.GetComponent<Button>().interactable = false;
        }

        if (isVisited==false && m_VisitType == MapState.Locked)
        {
            view.GetComponent<Image>().color = Color.black;
            view.GetComponent<Button>().interactable = false;
        }

        if (isVisited == false && m_VisitType == MapState.Near)
        {
            view.GetComponent<Image>().color = Color.white;
            view.GetComponent<Button>().interactable = true;
        }
    }
}