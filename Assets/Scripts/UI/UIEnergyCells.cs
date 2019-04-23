using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEnergyCells : MonoBehaviour
{
    public List<GameObject> CellList;
    public int maxMP;
    public int MP;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        //CellList = new List<GameObject>();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        //能量减少
        if (Player.Instance.Mp < MP)
        {
            UseEnergy((MP-Player.Instance.Mp ) / 4,MP/4);
        }
        MP = Player.Instance.Mp;
        DisplayEnergyCell(MP);
    }

    public void init()
    {
        maxMP = Player.Instance.maxMp/4;
        MP = Player.Instance.Mp;

        for (int i = 0; i < CellList.Count; i++)
        {
            Hide(i);
        }
        for (int i = 0; i < maxMP; i++)
        {
            Show(i);
        }
    }

    public void Hide(int num)
    {
        CellList[num].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        CellList[num].transform.GetChild(1).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        CellList[num].transform.GetChild(2).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
    }

    public void Show(int num)
    {
        CellList[num].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        CellList[num].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
        CellList[num].transform.GetChild(1).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
        CellList[num].transform.GetChild(2).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        
    }

    public void DisplayEnergyCell(int mp)
    {

        int fullcells = mp / 4;
        int remainder = mp - fullcells * 4;
        //Debug.Log("fullcells = "+ fullcells+"remainder = "+ remainder);
        //Debug.Log("fill = " + (float)remainder / 4f);

        for (int i = 0;i<fullcells;i++)
        {
            CellList[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        }
        for (int i = fullcells; i < maxMP ; i++)
        {
            CellList[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            CellList[i].transform.GetChild(2).GetComponent<Image>().enabled = false;
        }
        CellList[fullcells].transform.GetChild(0).GetComponent<Image>().fillAmount = (float)remainder / 4f;

    }

    //已经满了的能量格子闪烁
    public void Blink()
    {
//        Debug.Log("blink");
        int fullcells = MP / 4;

        for (int i = 0; i < fullcells; i++)
        {
            CellList[i].transform.GetChild(2).GetComponent<Image>().enabled = true;
            CellList[i].transform.GetChild(2).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
            CellList[i].transform.GetChild(2).GetComponent<Image>().DOFade(0,0.4f).SetEase(Ease.InQuad);

        }
    }

    public void UseEnergy(int amount,int before)
    {
        int startcell = before - amount;
        Debug.Log("startcell" + startcell);
        for (int i= startcell;i< before; i++)
        {
            Instantiate((GameObject)Resources.Load("VFX/EnergyUsed", typeof(GameObject)), CellList[i].transform);
            CellList[i].transform.Find("EnergyUsed(Clone)").GetComponent<VFX>().StartCoroutine("FadeOutLarger");

        }

    }
}
