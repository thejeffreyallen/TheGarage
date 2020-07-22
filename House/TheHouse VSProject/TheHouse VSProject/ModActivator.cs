using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModActivator : MonoBehaviour
{
    public enum options
    {
        exit,
        garage,
        wardobe,
        imageView
    }

    public options chosenMod;

    bool mouseOver;
    public Transform door;
    public float openSpeed;
    public int openRotY;

    public void OpenGarage()
    {
        FindObjectOfType<MainManager>().SetOpen();
        FindObjectOfType<HouseManager>().SetHouseOpen(false, true, false);
    }

    public void OpenWardrobe()
    {
        FindObjectOfType<WadrobeMainScript>().SetOpen();
        FindObjectOfType<HouseManager>().SetHouseOpen(false, true, false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SelectOption();
        }

        mouseOver = true;
    }

    void Update()
    {
        if (mouseOver)
        {
            door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(-90, openRotY, 0), openSpeed * Time.deltaTime);
        }
        else
        {
            door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(-90, 0, 0), openSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitHouse();

    }

    void OnMouseExit()
    {
        mouseOver = false;
    }

    void SelectOption()
    {
        switch (chosenMod)
        {
            case options.exit:
                ExitHouse();
                break;
            case options.garage:
                OpenGarage();
                break;
            case options.wardobe:
                OpenWardrobe();
                break;
            case options.imageView:
                FindObjectOfType<ImageDisplayer>().NextImage();
                break;
        }
    }

    void ExitHouse()
    {
        FindObjectOfType<HouseManager>().SetHouseOpen(false, true, false);
        SessionMarker.Instance.ResetPlayerAtMarker();
        HouseRoomLoader.instance.EnableLights();
    }

    IEnumerator MoveCam()
    {
        HouseCam cam = FindObjectOfType<HouseCam>();
        cam.MoveCamForward();

        while (!cam.HasCamMovedForward())
            yield return new WaitForEndOfFrame();

        SelectOption();
    }
}
