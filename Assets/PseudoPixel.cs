using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoPixel : MonoBehaviour
{

    public bool button;
    public Color color;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform, -Vector3.up);
        transform.Rotate(Vector3.up, 180);

        if (button)
        {
            button = false;
            SetPixelColor(color);
        }
    }

    public void SetPixelColor(Color color)
    {
        var mr = GetComponent<MeshRenderer>();
        var mat = mr.material;
        var instMat = mat;
        instMat.SetColor("_Color", color);
        mr.material = instMat;
    }
}
