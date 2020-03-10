using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BrakesManager : MonoBehaviour
{
    public static BrakesManager instance;
    public GameObject barsbrakePrefab;
    public GameObject framebrakePrefab;

    GameObject barBrakes;
    GameObject frameBrakes;

    public bool brakesEnabled;

    private void Awake()
    {
        instance = this;
        brakesEnabled = false;
    }

    public void SetBrakes()
    {
        if (!brakesEnabled)
            EnableBrakes();
        else
            DisableBrakes();
    }

    public void SetBrakes(bool enabled)
    {
        if (enabled)
            EnableBrakes();
        else
            DisableBrakes();
    }

    void EnableBrakes()
    {
        Transform bars = GameObject.Find("Bars Mesh").transform;
        Transform frame = GameObject.Find("Frame Mesh").transform;

        barBrakes = Instantiate(barsbrakePrefab, bars.transform.position, bars.transform.rotation);
        barBrakes.transform.localScale = (new Vector3(0.8833787f,0.893016f, 1));
        barBrakes.transform.localPosition = (new Vector3(barBrakes.transform.localPosition.x+0.0025f, barBrakes.transform.localPosition.y +0.026f, barBrakes.transform.localPosition.z-0.005f));
        barBrakes.transform.parent = bars;

        frameBrakes = Instantiate(framebrakePrefab, frame.transform.position, frame.transform.rotation);
        frameBrakes.transform.parent = frame;
        brakesEnabled = true;
    }

    void DisableBrakes()
    {
        Destroy(barBrakes);
        Destroy(frameBrakes);

        barBrakes = null;
        frameBrakes = null;
        brakesEnabled = false;
    }

    public GameObject GetBarBrakes()
    {
        return this.barBrakes;
    }

    public GameObject GetFrameBrakes()
    {
        return this.frameBrakes;
    }
}
