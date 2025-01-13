using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpriteToData : MonoBehaviour
{
    public ColorData data;
    public Texture2D srcImage;
    public Texture2D downscaledImage;

    public bool button;

    // Update is called once per frame
    void Update()
    {

    }
    private void OnValidate()
    {
        if (button)
        {
            button = false;
            GenerateData();
        }
    }

    private void GenerateData()
    {
        if (!data)
        {
            return;
        }

        if (!srcImage)
        {
            return;
        }

        Downscale();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        data.data = new List<PseudoPixelData>();

        for (int height = 0; height < downscaledImage.height; height++)
        {
            for (int width = 0; width < downscaledImage.width; width++)
            {
                var ppd = new PseudoPixelData();
                ppd.color = downscaledImage.GetPixel(height, width);
                ppd.shouldRender = ppd.color.a > 0.5f;

                data.data.Add(ppd);
            }
        }

        data.width = downscaledImage.width;
        data.height = downscaledImage.height;

        data.fullImage = srcImage;

        data.SetDirty();

        stopwatch.Stop();

        print("Donezo! Took " + stopwatch.ElapsedMilliseconds + " ms.");
    }

    private void Downscale()
    {
        downscaledImage = new Texture2D(srcImage.width, srcImage.height, srcImage.format, false);

        downscaledImage.SetPixels(srcImage.GetPixels());
        downscaledImage.Apply();

        downscaledImage = Resize(downscaledImage, 64, 64);
        downscaledImage.Apply();

    }

    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    public void GenerateByCode(ColorData newData, Texture2D image)
    {
        data = newData;
        srcImage = image;
        GenerateData();
    }
}
