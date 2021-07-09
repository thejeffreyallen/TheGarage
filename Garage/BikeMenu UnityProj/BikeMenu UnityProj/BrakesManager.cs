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

    public GameObject barBrakes;
    public GameObject frameBrakes;

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
        if (enabled && !brakesEnabled)
            EnableBrakes();
        else if(!enabled && brakesEnabled)
            DisableBrakes();
    }

    public bool IsEnabled()
    {
        return brakesEnabled;
    }

    void EnableBrakes()
    {
        Transform bars = PartMaster.instance.GetPart(PartMaster.instance.bars).transform;
        Transform frame = PartMaster.instance.GetPart(PartMaster.instance.frame).transform;

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
        Debug.Log("Destroying brake prefabs");
        Transform TempParent = new GameObject().transform;
        barBrakes.transform.parent = TempParent;
        frameBrakes.transform.parent = TempParent;
        Destroy(barBrakes);
        Destroy(frameBrakes);
        Destroy(TempParent.gameObject);
        Debug.Log("Brake prefabs destroyed");
        brakesEnabled = false;
    }

    public GameObject GetBarBrakes()
    {
        return barBrakes;
    }

    public GameObject GetFrameBrakes()
    {
        return frameBrakes;
    }
}
