using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    // ��������� ����, � �������� ����� ��� �� �������� � �������
    [SerializeField] private string _prompt;

    // ����� ��������� ���������� �������� _prompt ������� ����, ��� ���������� ���������
    public string InteractionPrompt => _prompt;

    // ������ ����, ��� ������ ��� �������������� � ��������
    public void Interact(PlayerDoInteract interactor)
    {
        Debug.Log("Eat food!");
    }
}
