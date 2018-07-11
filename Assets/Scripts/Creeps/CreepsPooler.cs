using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreepsPooler : MonoBehaviour {

    public static CreepsPooler current;
    public GameObject pooledObject;
    int PoolSize = 64;
    public bool IsExpandable = true;

    public List<GameObject> Pool;

	void Awake () {
        current = this;
	}

    void Start()
    {
        Pool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.parent = this.transform;
            obj.transform.position = CreepPath.instance.path[0];
            obj.name = "Creep_" + i;
            obj.SetActive(false);
            Pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (!Pool[i].activeInHierarchy)
            {
                return Pool[i];
            }

        }

        if (IsExpandable)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.transform.parent = this.transform;
            obj.name = "ExtraCreep";
            Pool.Add(obj);
            return obj;
        }

        Debug.Log("ObjectPool is full and can't epxand");
        return null;
    }
    
}
