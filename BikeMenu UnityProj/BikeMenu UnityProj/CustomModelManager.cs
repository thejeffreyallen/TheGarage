/**
*To be used as a data structure for managing custom bike part models. 
*/
using UnityEngine;
using System.Collections;
using UnityEngine;

public class CustomModelManager : MonoBehaviour
{
    public GameObject[] partsCollection;
    private int index = 0;
    private int size;
    private GameObject originalMesh;

    public CustomModelManager(GameObject[] gameObjects)
    {
        this.partsCollection = gameObjects;
        this.size = partsCollection.Length;
    }

    public GameObject GetNextObject()
    {
        GameObject temp;
        if (index < partsCollection.Length)
        {
            temp = partsCollection[index];
            index++;
        }
        else
        {
            temp = partsCollection[0];
            index = 1;
        }
        return temp;
    }

    public GameObject GetPreviousObject()
    {
        GameObject temp;
        if (index > 0)
        {
            temp = partsCollection[index - 1];
            index--;
        }
        else
        {
            index = size - 1;
            temp = partsCollection[index];
            index--;
        }
        return temp;
    }

    public void SetOriginal(GameObject original)
    {
        this.originalMesh = original;
    }

    public GameObject GetOriginal()
    {
        return this.originalMesh;
    }

    public int Size()
    {
        return size;
    }


}
