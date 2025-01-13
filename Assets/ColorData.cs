using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PseudoPixelData
{
    public Color color;
    public bool shouldRender;
}

[CreateAssetMenu(fileName = "Data", menuName = "PseudoPixel/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    public List<PseudoPixelData> data;
    public int width;
    public int height;
    public Texture2D fullImage;
}
