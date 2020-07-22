using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkeletonFollower : MonoBehaviour
{
    public Transform boneParent;
    Transform playerBoneParent;

    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "mixamorig:Hips")
            {
                boneParent = transform.GetChild(i);
                transform.GetChild(i).name += "CustomChar";
            }
        }

        transform.position = GameObject.Find("Daryien").transform.position;
    }

    void FixedUpdate()
    {
        if (playerBoneParent == null)
            GetPlayerBoneParent();

        boneParent.transform.position = Vector3.MoveTowards(boneParent.position, playerBoneParent.position, 100f);
        boneParent.transform.rotation = playerBoneParent.rotation;

        LeftLeg();
        RightLeg();
        Spine();
        LeftArm();
        RightArm();
    }

    void Update()
    {
       // DisableDaryien();
    }

    void DisableDaryien()
    {
        Transform daryien = GameObject.Find("Daryien").transform;

        for (int i = 0; i < daryien.childCount; i++)
        {
            if (daryien.GetChild(i).name != "mixamorig:Hips")
                daryien.GetChild(i).gameObject.SetActive(false);
        }

        Transform head = GameObject.Find("mixamorig:Head").transform;

        for (int i = 0; i < head.childCount; i++)
        {
            if (head.GetChild(i).name != "mixamorig:HeadTop_End")
                head.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Spine()
    {
        boneParent.transform.GetChild(2).position = playerBoneParent.transform.GetChild(2).position;
        boneParent.transform.GetChild(2).rotation = playerBoneParent.transform.GetChild(2).rotation;

        boneParent.transform.GetChild(2).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).rotation;

        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).rotation;

        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).rotation;

        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation;

        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation;
    }

    void RightArm()
    {
        //Right Shoulder
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).rotation;

        //Right Arm
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).rotation;

        //Right ForeArm
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).rotation;

        //Right Hand
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Right Hand Index 1
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Right Hand Index 2
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Right Hand Index 3
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Right Hand Index 4
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Right Hand Thumb 1
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).rotation;

        //Right Hand Thumb 2
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation;

        //Right Hand Thumb 3
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation;

        //Right Hand Thumb 4
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation;
    }

    void LeftArm()
    {
        //Left Shoulder
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Arm
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left ForeArm
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand Index 1
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand Index 2
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand Index 3
        // boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand Index 4
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        //Left Hand Thumb 1
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).rotation;

        //Left Hand Thumb 2
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).rotation;

        //Left Hand Thumb 3
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).rotation;

        //Left Hand Thumb 4
        //  boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation;
    }

    void LeftLeg()
    {
        boneParent.transform.GetChild(0).position = playerBoneParent.transform.GetChild(0).position;
        boneParent.transform.GetChild(0).rotation = playerBoneParent.transform.GetChild(0).rotation;

        boneParent.transform.GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(0).GetChild(0).rotation;

        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).rotation;

        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;

        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;
    }

    void RightLeg()
    {
        boneParent.transform.GetChild(1).position = playerBoneParent.transform.GetChild(1).position;
        boneParent.transform.GetChild(1).rotation = playerBoneParent.transform.GetChild(1).rotation;

        boneParent.transform.GetChild(1).GetChild(0).position = playerBoneParent.transform.GetChild(1).GetChild(0).position;
        boneParent.transform.GetChild(1).GetChild(0).rotation = playerBoneParent.transform.GetChild(1).GetChild(0).rotation;

        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).rotation;

        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).rotation;

        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).position;
        boneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation = playerBoneParent.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).rotation;
    }

    void GetPlayerBoneParent()
    {
        playerBoneParent = GameObject.Find("mixamorig:Hips").transform;
        // boneParent.parent = playerBoneParent.parent.transform;
    }
}
