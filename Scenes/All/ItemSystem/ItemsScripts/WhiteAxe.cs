using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteAxe : Items, IInteractable
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
    [SerializeField] private float _health = 0;
    [SerializeField] private string _currency = "";
    [SerializeField] private float _nodamagechance = 0;
    [SerializeField] private float _chance = 0;
    [SerializeField] private float _value = 0;
    [SerializeField] private bool _chancefullhp;

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
        StatHolder.Health += _health;
        StatHolder.NoDamageChance += _nodamagechance - (StatHolder.NoDamageChance / 15);
        StatHolder.FullRegenChance = _chancefullhp;
        if (_currency != "") SetBuff();
    }

    public void SetBuff()
    {
        if (_currency == "Haste")
        {
            StatHolder.HasteBuff = true;
            StatHolder.HasteChance = _chance;
            StatHolder.HasteValue = _value;
        }
        if (_currency == "SkillCd")
        {
            StatHolder.CdBuff = true;
            StatHolder.CdChance = _chance;
            StatHolder.CdValue = _value;
        }
        if (_currency == "ChanceDamage")
        {
            StatHolder.DamageUp = true;
            StatHolder.DamageUpChance += _chance;
            StatHolder.DamageUpValue += _value;
        }
        if (_currency == "ChanceMagicUp")
        {
            StatHolder.MagicPower += ChanceMagicUp();
        }
        if (_currency == "ChanceCritRateUp")
        {
            StatHolder.CritRate += ChanceCritUp();
        }
        if (_currency == "ChanceCritDamageUp")
        {
            StatHolder.CritDamage += ChanceCritUp();
        }
        if (_currency == "ChanceRegenUp")
        {
            StatHolder.Regen += ChanceCritUp();
        }
        if (_currency == "Haste,SkillCd")
        {
            StatHolder.HasteBuff = true;
            StatHolder.HasteChance = _chance;
            StatHolder.HasteValue = _value;
            StatHolder.CdBuff = true;
            StatHolder.CdChance = _chance;
            StatHolder.CdValue = _value;
        }
    }

    public float ChanceMagicUp()
    {
        if (Random.Range(0, 1.0f) <= StatHolder.CritRate)
        {
            return _value;
        }
        else return 0.0f;
    }

    public float ChanceCritUp()
    {
        if (Random.Range(0, 100.0f) <= _chance)
        {
            return _value;
        }
        else return 0.0f;
    }
}
