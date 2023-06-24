using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FudouMind : Items, IInteractable
{
    // статы предмета
    [SerializeField] private float _damage = 0;

    public override void Stats()
    {
        StatHolder.Damage = (StatHolder.Damage + StatHolder.MagicPower + _damage) * 2;
        StatHolder.MagicPower = StatHolder.Damage;
    }
}
