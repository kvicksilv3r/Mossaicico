using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class ImageResize : MonoBehaviour
{
    public static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = System.Drawing.Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }

    private static void Resize(Texture2D texture, int newWidth, int newHeight)
    {
        RenderTexture tmp = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTexture.active = tmp;
        UnityEngine.Graphics.Blit(texture, tmp);
        texture.Reinitialize(newWidth, newHeight, texture.format, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.ReadPixels(new Rect(Vector2.zero, new Vector2(newWidth, newHeight)), 0, 0);
        texture.Apply();
        RenderTexture.ReleaseTemporary(tmp);
    }
}
