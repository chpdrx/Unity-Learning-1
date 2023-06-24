using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropLoot : MonoBehaviour, IInteractable
{
    [SerializeField] private string _descripton;
    // разные грейды вещей в разные списки
    [SerializeField] private List<Items> _normalitems;
    [SerializeField] private List<Items> _mediumitems;
    [SerializeField] private List<Items> _epicitems;
    [SerializeField] private List<Items> _legendaryitems;
    // вероятности на каждый грейд
    [SerializeField] private float _normalRate;
    [SerializeField] private float _mediumRate;
    [SerializeField] private float _epicRate;

    public string InteractionPrompt => _descripton;

    // вызывается при взаимодействии с объектом, дающим лут
    public void Interact(PlayerDoInteract interactor)
    {
        string grade = GradeSelect();
        if (grade == "Normal") NormalSelect();
        if (grade == "Medium") MediumSelect();
        if (grade == "Epic") EpicSelect();
        if (grade == "Legendary") LegendarySelect();
    }

    // рандомный выбор грейда предмета
    private string GradeSelect()
    {
        var rand = Random.Range(0, 100.0f);
        if (rand <= _normalRate) return "Normal";
        else if (rand <= _mediumRate) return "Medium";
        else if (rand <= _epicRate) return "Epic";
        else return "Legendary";
    }

    // выбор предмета из списка обычного грейда
    private void NormalSelect()
    {
        var rand = Random.Range(0, _normalitems.Capacity);
        Instantiate(_normalitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // выбор предмета из списка среднего грейда
    private void MediumSelect()
    {
        var rand = Random.Range(0, _mediumitems.Capacity);
        Instantiate(_mediumitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // выбор предмета из списка эпик грейда
    private void EpicSelect()
    {
        var rand = Random.Range(0, _epicitems.Capacity);
        Instantiate(_epicitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }

    // выбор предмета из списка легендарного грейда
    private void LegendarySelect()
    {
        var rand = Random.Range(0, _legendaryitems.Capacity);
        Instantiate(_legendaryitems[rand], gameObject.transform.position, gameObject.transform.rotation);
    }
}
