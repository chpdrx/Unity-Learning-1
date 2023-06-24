using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DissolveExample;
using UnityEngine.AI;

public class EnemyTake : MonoBehaviour, IEnemyInteractable
{
    // текстовое поле, с которого можно что то забирать у объекта
    [SerializeField] private string _name;
    // хп этого объекта
    [SerializeField] public float health = 20.0f;

    private Animator anim;

    // префабы визуальных эффектов
    private SpawnEffect _particles;
    private DissolveChilds _shader;

    // ai врага
    private EnemyAIController _ai;

    // смерть моба
    private bool _isdie = false;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        _particles = gameObject.GetComponent<SpawnEffect>();
        _shader = gameObject.GetComponent<DissolveChilds>();
        _ai = gameObject.GetComponent<EnemyAIController>();
        _particles.enabled = false;
        _shader.enabled = false;
    }

    // логика того, что делать при взаимодействии с объектом, получение урона объектом
    public void EnemyInteractable(DoDamage interactor, float damage)
    {
        if (!_isdie)
        {
            health -= damage;
            // анимация получения урона
            StartCoroutine(TakeDamage());
            _ai.playerInRange = true;
            if (health <= 0.0f)
            {
                // анимация и эффекты смерти
                StartCoroutine(DeathAnimation());
                // эффекты после смерти занимают 4f
                Destroy(gameObject, 5f);
            }
        }
        
    }

    private IEnumerator TakeDamage()
    {
        anim.SetBool("isTake", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isTake", false);
    }

    private IEnumerator DeathAnimation()
    {
        _ai.AiDeath();
        _isdie = true;
        anim.SetBool("isTake", false);
        anim.SetBool("isDie", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isDie", false);
        yield return new WaitForSeconds(0.8f);
        _particles.enabled = true;
        _shader.enabled = true;
        // лут с врага
        Drop();
    }

    private void Drop()
    {
        gameObject.GetComponent<EnemyLoot>().LootSelect();
    }
}
