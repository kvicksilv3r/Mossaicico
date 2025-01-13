using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public ColorData data;
    public GameObject pseudoPixel;
    public Transform pixelHolster;

    public int width;
    public int height;

    public float depthExtent = 6;

    float extent = 1.5f;

    float scale = 1;

    public void InitializeImage(ColorData data)
    {
        this.data = data;
        ClearPixels();
        Generate();
        MovePixels();
    }

    private void ClearPixels()
    {
        foreach (Transform child in pixelHolster.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void MovePixels()
    {
        var cam = Camera.main.transform;

        for (int i = 0; i < pixelHolster.childCount; i++)
        {
            var child = pixelHolster.GetChild(i);

            var vectorPath = child.position - cam.position;

            if (Random.Range(0, 2) == 1)
            {
                var random = Random.Range(1, depthExtent);
                child.position = cam.position + vectorPath.normalized * random;
                child.localScale /= Mathf.Clamp(5.0f - random, 1f, 5f);
            }

            else
            {
                //continue;
                var random = Random.Range(0, depthExtent);
                child.position = cam.position + vectorPath.normalized * (5 + random);
                child.localScale *= random + 1;
            }
        }
    }

    private void Generate()
    {
        //Set Scale
        scale = (extent * 2) / width;

        var posX = extent * -1f;
        var posY = extent * -1f;

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                var dataIndex = CalcColor(w, h);

                if (!data.data[dataIndex].shouldRender)
                {
                    continue;
                }

                var g = Instantiate(pseudoPixel, pixelHolster);
                g.transform.position = new Vector3(posX + (scale * h), posY + (scale * w), 0);
                g.GetComponent<PseudoPixel>().SetPixelColor(data.data[dataIndex].color);
                g.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

    private int CalcColor(int x, int y)
    {
        var horizontal = data.width / width;
        var vertical = data.height / height;

        return (data.width * y * vertical) + (x * horizontal);
    }
}
