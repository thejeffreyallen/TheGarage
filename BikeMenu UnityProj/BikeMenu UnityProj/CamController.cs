using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CamController : MonoBehaviour
{
    Transform bike;

    List<Vector3> camPositions = new List<Vector3>();

    void OnEnable()
    {
        bike = GameObject.Find("BMX").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, bike.position) <= 4)
        {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * .25f);
                transform.Rotate(Vector3.right * -Input.GetAxis("Mouse Y") * .25f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                if (Input.GetKey(KeyCode.W))
                    transform.Translate(Vector3.forward * .005f);

                if (Input.GetKey(KeyCode.S))
                    transform.Translate(Vector3.back * .005f);

                if (Input.GetKey(KeyCode.D))
                    transform.Translate(Vector3.right * .005f);

                if (Input.GetKey(KeyCode.A))
                    transform.Translate(Vector3.left * .005f);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            AddPositionToList();
        }
        else
        {
            transform.position = camPositions[0];
            camPositions.RemoveAt(0);
        }
    }

    void AddPositionToList()
    {
        camPositions.Insert(0, transform.position);

        if (camPositions.Count >= 100)
            camPositions.RemoveAt(camPositions.Count);
    }
}
