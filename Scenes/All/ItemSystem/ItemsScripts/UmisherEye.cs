using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmisherEye : Items, IInteractable
{
    // статы предмета
    [SerializeField] private float _chance = 0;
    [SerializeField] private float _value = 0;

    public override void Stats()
    {
        if (Random.Range(0, 100.0f) <= _chance)
        {
            StatHolder.MagicPower *= _value;
        }
        else StatHolder.umishereye = true;
    }
}
