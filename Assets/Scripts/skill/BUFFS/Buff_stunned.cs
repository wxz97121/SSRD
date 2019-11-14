using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_stunned : Buff
{
    private GameObject VFXGO;
    //public string[] VFXStr;
    public override void BuffAdded(Character p_chara, string str)
    {
        m_name = "stunned";

        base.BuffAdded(p_chara, str);


        VFXGO = VFX.ShowVFX("Stunned", new Vector3(0f,1f,0f), p_chara);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void BuffRemove()
    {
        base.BuffRemove();
        VFXGO.GetComponent<VFX>().Kill();
    }
}
