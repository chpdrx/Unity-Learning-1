using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerRespawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies = new();
    [SerializeField] private float _timer = 30.0f;
    private bool _canSpawn = true;

    private void Update()
    {
        if (_canSpawn) StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        _canSpawn = false;
        Instantiate(_enemies[Random.Range(0, _enemies.Capacity)], gameObject.transform.position, gameObject.transform.rotation);
        yield return new WaitForSeconds(_timer);
        _canSpawn = true;
    }
}
