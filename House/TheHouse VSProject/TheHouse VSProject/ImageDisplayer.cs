using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ImageDisplayer : MonoBehaviour
{
    private List<Texture> images = new List<Texture>();
    int currentSelected;

    public GameObject screen;

    string[] fileNames;

    void OnEnable()
    {
        GetImages();
    }

    public void NextImage()
    {
        currentSelected++;
        if (currentSelected > images.Count - 1)
            currentSelected = 0;

        screen.GetComponent<Renderer>().material.mainTexture = images[currentSelected];
    }

    void GetImages()
    {
        fileNames = Directory.GetFiles(Application.dataPath + "\\TheHouseContent/Camera Mode Images");

        StartCoroutine(LoadImages());
    }

    IEnumerator LoadImages()
    {
        for (int i = 0; i < fileNames.Length; i++)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(fileNames[i]);
            yield return webRequest.SendWebRequest();

            images.Add(((DownloadHandlerTexture)webRequest.downloadHandler).texture as Texture);
        }

        screen.GetComponent<Renderer>().material.mainTexture = images[currentSelected];
    }
}
