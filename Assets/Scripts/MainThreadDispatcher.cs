using UnityEngine;
using System.Collections.Generic;
public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MainThreadDispatcher");
                instance = go.AddComponent<MainThreadDispatcher>();
            }
            return instance;
        }
    }

    public void Enqueue(System.Action action)
    {
        lock (queueLock)
        {
            actionQueue.Enqueue(action);
        }
    }

    private readonly object queueLock = new object();
    private readonly Queue<System.Action> actionQueue = new Queue<System.Action>();

    private void Update()
    {
        lock (queueLock)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }
}
