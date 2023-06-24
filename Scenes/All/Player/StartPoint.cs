using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private void OnLevelWasLoaded(int level)
    {
        var startpoint = GameObject.FindGameObjectWithTag("Player");
        startpoint.transform.position = gameObject.transform.position;
    }

}
