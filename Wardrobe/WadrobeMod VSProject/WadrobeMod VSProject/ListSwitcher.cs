using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ListSwitcher : MonoBehaviour
{
    public Transform[] listParents;

    int selected;

    void OnEnable()
    {
        SortList();
    }

    void Update()
    {
        if (selected != TextureSwapManager.instance.clothingDropdown.value)
        {
            selected = TextureSwapManager.instance.clothingDropdown.value;

            SortList();
        }
    }

    void SortList()
    {
        for (int i = 0; i < listParents.Length; i++)
        {
            if (i == selected)
                listParents[i].gameObject.SetActive(true);
            else
                listParents[i].gameObject.SetActive(false);
        }
    }
}
