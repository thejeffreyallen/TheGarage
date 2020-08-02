using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static Options instance;

    public GameObject menu;

    Transform player;
    bool lookAt = false;
    public Text lookAtText;

    bool followPlayer = false;
    Vector3 followOffset;
    public Text followText;

    public Text timeScaleText;

    void Start()
    {
        instance = this;

        menu.SetActive(false);
        player = GameObject.Find("Frame Mesh").transform;

        followText.text = "Set Following Player - " + followPlayer.ToString();
        lookAtText.text = "Set Looking at Player - " + lookAt.ToString();
        timeScaleText.text = "Change Time Scale - Frozen";
    }

    void Update()
    {
        if (ModManager.instance.IsEnabled())
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                menu.SetActive(true);
                SetMouse(false);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                menu.SetActive(false);
                SetMouse(true);
            }
        }
    }

    void LateUpdate()
    {
        if (ModManager.instance.IsEnabled())
        {
            if (lookAt)
                Camera.current.transform.LookAt(player);

            if (followPlayer)
                Camera.current.transform.position = player.position - followOffset;
        }
    }

    public void SwitchFollowPlayer()
    {
        followPlayer = !followPlayer;
        followText.text = "Set Following Player - " + followPlayer.ToString();

        if (followPlayer)
            followOffset = player.position - Camera.current.transform.position;
    }

    public void SetFollowPlayer(bool _follow)
    {
        followPlayer = _follow;
        followText.text = "Set Following Player - " + followPlayer.ToString();

        if (_follow)
            followOffset = player.position - Camera.current.transform.position;
    }

    public void SetFOV(Slider s)
    {
        Camera.current.fieldOfView = s.value;
    }

    public void ResetFOV(Slider s)
    {
        Camera.current.fieldOfView = 60;
        s.value = 60;
    }

    public void SwitchLookAtPlayer()
    {
        lookAt = !lookAt;
        lookAtText.text = "Set Looking At Player - " + lookAt.ToString();
    }

    public void SetLookAtPlayer(bool _lookAt)
    {
        lookAt = _lookAt;
        lookAtText.text = "Set Looking At Player - " + lookAt.ToString();
    }

    public void SetTimeScale(float _timeScale)
    {
        Time.timeScale = _timeScale;

        if (Time.timeScale == 0)
            timeScaleText.text = "Change Time Scale - Frozen";
        else
            timeScaleText.text = "Change Time Scale - Unfrozen";
    }

    public void SwitchTimeScale()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            timeScaleText.text = "Change Time Scale - Unfrozen";
        }
        else
        {
            Time.timeScale = 0;
            timeScaleText.text = "Change Time Scale - Frozen";
        }
    }

    public void SetMouse(bool locked)
    {
        if (locked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !locked;
    }

    public bool IsMenuActive()
    {
        return menu.activeSelf;
    }
}
