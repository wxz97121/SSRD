using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_playVFX : Buff
{
    private GameObject VFXGO;
    public string[] VFXStr;
    public override void BuffAdded(Character p_chara, string str)
    {
        m_name = "playVFX";

        base.BuffAdded(p_chara, str);

        VFXStr = str.Split('/');
        VFXGO = VFX.ShowVFX(VFXStr[0], new Vector3(float.Parse(VFXStr[1]), float.Parse(VFXStr[2]), float.Parse(VFXStr[3])), p_chara);


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
