using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private string _descripton;
    // ������ ������ ����� � ������ ������
    [SerializeField] private List<Items> _normalitems;
    [SerializeField] private List<Items> _mediumitems;
    [SerializeField] private List<Items> _epicitems;
    [SerializeField] private List<Items> _legendaryitems;
    // ����������� �� ������ �����
    [SerializeField] private float _normalRate;
    [SerializeField] private float _mediumRate;
    [SerializeField] private float _epicRate;
    
    // ��������� ����� ������ ��������
    public void LootSelect()
    {
        var rand = Random.Range(0, 100.0f);
        if (rand <= _normalRate) NormalSelect();
        else if (rand <= _mediumRate) MediumSelect();
        else if (rand <= _epicRate) EpicSelect();
        else LegendarySelect();
    }

    // ����� �������� �� ������ �������� ������
    private void NormalSelect()
    {
        var rand = Random.Range(0, _normalitems.Capacity);
        Instantiate(_normalitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // ����� �������� �� ������ �������� ������
    private void MediumSelect()
    {
        var rand = Random.Range(0, _mediumitems.Capacity);
        Instantiate(_mediumitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // ����� �������� �� ������ ���� ������
    private void EpicSelect()
    {
        var rand = Random.Range(0, _epicitems.Capacity);
        Instantiate(_epicitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // ����� �������� �� ������ ������������ ������
    private void LegendarySelect()
    {
        var rand = Random.Range(0, _legendaryitems.Capacity);
        Instantiate(_legendaryitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }
}
