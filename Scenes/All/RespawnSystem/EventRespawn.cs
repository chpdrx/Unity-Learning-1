using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventRespawn : MonoBehaviour
{
    public static EventRespawn current;
    public event Action<int> RespawnEvent;

    private void Awake()
    {
        if (current == null) current = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void RespawnEnemyEvent(int id)
    {
        RespawnEvent?.Invoke(id);
    }
}
