using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustemSoul : Items, IInteractable
{
    // статы предмета
    [SerializeField] private float _chance = 0;
    [SerializeField] private float _value = 0;

    public override void Stats()
    {
        StatHolder.Damage *= 2;
        StatHolder.DamageUp = true;
        StatHolder.DamageUpChance += _chance;
        StatHolder.DamageUpValue += _value;
    }
}
