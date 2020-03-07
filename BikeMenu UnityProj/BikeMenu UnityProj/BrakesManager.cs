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
}
