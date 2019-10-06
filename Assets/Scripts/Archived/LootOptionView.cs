//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//using TMPro;

//public class LootOptionView : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
//{

//    public LootOption option;
//    public TextMeshProUGUI UIName;
//    public TextMeshProUGUI UIDesc;

//    private Text decText;

//    // Start is called before the first frame update
//    void Start()
//    {
//        decText = GameObject.Find("LootDesc").GetComponents<Text>()[0] as Text;

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        UIName.text = option.GetName();
//        UIDesc.text = option.GetDesc();
//    }

//    public void SetOption(LootOption opt)
//    {
//        option = opt;
        
//    }

//    public void OnClick()
//    {
//        option.Activate();
//        //LootController.Instance.EndLoot();
//    }

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        if (option != null)
//            decText.text = option.GetDesc();
//    }
//    public void OnPointerExit(PointerEventData eventData)
//    {
//        decText.text = "";
//    }
//}
