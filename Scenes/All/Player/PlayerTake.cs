using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class PlayerTake : MonoBehaviour, IPlayerInteractable
{
    // статы персонажа
    private float max_health;
    // класс с ui
    [SerializeField] UIController _gui;
    // возможность регена хп
    private bool canRegen = true;
    private float regen_buffer;
    // возможность использовать легу LukanRage
    private bool canlukanrage = true;
    // аниматор персонажа
    private Animator anim;

    private void Start()
    {
        max_health = StatHolder.Health;
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        // если настало время регена и текущее хп ниже максимального, то регенить хп
        if (canRegen && StatHolder.Health < max_health) 
        {
            StartCoroutine(Regeneration());
        }

        if (StatHolder.woodleg) WoodenLegDeath();
        if (StatHolder.umishereye) TakeDamage(1000.0f);
    }

    // регенерация хп с кд в 5 секунд
    private IEnumerator Regeneration()
    {
        canRegen = false;
        // если есть шанс фулл регена, то вызывать метод для этого
        if (StatHolder.FullRegenChance) StartCoroutine(FullRegen());
        StatHolder.Health += StatHolder.Regen;
        _gui.HPBarController(-StatHolder.Regen);
        yield return new WaitForSeconds(5.0f);
        canRegen = true;
    }

    // шанс восстановить всё хп 5%
    private IEnumerator FullRegen()
    {
        if (Random.Range(0, 100.0f) <= 5.0f)
        {
            regen_buffer = StatHolder.Regen;
            StatHolder.Regen = max_health - StatHolder.Health;
        }
        yield return new WaitForSeconds(0.5f);
        StatHolder.Regen = regen_buffer;
    }

    // логика того, что делать при взаимодействии с персонажем
    public void DamageInteractable(EnemyDamage interactor, float damage)
    {
        TakeDamage(damage);
    }

    // получение урона персонажем
    private void TakeDamage(float damageCount)
    {
        StatHolder.Health -= damageCount * NoDamage(); // урон умножить на вероятность не получить урон
        Debug.Log(StatHolder.Health);
        StartCoroutine(AnimationTakeDamage());
        _gui.HPBarController(damageCount);
        // если хп ниже или равно нулю, то вызывать PlayerDeath
        if (StatHolder.Health <= 0) PlayerDeath();
        // если хп ниже 15% вызывать LukanRage при наличии леги и отсутствии кд
        if (StatHolder.Health <= max_health / 7.0f && canlukanrage && StatHolder.lukanrage) StartCoroutine(LukanRage());
    }

    // анимация получения урона
    private IEnumerator AnimationTakeDamage()
    {
        anim.SetBool("isTake", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isTake", false);
    }

    // вероятность не получить урон с шансом StatHolder.NoDamageChance
    private float NoDamage()
    {
        if (Random.Range(0, 100.0f) <= StatHolder.NoDamageChance)
        {
            return 0.0f;
        }
        else return 1.0f;
    }

    // работа леги LukanRage
    private IEnumerator LukanRage()
    {
        canlukanrage = false;
        var _haste = StatHolder.Haste * 3.0f;
        var _skillcd = StatHolder.SkillCD * 3.0f;
        var _damage = StatHolder.Damage * 3.0f;
        StatHolder.Haste += _haste;
        StatHolder.SkillCD += _skillcd;
        StatHolder.Damage += _damage;
        yield return new WaitForSeconds(10.0f);
        StatHolder.Haste -= _haste;
        StatHolder.SkillCD -= _skillcd;
        StatHolder.Damage -= _damage;
        canlukanrage = true;
    }

    // обработка смерти игрока
    private void PlayerDeath()
    {
        // если есть лега PotatoOfDokuga, то восстанавливать фулл хп вместо смерти с шансом
        if (StatHolder.dokugapotato > 0)
        {
            if (Random.Range(0, 100.0f) <= StatHolder.dokugapotato)
            {
                StatHolder.Health = 100.0f;
            }
        }
        anim.SetBool("isDie", true);
        gameObject.GetComponent<PlayerDamage>().enabled = false;
        gameObject.GetComponent<ThirdPersonController>().enabled = false;
        //_gui.DeathMenu();
    }

    // обработка смерти через 10 минут от леги WoodenLeg
    private void WoodenLegDeath()
    {
        StatHolder.woodleg = false;
        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(600.0f);
        // снижение шанса не получить урон в 2 раза перед нанесенем урона
        StatHolder.NoDamageChance /= 2;
        TakeDamage(1000.0f);
    } 
}
