using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DokugaPotato : Items, IInteractable
{
    // статы предмета
    [SerializeField] private float _chancenotdie = 0;
    [SerializeField] private float _regen = 0;
    [SerializeField] private float _speed = 0;

    public override void Stats()
    {
        StatHolder.Regen += _regen;
        StatHolder.Speed += (StatHolder.Speed / 100) * _speed;
        StatHolder.dokugapotato = _chancenotdie;
    }
}
