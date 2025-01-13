using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI winsText;
    public string winsTextText;
    public ColorData activeData;
    public RawImage fullImage;

    private void OnEnable()
    {
        GetWins();
    }

    public void Activate(ColorData data)
    {
        fullImage.texture = data.fullImage;
    }

    private void GetWins()
    {
        var wins = PlayerPrefs.GetInt(GameManager.WIN_KEY);
        winsText.text = wins + winsTextText;
    }
}
