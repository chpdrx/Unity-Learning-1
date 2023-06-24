using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ClassChoose : MonoBehaviour
{
    // �������� ������� UI
    public static VisualElement visuals;

    // ������ ������ ���������
    public Button _warrior;
    public Button _archer;
    public Button _villian;

    // �������������� ������
    public Button _back;
    public Button _start;

    // �������� ���������
    public Label _description;

    // �������� ������ ����� �������
    private string scene_data;

    private void Start()
    {
        visuals = GetComponent<UIDocument>().rootVisualElement;
        _warrior = visuals.Q<Button>("Warrior");
        _warrior.clicked += WarriorButton;
        _archer = visuals.Q<Button>("Archer");
        _archer.clicked += ArcherButton;
        _villian = visuals.Q<Button>("Villian");
        _villian.clicked += VillianButton;
        _back = visuals.Q<Button>("Back");
        _back.clicked += Back;
        _start = visuals.Q<Button>("Start");
        _start.clicked += NextScene;
        _description = visuals.Q<Label>("Description");
    }

    void WarriorButton()
    {
        scene_data = "Warrior";
        _description.text = "Warrior";
    }

    void ArcherButton()
    {
        scene_data = "Archer";
        _description.text = "Archer";
    }

    void VillianButton()
    {
        _description.text = "Villian";
    }

    void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void NextScene()
    {
        SceneManager.LoadScene("Start Scene");
    }

    private void OnDestroy()
    {
        DataSceneHolder.ClassChoose = scene_data;
    }
}
