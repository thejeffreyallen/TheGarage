using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehicles;

    public float vehicleSpeed;

    public Transform[] pathToFollow;

    float timeTillSpawn;

    void Update()
    {
        if (Time.time >= timeTillSpawn)
        {
            SpawnVehicle();
            timeTillSpawn = Time.time + UnityEngine.Random.Range(3, 8);
        }
    }

    void SpawnVehicle()
    {
        GameObject car = Instantiate(vehicles[UnityEngine.Random.Range(0, vehicles.Length)], pathToFollow[0].position, pathToFollow[0].rotation);

        VehicleDriver vd = car.AddComponent<VehicleDriver>();
        vd.speed = vehicleSpeed;
        vd.path = pathToFollow;
    }
}
