using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRespawn : MonoBehaviour
{
    private bool _canRespawn = true;
    public int _triggerID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _canRespawn)
        {
            _canRespawn = false;
            EventRespawn.current.RespawnEnemyEvent(_triggerID);
            Destroy(gameObject);
        }
    }
}
