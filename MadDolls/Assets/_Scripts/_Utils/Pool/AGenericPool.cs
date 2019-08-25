using System.Collections.Generic;
using UnityEngine;

public abstract class AGenericPool<T> : MonoBehaviour where T : APoolObject
{
    [SerializeField] private T prefab;

    public static AGenericPool<T> Instance { get; private set; }
    private Queue<T> objects = new Queue<T>();
    private Transform tf;
    public int Count { private set; get; }

    private void Awake()
    {
        Instance = this;
        tf = transform;
        Count = 0;
        KU.StartTimer(DebugPoolCount, .1f, true);
    }

    public void DebugPoolCount() {
        KU.LogPermanent(tf.name, objects.Count + "/" + Count, Color.white, false);
    }

    public T Get()
    {
        if(objects.Count == 0)
        {
            AddObjects(1);
            Count++;
        }
        return objects.Dequeue();
    }

    public T Get(Vector3 startPosition, bool activate = false)
    {
        var obj = Get();

        obj.SetPosition(startPosition);

        if (activate)
            obj.Activate();

        return obj;
    }

    public T Get(Vector3 startPosition, Quaternion startRotation, bool activate = false)
    {
        var obj = Get();

        obj.SetPosition(startPosition);
        obj.SetRotation(startRotation);

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
