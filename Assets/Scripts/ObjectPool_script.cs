//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_script : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objectPrefabs;

    private List<GameObject> _objectPool;

    private void Start()
    {
        this._objectPool = new List<GameObject>();
    }

    private void Update()
    {
        //
    }

    public GameObject GetObject(string type)
    {
        foreach (GameObject obj in this._objectPool)
        {
            if ((obj.name == type) && !obj.activeInHierarchy) {
                obj.SetActive(true);
                return obj;
            }
        }

        for (int i = 0; i < this._objectPrefabs.Length; i++)
        {
            if (this._objectPrefabs[i].name == type)
            {
                GameObject obj = (GameObject)Instantiate(this._objectPrefabs[i]);
                obj.name       = type;
                obj.transform.SetParent(GameObject.Find("ObjectPool_object").transform);
                this._objectPool.Add(obj);
                return obj;
            }
        }

        return null;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}
