using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class StickerManager : MonoBehaviour
{
    public Material decalMat;
    Dictionary<int, GameObject> partList = new Dictionary<int, GameObject>();

    void Awake()
    {
    }

    public void SetStickerDecal(int key)
    {
        GameObject copy = PartMaster.instance.GetPart(key);
        if (copy == null)
        {
            Debug.Log("Can't find part at SetStickerDecal()");
            return;
        }   
        copy = Instantiate(copy, PartMaster.instance.GetPart(key).transform);
        MeshRenderer mr = copy.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            Debug.Log("Part doesn't have a MeshRenderer at SetStickerDecal()");
            Destroy(copy);
            return;
        }
        copy.transform.localPosition = new Vector3(0f, 0f, 0f);
        partList.Add(key, copy);
        Material[] matCopy = new Material[1];
        matCopy[0] = new Material(decalMat.shader);
        matCopy[0].CopyPropertiesFromMaterial(decalMat);
        mr.materials = matCopy;
    }

    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.S) && !partList.ContainsKey(0))
        {
            SetStickerDecal(0);
        }
        if (Input.GetKey(KeyCode.X) && partList.ContainsKey(0))
        {
            Destroy(partList[0]);
            partList.Remove(0);
        }
        */
    }

}
