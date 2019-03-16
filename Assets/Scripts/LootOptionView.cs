using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootOptionView : MonoBehaviour
{

    public LootOption option;
    public TextMeshProUGUI UIName;
    public TextMeshProUGUI UIDesc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UIName.text = option.GetName();
        UIDesc.text = option.GetDesc();
    }

    public void SetOption(LootOption opt)
    {
        option = opt;
        
    }

    public void OnClick()
    {
        option.Activate();
        LootController.Instance.EndLoot();
    }
}
