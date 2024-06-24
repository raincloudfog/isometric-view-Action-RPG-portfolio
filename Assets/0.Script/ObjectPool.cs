using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour//,new() 
{
    public T _value;

    public Queue<T> _valueQueue = new Queue<T>();

    public ObjectPool()
    {

    }

    public ObjectPool(T initialPrefab, int instialSize = 10)   
    {
        _value = initialPrefab;
        for (int i = 0; i < instialSize; i++)
        {
            AddObejctToPool();
        }
    }

    private T AddObejctToPool()
    {
        T obj = Object.Instantiate(_value);
        obj.gameObject.SetActive(false);
        _valueQueue.Enqueue(obj);
        return obj;
    }

    public T GetValue()
    {
        if( _valueQueue.Count <= 0 )
        {
            AddObejctToPool();
        }

        T obj = _valueQueue.Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReturunValue(T obj)
    {
        obj.gameObject.SetActive(false);
        _valueQueue.Enqueue(obj);
    }
}


public class ObjectPool
{
    public GameObject _value;

    public Queue<GameObject> _valueQueue = new Queue<GameObject>();

    public ObjectPool()
    {

    }

    public ObjectPool(GameObject initialPrefab, int instialSize = 10)
    {
        _value = initialPrefab;
        for (int i = 0; i < instialSize; i++)
        {
            AddObejctToPool();
        }
    }

    private GameObject AddObejctToPool()
    {
        GameObject obj = Object.Instantiate(_value);
        obj.gameObject.SetActive(false);
        _valueQueue.Enqueue(obj);
        return obj;
    }

    public GameObject GetValue()
    {
        if (_valueQueue.Count <= 0)
        {
            AddObejctToPool();
        }

        GameObject obj = _valueQueue.Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReturunValue(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        _valueQueue.Enqueue(obj);
    }
}
