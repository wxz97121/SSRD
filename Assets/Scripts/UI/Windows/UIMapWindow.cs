using System.Collections;


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIMapWindow : UIWindow
{

    public Image playerIcon;



    IEnumerator UnlockMaps(string[] mapNames)
    {
        yield return 0;
    }

    public void UpdateAreaVisitStateAll()
    {

        MapController.Instance.currentMapArea.m_VisitType = MapState.Current;
        Debug.Log(playerIcon.transform.localPosition);
        Debug.Log(MapController.Instance.currentMapArea.view.transform.localPosition);

        playerIcon.transform.localPosition = new Vector3(-120f, +80f, 0) + MapController.Instance.currentMapArea.view.transform.localPosition;

        foreach (MapArea m in MapController.Instance.mapAreas)
        {
            m.UpdateAreaVisitState();
        }
        foreach (MapArea m in MapController.Instance.currentMapArea.LinkedMaps)
        {
            if (m.m_VisitType == MapState.Unlocked)
            {
                m.m_VisitType = MapState.Near;
            }
            m.UpdateAreaVisitState();
        }



        //todo:这里要增加解锁和显示隐藏的动画
    }


    public override void SetSelect()
    {
        MapController.Instance.currentMapArea.view.GetComponent<Button>().Select();
        base.SetSelect();

    }


    public override void Focus()
    {
        UpdateAreaVisitStateAll();
        base.Focus();
    }

    public override void OnClick(Button button)
    {

        if(MapController.Instance.currentMapArea== button.GetComponent<MapAreaView>().area)
        {
            button.GetComponent<MapAreaView>().area.Activate();
        }
        else if(button.GetComponent<MapArea>().m_VisitType== MapState.Near)
        {
            StartCoroutine(PlayerIconMove(button));
        }

        base.OnClick(button);
    }

    IEnumerator PlayerIconMove(Button button)
    {
        AllButtonDisconnect();

        float time = 0.2f;
        float timecount = 0f;
        float a = 0f;
        //todo 每个按钮要增加箭头指示位置


        Vector3 startpos;


        startpos = playerIcon.transform.localPosition;



        Vector3 gopos = button.GetComponent<MapAreaView>().transform.localPosition + new Vector3(-120f, +80f, 0);




        while (timecount <= time)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timecount += Time.deltaTime;

            a = -(timecount * timecount) / (time * time) + 2 * timecount / time;
            if (a > 1) { a = 1; }


            playerIcon.transform.localPosition = Vector3.Lerp(
                    startpos,
                    gopos,
                    a
            );


        }
        yield return new WaitForSeconds(0.3f);

        button.GetComponent<MapArea>().Activate();
    }

    
}
