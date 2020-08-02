using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CamController : MonoBehaviour
{
    float moveSpeed = .01f;
    float turnSpeed = .1f;
    float rollSpeed = 1f;

    float zRot;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.forward * -moveSpeed);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.right * -moveSpeed);

        if (!Options.instance.IsMenuActive())
            transform.Rotate((Vector3.up * Input.GetAxis("Mouse X") * turnSpeed) + (Vector3.right * -Input.GetAxis("Mouse Y") * turnSpeed));

        if (Input.GetKey(KeyCode.Q))
            zRot += .1f;
        else if (Input.GetKey(KeyCode.E))
            zRot -= .1f;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRot);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed *= 3;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed = .01f;
    }
}
