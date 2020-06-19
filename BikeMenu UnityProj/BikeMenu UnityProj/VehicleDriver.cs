using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VehicleDriver : MonoBehaviour
{
    public float speed;

    public Transform[] path;
    int targetPoint;

    void OnEnable()
    {
        targetPoint = 1;
    }

    void Update()
    {
        if (transform.position == path[targetPoint].position)
        {
            if (targetPoint == path.Length - 1)
                Destroy(gameObject);

            targetPoint++;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, path[targetPoint].position, speed);
            transform.LookAt(path[targetPoint], Vector3.up);
        }
    }
}
