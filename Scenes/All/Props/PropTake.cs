using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DissolveExample;

public class PropTake : MonoBehaviour, IEnemyInteractable
{
    // текстовое поле, с которого можно что то забирать у объекта
    [SerializeField] private string _name;
    // хп этого объекта
    [SerializeField] private float health = 100.0f;

    // через интерфейс возвращает значение _name объекта туда, где вызывается интерфейс
    public string InteractionPrompt => _name;

    // префабы визуальных эффектов
    private SpawnEffect _particles;
    private DissolveChilds _shader;

    private void Start()
    {
        _particles = gameObject.GetComponent<SpawnEffect>();
        _shader = gameObject.GetComponent<DissolveChilds>();
        _particles.enabled = false;
        _shader.enabled = false;
    }

    // логика того, что делать при взаимодействии с объектом
    public void EnemyInteractable(DoDamage interactor, float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            StartCoroutine(DeathAnimation());
            Destroy(gameObject, 4f);
        }
        Debug.Log(health);
    }

    private IEnumerator DeathAnimation()
    {
        _particles.enabled = true;
        _shader.enabled = true;
        yield return new WaitForSeconds(2f);
        Drop();
    }

    private void Drop()
    {
        gameObject.GetComponent<EnemyLoot>().LootSelect();
    }
}
