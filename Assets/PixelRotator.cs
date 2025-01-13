using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelRotator : MonoBehaviour
{
    public Slider xSldier;
    public Slider ySldier;
    public Slider zSldier;

    public Transform pixelHolster;

    private Vector3 offset;
    private Vector3 input;

    private bool isActive = false;

    public void Shuffle()
    {
        GenerateOffset();
        RotatePixels();
        isActive = true;
    }

    private void GenerateOffset()
    {
        offset = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        RotatePixels();

        if (pixelHolster.rotation.eulerAngles.magnitude < 5)
        {
            GenerateOffset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        ReadSliders();

        RotatePixels();

        ValidateRotation();

    }

    private void ValidateRotation()
    {
        if (pixelHolster.rotation.eulerAngles.magnitude < 5)
        {
            isActive = false;
            GameManager.Instance.LevelCompleted();
        }
    }

    private void RotatePixels()
    {
        pixelHolster.rotation = Quaternion.Euler(offset + input);
    }

    private void ReadSliders()
    {
        input.x = xSldier.value;
        input.y = ySldier.value;
        input.z = zSldier.value;
    }
}
