using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class ArcherDoDamage : PlayerDamage
{
    [SerializeField] private GameObject cast_point;
    [SerializeField] private ParticleSystem skill_animation;

    // взаимодействие по левой кнопки мыши с объектами слоя Damagable
    public override void DamageInteract()
    {
        BuffChecker();
        StartCoroutine(ArrowDelay());
        StartCoroutine(LmbCooldown());
    }

    // задержка перед пуском стрелы
    IEnumerator ArrowDelay()
    {
        yield return new WaitForSeconds(0.7f);
        var arrow = ArrowPool.Instance.Get();
        arrow.transform.SetPositionAndRotation(cast_point.transform.position, transform.rotation);
        arrow.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        arrow.GetComponent<Rigidbody>().velocity = transform.forward * 20;
    }

    // взаимодействие по правой кнопке мыши с объектами слоя Damagable
    public override void SkillInteract()
    {
        BuffChecker();
        StartCoroutine(SkillDelay());
        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillDelay()
    {
        skill_animation.Play();
        yield return new WaitForSeconds(2f);
        var ball = ArcherSkillPool.Instance.Get();
        ball.transform.SetPositionAndRotation(cast_point.transform.position, transform.rotation);
        ball.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ball.GetComponent<Rigidbody>().velocity = transform.forward * 20;
        skill_animation.Pause();
        skill_animation.Clear();
    }
}