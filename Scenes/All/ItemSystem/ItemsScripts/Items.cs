using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour, IInteractable
{
    [SerializeField] public Sprite item_icon;
    [SerializeField] public string _description;

    // ����� ��������� ���������� �������� _prompt ������� ����, ��� ���������� ���������
    public string InteractionPrompt => _description;

    // ������ ����, ��� ������ ��� �������������� � ��������
    public void Interact(PlayerDoInteract interactor)
    {
        StatHolder._inventory.Add(item_icon);
        Stats();
        Destroy(gameObject, 0.2f);
    }

    public abstract void Stats();
}
