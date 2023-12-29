using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    [SerializeField]
    private GameObject[] objectPrebs; /*monster from the library*/

    private List<GameObject> pooledObjects = new List<GameObject>(); /*monster already dead*/

    /*get the object from the pooledObject first,
     if cannot find it, then create new monster*/
    public GameObject GetObject(string type)
    {
        foreach(GameObject go in pooledObjects)
        {
            /*It is not active in hierarchy*/
            if(go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        for (int i = 0; i < objectPrebs.Length; i++)
        {
            if(objectPrebs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrebs[i]);

                pooledObjects.Add(newObject);

                newObject.name = type;
                return newObject;
            }
        }

        return null;
    }

    /*Disactive the gameObject*/
    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
