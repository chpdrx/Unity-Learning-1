using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHolder : MonoBehaviour
{
    // статы персонажа
    public static float Damage = 1.0f;
    public static float MagicPower = 10.0f;
    public static float Haste = 1.6f;
    public static float CritRate = 0.1f;
    public static float CritDamage = 1f;
    public static float Regen = 1.0f;
    public static float SkillCD = 1.6f;
    public static float Speed = 4.0f;
    public static float Health = 100.0f;
    public static List<Sprite> _inventory = new();

    // текущий урон персонажа
    public static float Current_Damage = Damage + (Damage*Crit());
    public static float Skill_Damage = MagicPower;

    // какой баф активен
    public static bool HasteBuff = false;
    public static bool CdBuff = false;
    public static bool DamageUp = false;
    public static float NoDamageChance = 0;
    public static bool FullRegenChance = false;
    // шансы и значения бафов
    public static float HasteChance = 0;
    public static float HasteValue = 0;
    public static float CdChance = 0;
    public static float CdValue = 0;
    public static float DamageUpChance = 0;
    public static float DamageUpValue = 0;

    // наличие легендарок
    public static bool woodleg = false;
    public static bool umishereye = false;
    public static float dokugapotato = 0;
    public static bool lukanrage = false;
    public static bool bakaimpact = false;

    // расчёт будет ли крит
    public static float Crit()
    {
        if (Random.Range(0, 1.0f) <= CritRate)
        {
            return CritDamage;
        }
        else return 0.0f;
    }
}
