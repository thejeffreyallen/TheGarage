using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    Transform listParent;

    float scrollSpeed = 50f;

    public void MoveList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                listParent = transform.GetChild(i);
        }

        listParent.transform.Translate(Vector3.up * Input.mouseScrollDelta.y * -scrollSpeed);
    }
}
