using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AikimiRun : Items
{
    // статы предмета
    [SerializeField] private float _damagemultiplier = 0;
    [SerializeField] private float _speed = 0;

    public override void Stats()
    {
        StatHolder.Damage *= _damagemultiplier;
        StatHolder.Speed += (StatHolder.Speed / 100) * _speed;
    }
}
