using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class PlayerTake : MonoBehaviour, IPlayerInteractable
{
    // ����� ���������
    private float max_health;
    // ����� � ui
    [SerializeField] UIController _gui;
    // ����������� ������ ��
    private bool canRegen = true;
    private float regen_buffer;
    // ����������� ������������ ���� LukanRage
    private bool canlukanrage = true;
    // �������� ���������
    private Animator anim;

    private void Start()
    {
        max_health = StatHolder.Health;
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        // ���� ������� ����� ������ � ������� �� ���� �������������, �� �������� ��
        if (canRegen && StatHolder.Health < max_health) 
        {
            StartCoroutine(Regeneration());
        }

        if (StatHolder.woodleg) WoodenLegDeath();
        if (StatHolder.umishereye) TakeDamage(1000.0f);
    }

    // ����������� �� � �� � 5 ������
    private IEnumerator Regeneration()
    {
        canRegen = false;
        // ���� ���� ���� ���� ������, �� �������� ����� ��� �����
        if (StatHolder.FullRegenChance) StartCoroutine(FullRegen());
        StatHolder.Health += StatHolder.Regen;
        _gui.HPBarController(-StatHolder.Regen);
        yield return new WaitForSeconds(5.0f);
        canRegen = true;
    }

    // ���� ������������ �� �� 5%
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

    // ������ ����, ��� ������ ��� �������������� � ����������
    public void DamageInteractable(EnemyDamage interactor, float damage)
    {
        TakeDamage(damage);
    }

    // ��������� ����� ����������
    private void TakeDamage(float damageCount)
    {
        StatHolder.Health -= damageCount * NoDamage(); // ���� �������� �� ����������� �� �������� ����
        Debug.Log(StatHolder.Health);
        StartCoroutine(AnimationTakeDamage());
        _gui.HPBarController(damageCount);
        // ���� �� ���� ��� ����� ����, �� �������� PlayerDeath
        if (StatHolder.Health <= 0) PlayerDeath();
        // ���� �� ���� 15% �������� LukanRage ��� ������� ���� � ���������� ��
        if (StatHolder.Health <= max_health / 7.0f && canlukanrage && StatHolder.lukanrage) StartCoroutine(LukanRage());
    }

    // �������� ��������� �����
    private IEnumerator AnimationTakeDamage()
    {
        anim.SetBool("isTake", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isTake", false);
    }

    // ����������� �� �������� ���� � ������ StatHolder.NoDamageChance
    private float NoDamage()
    {
        if (Random.Range(0, 100.0f) <= StatHolder.NoDamageChance)
        {
            return 0.0f;
        }
        else return 1.0f;
    }

    // ������ ���� LukanRage
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

    // ��������� ������ ������
    private void PlayerDeath()
    {
        // ���� ���� ���� PotatoOfDokuga, �� ��������������� ���� �� ������ ������ � ������
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

    // ��������� ������ ����� 10 ����� �� ���� WoodenLeg
    private void WoodenLegDeath()
    {
        StatHolder.woodleg = false;
        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(600.0f);
        // �������� ����� �� �������� ���� � 2 ���� ����� ��������� �����
        StatHolder.NoDamageChance /= 2;
        TakeDamage(1000.0f);
    } 
}
