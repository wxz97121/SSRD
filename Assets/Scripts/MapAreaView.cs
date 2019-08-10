using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapAreaView : MonoBehaviour
{
    public MapArea area = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (area != null)
        {
            transform.Find("Label").GetComponent<TextMeshProUGUI>().text = area.AreaName;

            if (area.visited)
            {
                GetComponent<Image>().color = Color.gray;
                GetComponent<Button>().interactable = false;
            }
            else if (area.canVisit)
            {
                GetComponent<Image>().color = Color.green;
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Image>().color = Color.red;
                GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnClick()
    {
        area.Activate();
    }
}
