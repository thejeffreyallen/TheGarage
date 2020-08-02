using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

public class ImageTaker : MonoBehaviour
{
    public GameObject ui;

    void Start()
    {
        ui.SetActive(false);
    }

    void Update()
    {
        if (ModManager.instance.IsEnabled() && Input.GetKeyDown(KeyCode.Space))
        {
            TakeImage();
        }
    }

    public void TakeImage()
    {
        Bitmap bt = new Bitmap(Screen.width, Screen.height, PixelFormat.Format32bppArgb);
        System.Drawing.Graphics screenShot = System.Drawing.Graphics.FromImage(bt);
        Size s = new Size(bt.Width, bt.Height);
        screenShot.CopyFromScreen(0, 0, 0, 0, s);

        if (Directory.Exists(Path.Combine(Application.dataPath, "TheHouseContent/Camera Mode Images")))
            bt.Save(Application.dataPath + "\\TheHouseContent/Camera Mode Images/Volution CameraMode Image " + DateTime.Now.TimeOfDay.TotalMilliseconds.ToString() + ".png");
        else
        {
            Directory.CreateDirectory(Application.dataPath + "\\TheHouseContent/Camera Mode Images");
            bt.Save(Application.dataPath + "\\TheHouseContent/Camera Mode Images/Volution CameraMode Image " + DateTime.Now.TimeOfDay.TotalMilliseconds.ToString() + ".png");
        }

        StartCoroutine(ShowUI());
    }

    IEnumerator ShowUI()
    {
        ui.SetActive(true);
        yield return new WaitForSecondsRealtime(.1f);
        ui.SetActive(false);
    }
}
