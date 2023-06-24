using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T projectile_prefab;

    private Queue<T> projectileShots = new Queue<T>();
    public static ObjectPool<T> Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public T Get()
    {
        if (projectileShots.Count == 0)
        {
            AddShots(1);
        }
        return projectileShots.Dequeue();
    }

    private void AddShots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T projectileShot = Instantiate(projectile_prefab);
            projectileShot.gameObject.SetActive(false);
            projectileShots.Enqueue(projectileShot);
        }
    }

    public void ReturnToPull(T projectileShot)
    {
        projectileShot.GetComponent<Rigidbody>().velocity = transform.forward * 0;
        projectileShot.gameObject.SetActive(false);
        projectileShots.Enqueue(projectileShot);
    }
}
