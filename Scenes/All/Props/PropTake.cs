using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DissolveExample;

public class PropTake : MonoBehaviour, IEnemyInteractable
{
    // ��������� ����, � �������� ����� ��� �� �������� � �������
    [SerializeField] private string _name;
    // �� ����� �������
    [SerializeField] private float health = 100.0f;

    // ����� ��������� ���������� �������� _name ������� ����, ��� ���������� ���������
    public string InteractionPrompt => _name;

    // ������� ���������� ��������
    private SpawnEffect _particles;
    private DissolveChilds _shader;

    private void Start()
    {
        _particles = gameObject.GetComponent<SpawnEffect>();
        _shader = gameObject.GetComponent<DissolveChilds>();
        _particles.enabled = false;
        _shader.enabled = false;
    }

    // ������ ����, ��� ������ ��� �������������� � ��������
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
