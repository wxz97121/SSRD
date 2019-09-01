using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct ColorSwapSturct
{
    public Color OldColor, NewColor;
    public float Dist;
}

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class SpriteColorSwap : MonoBehaviour
{
    public ColorSwapSturct[] ColorSwapArray;
    SpriteRenderer m_renderer;
    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += (m_Change) => { Awake(); };
#endif
    }
    private void Awake()
    {

        //Debug.Log("SHABI");
        m_renderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }
    void UpdateColor()
    {
        if (!m_renderer) return;
        m_renderer.material = new Material(Shader.Find("Custom/SpriteSwapShader"));
        if (ColorSwapArray.Length >= 1)
        {
            m_renderer.sharedMaterial.SetColor("_OldColor", ColorSwapArray[0].OldColor);
            m_renderer.sharedMaterial.SetColor("_NewColor", ColorSwapArray[0].NewColor);
            m_renderer.sharedMaterial.SetFloat("_DistToSwap", ColorSwapArray[0].Dist);
        }
        if (ColorSwapArray.Length >= 2)
        {
            m_renderer.sharedMaterial.SetColor("_OldColor2", ColorSwapArray[1].OldColor);
            m_renderer.sharedMaterial.SetColor("_NewColor2", ColorSwapArray[1].NewColor);
            m_renderer.sharedMaterial.SetFloat("_DistToSwap2", ColorSwapArray[1].Dist);
        }
        if (ColorSwapArray.Length >= 3)
        {
            m_renderer.sharedMaterial.SetColor("_OldColor3", ColorSwapArray[2].OldColor);
            m_renderer.sharedMaterial.SetColor("_NewColor3", ColorSwapArray[2].NewColor);
            m_renderer.sharedMaterial.SetFloat("_DistToSwap3", ColorSwapArray[2].Dist);
        }
    }
    private void OnValidate()
    {
        UpdateColor();
    }

}

