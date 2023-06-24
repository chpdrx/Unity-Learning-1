using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRespawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies = new();
    public int pointID;

    private void Start()
    {
        EventRespawn.current.RespawnEvent += Respawn;
    }

    private void Respawn(int _triggerID)
    {
        if (_triggerID == pointID) 
        {
            Instantiate(_enemies[Random.Range(0, _enemies.Capacity)], gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventRespawn.current.RespawnEvent -= Respawn;
    }
}
