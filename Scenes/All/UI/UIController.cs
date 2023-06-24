using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using StarterAssets;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // класс с хп игрока
    [SerializeField] PlayerTake player;
    // класс с уроном игрока
    [SerializeField] PlayerDamage player_damage;
    // ввод, из Input System
    public StarterAssetsInputs _input;
    // корневой элемент UI
    public static VisualElement visuals;
    // полоска хп
    public VisualElement hpbar;
    private float hp; // % полоски хп в px
    // меню персонажа
    public VisualElement character;
    // описание предмета
    public VisualElement desc;
    public Label desclabel;
    public Button submit;
    public Button cancel;
    // меню с кнопками и статами
    public Label _stats;
    // кнопки меню
    public Button _menu;
    public Button _exit;

    void Start()
    {
        visuals = GetComponent<UIDocument>().rootVisualElement;
        hp = (Screen.currentResolution.width / 100) * 20;
        _menu = visuals.Q<Button>("Menu");
        _menu.clicked += MenuButton;
        _exit = visuals.Q<Button>("Exit");
        _exit.clicked += ExitButton;
    }

    private void Update()
    {
        if (_input.character_menu)
        {
            _input.character_menu = false;
            OpenCharacter();
        }
    }

    public void HPBarController(float damage)
    {
        hpbar = visuals.Q<VisualElement>("HPBar");
        hpbar.style.width = hp - damage * (hp / StatHolder.Health);
        hp -= damage * (hp / StatHolder.Health);
    }

    public void OpenCharacter()
    {
        character = visuals.Q<VisualElement>("Character");
        if (character.style.visibility == Visibility.Visible)
        {
            character.style.visibility = Visibility.Hidden;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else 
        {
            CharacterStats();
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            character.style.visibility = Visibility.Visible;
        }
    }

    public void CharacterStats()
    {
        _stats = visuals.Q<Label>("Stats");
        _stats.text = ("Damage: " + StatHolder.Damage + " Skill Power: " + StatHolder.MagicPower + " Haste: " + StatHolder.Haste +
            " CritRate: " + StatHolder.CritRate + " CritDamage: " + StatHolder.CritDamage + " Regen: " + StatHolder.SkillCD +
            " Health: " + StatHolder.Health);
    }

    void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ExitButton()
    {
        Application.Quit();
    }
}
