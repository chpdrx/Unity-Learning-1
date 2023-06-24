using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLeg : Items, IInteractable
{
    // статы предмета
    [SerializeField] private float _damage = 0;
    [SerializeField] private float _magicpower = 0;
    [SerializeField] private float _haste = 0;
    [SerializeField] private float _critrate = 0;
    [SerializeField] private float _critdamage = 0;
    [SerializeField] private float _regen = 0;
    [SerializeField] private float _skillcd = 0;
    [SerializeField] private float _speed = 0;
    [SerializeField] private bool _woodleg;

    public override void Stats()
    {
        StatHolder.Damage += _damage;
        StatHolder.MagicPower += _magicpower;
        StatHolder.Haste -= (StatHolder.Haste / 100) * _haste;
        StatHolder.CritRate += (StatHolder.CritRate / 100) * _critrate;
        StatHolder.CritDamage += (StatHolder.CritDamage / 100) * _critdamage;
        StatHolder.Regen += _regen;
        StatHolder.SkillCD -= (StatHolder.SkillCD / 100) * _skillcd;
        StatHolder.Speed += (StatHolder.Speed / 100) * _speed;
        StatHolder.woodleg = _woodleg;
    }
}
