using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGenericPool<T> : MonoBehaviour where T : APoolObject
{
    [SerializeField] private T prefab;

    public static AGenericPool<T> Instance { get; private set; }
    private Queue<T> objects = new Queue<T>();
    private Transform tf;

    private void Awake()
    {
        Instance = this;
        tf = transform;
    }

    public T Get()
    {
        if(objects.Count == 0)
        {
            AddObjects(1);
        }
        return objects.Dequeue();
    }

    public T Get(Vector3 startPosition, bool activate = false)
    {
        if(objects.Count == 0)
        {
            AddObjects(1);
        }

        var obj = objects.Dequeue();

        obj.SetPosition(startPosition);

        if (activate)
            obj.Activate();

        return obj;
    }

    public void Return(T objectToReturn)
    {
        objectToReturn.Reset();
        objects.Enqueue(objectToReturn);
    }

    private void AddObjects(int nb)
    {
        var newObject = Instantiate(prefab, tf);
        newObject.Reset();
        objects.Enqueue(newObject);
    }
}
