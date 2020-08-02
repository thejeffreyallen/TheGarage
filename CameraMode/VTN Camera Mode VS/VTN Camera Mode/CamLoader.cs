using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CamLoader : MonoBehaviour
{
    static GameObject cam;
    static Transform mainCam;

    public static void LoadCam()
    {
        mainCam = Camera.current.transform;
        cam = Instantiate(mainCam.gameObject, mainCam.position, mainCam.rotation);
        cam.gameObject.AddComponent<CamController>();
        mainCam.GetComponent<Camera>().enabled = false;

        Options.instance.SetTimeScale(0);
        Options.instance.SetMouse(true);
    }

    public static void UnloadCam()
    {
        Options.instance.SetTimeScale(1);
        Options.instance.SetMouse(false);
        Options.instance.SetLookAtPlayer(false);
        Options.instance.SetFollowPlayer(false);

        Destroy(cam.gameObject);
        cam = null;

        mainCam.GetComponent<Camera>().enabled = true;
    }
}
