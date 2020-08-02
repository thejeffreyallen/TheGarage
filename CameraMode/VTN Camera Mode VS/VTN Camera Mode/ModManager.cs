using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ModManager : MonoBehaviour
{
    public static ModManager instance;

    bool isEnabled = false;

    public GameObject helpHUD;

    void Start()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        if (helpHUD.activeSelf)
            helpHUD.SetActive(false);
    }

    void Update()
    {
        if (MGInputManager.RightStick_Down())
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                CamLoader.LoadCam();
                StartCoroutine(ShowHelp());
            }
            else
                CamLoader.UnloadCam();
                
        }
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }

    IEnumerator ShowHelp()
    {
        helpHUD.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        helpHUD.SetActive(false);
    }
}
