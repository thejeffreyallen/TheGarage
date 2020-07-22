using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HouseCam : MonoBehaviour
{
    Transform cam;

    public int currentListVal;
    List<Transform> camPositions = new List<Transform>();

    public float moveSpeed;
    public float rotateSpeed;

    Vector3 targetPos;
    Quaternion targetRot;

    public Text headerText;

    bool moveForward;
    Vector3 moveToPos;

    void Update()
    {
        if (cam.position != targetPos)
        {
            cam.position = Vector3.MoveTowards(cam.position, targetPos, moveSpeed * Time.deltaTime);
            cam.rotation = Quaternion.RotateTowards(cam.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        if (moveForward)
        {
            cam.position = Vector3.MoveTowards(cam.position, moveToPos, Vector3.Distance(cam.position, moveToPos) * Time.deltaTime);       
        }
    }

    public void SetTargetPos(int listValue)
    {
        if (cam.position == targetPos)
        {
            if ((currentListVal + listValue) >= camPositions.Count)
                currentListVal = 0;
            else if ((currentListVal + listValue) < 0)
                currentListVal = camPositions.Count - 1;
            else
                currentListVal += listValue;

            targetPos = camPositions[currentListVal].position;
            targetRot = camPositions[currentListVal].rotation;

            SetHeader();
        }
    }

    void SetHeader()
    {
        if (currentListVal == 0)
            headerText.text = "Go Ride";
        else if (currentListVal == 1)
            headerText.text = "The Wardrobe";
        else if (currentListVal == 2)
            headerText.text = "The Garage";
        else if (currentListVal == 3)
            headerText.text = "View Captures";
    }

    public void FindCam()
    {
        GetCamPositions();

        cam = HouseRoomLoader.instance.room.transform.GetChild(0).transform;
        targetPos = cam.position;
        currentListVal = 0;
        SetHeader();
    }

    public void GetCamPositions()
    {
        camPositions.Clear();

        GameObject parent = new GameObject();

        parent = HouseRoomLoader.instance.room.transform.GetChild(2).gameObject;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            camPositions.Add(parent.transform.GetChild(i));
        }
    }

    public void MoveCamForward()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            float x = (cam.position.x - hit.point.x) * .75f;
            float z = (cam.position.z - hit.point.z) * .75f;

            moveToPos = new Vector3(x, cam.position.y, z);
            moveForward = true;
        }
    }

    public bool HasCamMovedForward()
    {
        return cam.position == moveToPos;
    }
}
