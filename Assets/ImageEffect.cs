using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffect : MonoBehaviour
{
    public Material ImageMat;
    float dx, dy;
    [HideInInspector]
    public bool isShake = false;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isShake)
        {
            dx += (Random.value - 0.5f) * 0.25f * Time.deltaTime;
            dy += (Random.value - 0.5f) * 0.25f * Time.deltaTime;
            dx = Mathf.Clamp(dx, -0.2f, 0.2f);
            dy = Mathf.Clamp(dy, -0.2f, 0.2f);

            //dx = 1;
        }
        else dx = dy = 0;
        print("dx " + dx.ToString());
        print("dy " + dy.ToString());

        ImageMat.SetFloat("_DeltaX", dx);
        ImageMat.SetFloat("_DeltaY", dy);
        Graphics.Blit(source, destination, ImageMat);
    }
    private void Update()
    { }

}
