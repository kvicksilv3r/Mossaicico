using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GridMaker gridMaker;
    public PixelRotator pixelRotator;

    public ColorData defaultData;
    public ColorData randomData;
    public SpriteToData dataGenerator;

    public static GameManager Instance;

    public GameObject WinScreen;

    public Texture2D downloadedImage;

    private ColorData currentData;

    public const string WIN_KEY = "Wins";
    public const string DL_URL = "https://picsum.photos/512";

    private void Awake()
    {
        Instance = this;
        SetupPrefs();
    }

    private void SetupPrefs()
    {
        if (!PlayerPrefs.HasKey(WIN_KEY))
        {
            PlayerPrefs.SetInt(WIN_KEY, 0);
        }
    }

    public void DefaultImage()
    {
        currentData = defaultData;
        gridMaker.InitializeImage(defaultData);
        pixelRotator.Shuffle();
    }

    public async void RandomImage()
    {
        //https://picsum.photos/64
        await GetRandomImage();
        dataGenerator.GenerateByCode(randomData, downloadedImage);
        gridMaker.InitializeImage(randomData);
        pixelRotator.Shuffle();
        currentData = randomData;
    }

    async
    Task
GetRandomImage()
    {
        downloadedImage = await DownloadImageAsync(DL_URL);
    }

    private async Task<Texture2D> DownloadImageAsync(string mediaUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl))
        {
            var operation = request.SendWebRequest();

            // Await the completion of the UnityWebRequest
            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }

    public void LevelCompleted()
    {
        IncreaseWins();
        WinScreen.SetActive(true);
        WinScreen.GetComponent<WinScreen>().Activate(currentData);
    }

    private void IncreaseWins()
    {
        var wins = PlayerPrefs.GetInt(WIN_KEY);
        wins++;
        PlayerPrefs.SetInt(WIN_KEY, wins);
        PlayerPrefs.Save();
    }
}
