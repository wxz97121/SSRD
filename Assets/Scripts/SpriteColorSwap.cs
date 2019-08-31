using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorSwapSturct
{
    public Color OldColor, NewColor;
    public float Dist;
}

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteColorSwap : MonoBehaviour
{
    public ColorSwapSturct[] ColorSwapArray;
    
    private void Awake()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.material = new Material(Shader.Find("Custom/SpriteSwapShader"));
        if (ColorSwapArray.Length>=1)
        {
            renderer.material.SetColor("_OldColor", ColorSwapArray[0].OldColor);
            renderer.material.SetColor("_NewColor", ColorSwapArray[0].NewColor);
            renderer.material.SetFloat("_DistToSwap", ColorSwapArray[0].Dist);
        }
    }

}
