using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClothingSwapper : MonoBehaviour
{
    public GameObject customShoes;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            GameObject custom = Instantiate(customShoes);
            custom.AddComponent<SkeletonFollower>();
        }
    }
}
