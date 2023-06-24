using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour, IInteractable
{
    [SerializeField] string scene_name;
    // �����, ��� ��� ������� �� ��������� �����.
    public string _prompt = "NextScene";

    // ����� ��������� ���������� �������� _prompt ������� ����, ��� ���������� ���������
    public string InteractionPrompt => _prompt;

    // ������ ����, ��� ������ ��� �������������� � ��������
    public void Interact(PlayerDoInteract interactor)
    {
        SceneManager.LoadScene(scene_name);
    }
}
