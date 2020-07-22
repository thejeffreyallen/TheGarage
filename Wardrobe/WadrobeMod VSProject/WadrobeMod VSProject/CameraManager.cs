using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform[] positions;
    Vector3 targetPos;
    Quaternion targetRot;

    void OnEnable()
    {
        instance = this;
        SetTarget(0);
    }

    void Update()
    {
        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 100 * Time.deltaTime);
        }
    }

    public void SetTarget(int arrayNum)
    {
        targetPos = positions[arrayNum].position;
        targetRot = positions[arrayNum].rotation;
    }
}
